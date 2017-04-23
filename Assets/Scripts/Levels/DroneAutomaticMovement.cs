using UnityEngine;
using System.Linq;

public class DroneAutomaticMovement : AutomaticMovement
{
    public const float DRONE_AUTOMATIC_SPEED = 0.01f;
    private const float DISTANCE_ACHIEVED_WAYPOINT = 1.3f;
    private const float ANGLE_ACHIEVED_WAYPOINT = 4.0f;
    
    public override void buildWaypointsPathForCurrentConfiguration(Transform CurrentPosition)
    {
        if (GlobalInformation.QuadCurrentConfig == GlobalInformation.QuadRobotConfiguration.SPIRAL)
            basicPathCreator(CurrentPosition, (uint)GlobalInformation.QuadRobotConfiguration.SPIRAL);
        else if (GlobalInformation.QuadCurrentConfig == GlobalInformation.QuadRobotConfiguration.SCAN)
            basicPathCreator(CurrentPosition, (uint)GlobalInformation.QuadRobotConfiguration.SCAN);
        else if (GlobalInformation.QuadCurrentConfig == GlobalInformation.QuadRobotConfiguration.RANDOM)
            basicPathCreator(CurrentPosition, (uint)GlobalInformation.QuadRobotConfiguration.RANDOM);
    }
    
