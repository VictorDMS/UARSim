using UnityEngine;
using System.Linq;

public class VehicleAutomaticMovement : AutomaticMovement
{
    public enum VehicleAutomaticMovementState { LEFT_ROTATING, RIGHT_ROTATING, STRAIGHT };

    public const float VEHICLE_ROTATION_SPEED = 3.0f;//0.5f;
    public const float VEHICLE_STRAIGHT_SPEED = 0.005f;//0.01f;
    private const float DISTANCE_ACHIEVED_WAYPOINT = 1.3f;
    private const float ANGLE_ACHIEVED_WAYPOINT = 20.0f;//4.0f;
    
    public override void buildWaypointsPathForCurrentConfiguration(Transform CurrentPosition)
    {
        if ((ConfigBehavior.LightCurrentConfig == ConfigBehavior.VehicleRobotConfiguration.RIGHT) ||
            (ConfigBehavior.HeavyCurrentConfig == ConfigBehavior.VehicleRobotConfiguration.RIGHT) ||
            (ConfigBehavior.UltraCurrentConfig == ConfigBehavior.VehicleRobotConfiguration.RIGHT))
            basicPathCreator(CurrentPosition, (uint)ConfigBehavior.VehicleRobotConfiguration.RIGHT);
        else if ((ConfigBehavior.LightCurrentConfig == ConfigBehavior.VehicleRobotConfiguration.LEFT) ||
                (ConfigBehavior.HeavyCurrentConfig == ConfigBehavior.VehicleRobotConfiguration.LEFT) ||
                (ConfigBehavior.UltraCurrentConfig == ConfigBehavior.VehicleRobotConfiguration.LEFT))
            basicPathCreator(CurrentPosition, (uint)ConfigBehavior.VehicleRobotConfiguration.LEFT);
        else if ((ConfigBehavior.LightCurrentConfig == ConfigBehavior.VehicleRobotConfiguration.RANDOM) ||
                (ConfigBehavior.HeavyCurrentConfig == ConfigBehavior.VehicleRobotConfiguration.RANDOM) ||
                (ConfigBehavior.UltraCurrentConfig == ConfigBehavior.VehicleRobotConfiguration.RANDOM))
            basicPathCreator(CurrentPosition, (uint)ConfigBehavior.VehicleRobotConfiguration.RANDOM);
    }
    