    public override void basicPathCreator(Transform CurrentPosition, uint Config)
    {
        const uint WAYPOINTS_COUNT = 500;
        Direction CurrentDirection = new Direction();
        uint RowIndex = 0, ColumnIndex = 0, Steps = 0;
        MazeWaypointsPath = new Vector3[500];
        MazeWaypointsPathDirections = new Direction[500];
        WayPointsCurrentPosition = 0;

        findClosestFloor(CurrentPosition, ref CurrentDirection, ref RowIndex, ref ColumnIndex, true);

        MazeWaypointsPath[Steps] = MazeMatrix3DPoints[RowIndex][ColumnIndex];
        MazeWaypointsPathDirections[Steps] = CurrentDirection;
        ++Steps;

        Direction HorizontalLastDirection = CurrentDirection;
        uint CounterNorthMov = 0;

        const uint MAX_COUNT_SAME_DIRECTION = 4;
        const uint MARGIN_POSITION = 2;
        uint CounterSameDirection = 0;
        Direction[] RandomDirections = new Direction[] { Direction.NORTH, Direction.SOUTH, Direction.WEST, Direction.EAST };

        uint LimitColumn = 0, LimitRow = 0;
        bool FirstTimeSpiral = true;

        while (Steps < WAYPOINTS_COUNT)
        {
            if ((GlobalInformation.QuadRobotConfiguration)Config == GlobalInformation.QuadRobotConfiguration.SCAN)
            {
                //Change Direction of movement
                if ((CurrentDirection == Direction.WEST) && (ColumnIndex >= ((MazeSpawner.Columns * 2) + 1 - MARGIN_POSITION)) &&
                                                            (RowIndex < ((MazeSpawner.Rows * 2) + 1 - MARGIN_POSITION)))
                {
                    RowIndex += 2;
                    CurrentDirection = Direction.NORTH;
                    CounterNorthMov = 1;
                }
                else if ((CurrentDirection == Direction.EAST) && (ColumnIndex < MARGIN_POSITION) &&
                                                                 (RowIndex < ((MazeSpawner.Rows * 2) + 1 - MARGIN_POSITION)))
                {
                    RowIndex += 2;
                    CurrentDirection = Direction.NORTH;
                    CounterNorthMov = 1;
                }
                else if ((CurrentDirection == Direction.NORTH) && ((RowIndex > ((MazeSpawner.Rows * 2) + 1 - MARGIN_POSITION))
                                                                || ((CounterNorthMov > 2) && (HorizontalLastDirection == Direction.WEST))))
                {
                    ColumnIndex -= 2;
                    HorizontalLastDirection = CurrentDirection = Direction.EAST;
                }
                else if ((CurrentDirection == Direction.NORTH) && ((RowIndex >= ((MazeSpawner.Rows * 2) + 1 - MARGIN_POSITION))
                                                                || ((CounterNorthMov > 2) && (HorizontalLastDirection == Direction.EAST))))
                {
                    ColumnIndex += 2;
                    HorizontalLastDirection = CurrentDirection = Direction.WEST;
                }
                else
                {//Maintain trajectory
                    if ((CurrentDirection == Direction.WEST) && (ColumnIndex < MazeSpawner.Columns * 2 - 1))
                        ColumnIndex += 2;
                    else if ((CurrentDirection == Direction.EAST) && (ColumnIndex > 1))
                        ColumnIndex -= 2;
                    else if ((CurrentDirection == Direction.NORTH) && (RowIndex < MazeSpawner.Rows * 2 - 1))
                    {
                        RowIndex += 2;
                        ++CounterNorthMov;
                    }
                    else if (((ColumnIndex >= MazeSpawner.Columns * 2 - 1) && (RowIndex >= MazeSpawner.Rows * 2 - 1)) ||
                            ((ColumnIndex <= 1) && (RowIndex <= 1)))
                    {
                        ColumnIndex = 0;
                        RowIndex = 0;
                    }
                }
            }
            else if ((GlobalInformation.QuadRobotConfiguration)Config == GlobalInformation.QuadRobotConfiguration.RANDOM)
            {

                if (CounterSameDirection == 0)
                {
                    System.Random r = new System.Random();
                    RandomDirections = RandomDirections.OrderBy(x => r.Next()).ToArray();
                    CounterSameDirection = MAX_COUNT_SAME_DIRECTION;
                }
                for (ushort i = 0; i < NumberOfDirections; ++i)
                {
                    if ((RandomDirections[i] == Direction.NORTH) && (RowIndex < (MazeSpawner.Rows * 2) + 1 - MARGIN_POSITION))
                    {
                        RowIndex += 2;
                        CurrentDirection = Direction.NORTH;
                        --CounterSameDirection;
                        break;
                    }
                    else if ((RandomDirections[i] == Direction.SOUTH) && (RowIndex > MARGIN_POSITION))
                    {
                        RowIndex -= 2;
                        CurrentDirection = Direction.SOUTH;
                        --CounterSameDirection;
                        break;
                    }
                    else if ((RandomDirections[i] == Direction.EAST) && (ColumnIndex > MARGIN_POSITION))
                    {
                        ColumnIndex += 2;
                        CurrentDirection = Direction.EAST;
                        --CounterSameDirection;
                        break;
                    }
                    else if ((RandomDirections[i] == Direction.WEST) && (ColumnIndex < (MazeSpawner.Columns * 2) + 1 - MARGIN_POSITION))
                    {
                        ColumnIndex -= 2;
                        CurrentDirection = Direction.WEST;
                        --CounterSameDirection;
                        break;
                    }
                }
            }
            else if ((GlobalInformation.QuadRobotConfiguration)Config == GlobalInformation.QuadRobotConfiguration.SPIRAL)
            {

                if (FirstTimeSpiral)
                {
                    if ((RowIndex > (MazeSpawner.Rows * 2 + 1) / 2) && (ColumnIndex > (MazeSpawner.Columns * 2 + 1) / 2))
                    {
                        LimitColumn = (MazeSpawner.Columns * 2 + 1) - RowIndex + 2;
                        CurrentDirection = Direction.EAST; //dcha arriba
                    }
                    else if ((RowIndex > (MazeSpawner.Rows * 2 + 1) / 2) && (ColumnIndex < (MazeSpawner.Columns * 2 + 1) / 2))
                    {
                        LimitRow = (MazeSpawner.Rows * 2 + 1) - RowIndex + 2;
                        CurrentDirection = Direction.SOUTH;//izq arriba
                    }
                    else if ((RowIndex < (MazeSpawner.Rows * 2 + 1) / 2) && (ColumnIndex > (MazeSpawner.Columns * 2 + 1) / 2))
                    {
                        LimitRow = (MazeSpawner.Rows * 2 + 1) - RowIndex - 2;
                        CurrentDirection = Direction.NORTH;//dcha abajo
                    }
                    else if ((RowIndex < (MazeSpawner.Rows * 2 + 1) / 2) && (ColumnIndex < (MazeSpawner.Columns * 2 + 1) / 2))
                    {
                        LimitColumn = (MazeSpawner.Columns * 2 + 1) - RowIndex - 2;
                        CurrentDirection = Direction.WEST;//izq abajo
                    }
                    FirstTimeSpiral = false;
                }
                //Change Direction of movement
                else if ((CurrentDirection == Direction.WEST) && (ColumnIndex >= LimitColumn))
                {
                    LimitRow = LimitColumn - 2;
                    CurrentDirection = Direction.NORTH;
                    RowIndex += 2;
                }
                else if ((CurrentDirection == Direction.EAST) && (ColumnIndex <= LimitColumn))
                {
                    LimitRow = LimitColumn + 1;
                    CurrentDirection = Direction.SOUTH;
                    RowIndex -= 2;
                }
                else if ((CurrentDirection == Direction.NORTH) && (RowIndex >= LimitRow))
                {
                    LimitColumn = (MazeSpawner.Columns * 2 + 1) - LimitRow + 1;
                    CurrentDirection = Direction.EAST;
                    ColumnIndex -= 2;
                }
                else if ((CurrentDirection == Direction.SOUTH) && (RowIndex <= LimitRow))
                {
                    LimitColumn = (MazeSpawner.Columns * 2 + 1) - RowIndex - 2;
                    CurrentDirection = Direction.WEST;
                    ColumnIndex += 2;
                }
                else
                {//Maintain trajectory
                    if (CurrentDirection == Direction.WEST)
                        ColumnIndex += 2;
                    else if (CurrentDirection == Direction.EAST)
                        ColumnIndex -= 2;
                    else if (CurrentDirection == Direction.NORTH)
                        RowIndex += 2;
                    else if (CurrentDirection == Direction.SOUTH)
                        RowIndex -= 2;
                }
            }
            if(RowIndex > 20 || ColumnIndex > 20 || RowIndex < 0 || ColumnIndex < 0)
            {
                Debug.Log("GAFASAO!!!");
            }
            else if (MazeMatrix3DPoints[RowIndex][ColumnIndex] != new Vector3(0, 0, 0))
            {
                MazeWaypointsPath[Steps] = MazeMatrix3DPoints[RowIndex][ColumnIndex];
                MazeWaypointsPathDirections[Steps] = CurrentDirection;
            }
            ++Steps;
        }
    }

    public override uint getAutomaticMovementState(Transform CurrentPosition)
    {
        Direction WhereToGo;
        Vector3 RelativeVector = (MazeWaypointsPath[WayPointsCurrentPosition] - CurrentPosition.localPosition);
        float DistanceFromCurrentWaypoint = (new Vector2(RelativeVector.x, RelativeVector.z)).magnitude;

        if (DistanceFromCurrentWaypoint < DISTANCE_ACHIEVED_WAYPOINT){
            ++WayPointsCurrentPosition;
        }

        RelativeVector = MazeWaypointsPath[WayPointsCurrentPosition] - CurrentPosition.localPosition;
        if ((RelativeVector.z > 0) && (Mathf.Abs(RelativeVector.z) > Mathf.Abs(RelativeVector.x)))
            WhereToGo = Direction.NORTH;
        else if ((RelativeVector.x > 0) && (Mathf.Abs(RelativeVector.x) > Mathf.Abs(RelativeVector.z)))
            WhereToGo = Direction.WEST;
        else if ((RelativeVector.z < 0) && (Mathf.Abs(RelativeVector.z) > Mathf.Abs(RelativeVector.x)))
            WhereToGo = Direction.SOUTH;
        else if ((RelativeVector.x < 0) && (Mathf.Abs(RelativeVector.x) > Mathf.Abs(RelativeVector.z)))
            WhereToGo = Direction.EAST;
        else
            WhereToGo = Direction.NORTH; //TEST. TODO. Delete this shiat....

        return (uint)WhereToGo;
    }
}