    public override void basicPathCreator(Transform CurrentPosition, uint Config)
    {
        const uint WAYPOINTS_COUNT = 500;
        Direction CurrentDirection = new Direction();
        uint RowIndex = 0, ColumnIndex = 0, Steps = 0;
        MazeWaypointsPath = new Vector3[500];
        MazeWaypointsPathDirections = new Direction[500];
        WayPointsCurrentPosition = 0;

        findClosestFloor(CurrentPosition, ref CurrentDirection, ref RowIndex, ref ColumnIndex, false);

        MazeWaypointsPath[Steps] = MazeMatrix3DPoints[RowIndex][ColumnIndex];
        MazeWaypointsPathDirections[Steps] = CurrentDirection;
        ++Steps;

        while (Steps < WAYPOINTS_COUNT)
        {
            // enumerate next movement in order of priority
            Direction[] PriorizedDirections = new Direction[NumberOfDirections]; // declare array to hold priorities
            switch (CurrentDirection)
            { // populate array according to right hand rule results from table above
                case Direction.NORTH:
                    PriorizedDirections[0] = Direction.EAST; PriorizedDirections[1] = Direction.NORTH; PriorizedDirections[2] = Direction.WEST; PriorizedDirections[3] = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    PriorizedDirections[0] = Direction.WEST; PriorizedDirections[1] = Direction.SOUTH; PriorizedDirections[2] = Direction.EAST; PriorizedDirections[3] = Direction.NORTH;
                    break;
                case Direction.EAST:
                    PriorizedDirections[0] = Direction.SOUTH; PriorizedDirections[1] = Direction.EAST; PriorizedDirections[2] = Direction.NORTH; PriorizedDirections[3] = Direction.WEST;
                    break;
                case Direction.WEST:
                    PriorizedDirections[0] = Direction.NORTH; PriorizedDirections[1] = Direction.WEST; PriorizedDirections[2] = Direction.SOUTH; PriorizedDirections[3] = Direction.EAST;
                    break;
            }

            bool RandomMovement = false;
            bool StepDone = true;
            if ((ConfigBehavior.VehicleRobotConfiguration)Config == ConfigBehavior.VehicleRobotConfiguration.LEFT)
            {
                Direction TempDirection = PriorizedDirections[0];
                PriorizedDirections[0] = PriorizedDirections[2];
                PriorizedDirections[2] = TempDirection;
            }
            else if ((ConfigBehavior.VehicleRobotConfiguration)Config == ConfigBehavior.VehicleRobotConfiguration.RANDOM)
            {
                RandomMovement = true;
            }

            for (ushort i = 0; i < NumberOfDirections; ++i)
            {
                if (RandomMovement)
                {
                    if (StepDone)
                    {
                        System.Random r = new System.Random();
                        PriorizedDirections = PriorizedDirections.OrderBy(x => r.Next()).ToArray();
                        StepDone = false;
                    }
                }

                if ((PriorizedDirections[i] == Direction.NORTH) && (MazeMatrix3DPoints[RowIndex + 1][ColumnIndex] != new Vector3(0, 0, 0)))
                {
                    RowIndex += 2;
                    CurrentDirection = Direction.NORTH;
                    StepDone = true;
                    break;
                }
                else if ((PriorizedDirections[i] == Direction.SOUTH) && (MazeMatrix3DPoints[RowIndex - 1][ColumnIndex] != new Vector3(0, 0, 0)))
                {
                    RowIndex -= 2;
                    CurrentDirection = Direction.SOUTH;
                    StepDone = true;
                    break;
                }
                else if ((PriorizedDirections[i] == Direction.EAST) && (MazeMatrix3DPoints[RowIndex][ColumnIndex + 1] != new Vector3(0, 0, 0)))
                {
                    ColumnIndex += 2;
                    CurrentDirection = Direction.EAST;
                    StepDone = true;
                    break;
                }
                else if ((PriorizedDirections[i] == Direction.WEST) && (MazeMatrix3DPoints[RowIndex][ColumnIndex - 1] != new Vector3(0, 0, 0)))
                {
                    ColumnIndex -= 2;
                    CurrentDirection = Direction.WEST;
                    StepDone = true;
                    break;
                }
            }
            if (MazeMatrix3DPoints[RowIndex][ColumnIndex] != new Vector3(0, 0, 0))
            {
                MazeWaypointsPath[Steps] = MazeMatrix3DPoints[RowIndex][ColumnIndex];
                MazeWaypointsPathDirections[Steps] = CurrentDirection;
            }
            else
            {
                Debug.Log("Unexpected error building the WaypointsPath for Right configuration");
                break;
            }
            ++Steps;
        }
    }

    public override uint getAutomaticMovementState(Transform CurrentPosition)
    {
        VehicleAutomaticMovementState State;
        float DistanceFromCurrentWaypoint = (MazeWaypointsPath[WayPointsCurrentPosition] - CurrentPosition.localPosition).magnitude;
        float AngleFromCurrentWaypoint = getAngleToAPosition(CurrentPosition.localEulerAngles, MazeWaypointsPath[WayPointsCurrentPosition] - CurrentPosition.localPosition);

        if (DistanceFromCurrentWaypoint < DISTANCE_ACHIEVED_WAYPOINT)
        {
            ++WayPointsCurrentPosition;
            DistanceFromCurrentWaypoint = (MazeWaypointsPath[WayPointsCurrentPosition] - CurrentPosition.localPosition).magnitude;
            AngleFromCurrentWaypoint = getAngleToAPosition(CurrentPosition.localEulerAngles, MazeWaypointsPath[WayPointsCurrentPosition] - CurrentPosition.localPosition);
            if ((AngleFromCurrentWaypoint < 0) || (AngleFromCurrentWaypoint > (CIRCUMFERENCE_DEGREES / 2)))
                State = VehicleAutomaticMovementState.RIGHT_ROTATING;
            else
                State = VehicleAutomaticMovementState.LEFT_ROTATING;
        }
        else if ((AngleFromCurrentWaypoint < ANGLE_ACHIEVED_WAYPOINT))
        {
            State = VehicleAutomaticMovementState.STRAIGHT;
        }
        else
        {
            if ((AngleFromCurrentWaypoint < 0) || (AngleFromCurrentWaypoint > (CIRCUMFERENCE_DEGREES / 2)))
                State = VehicleAutomaticMovementState.RIGHT_ROTATING;
            else
                State = VehicleAutomaticMovementState.LEFT_ROTATING;
        }
        return (uint)State;
    }
}