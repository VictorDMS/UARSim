using UnityEngine;

public abstract class AutomaticMovement : MonoBehaviour
{
    static public int[][] MazeMatrix;
    static public Vector3[][] MazeMatrix3DPoints;
    static public Vector3[] MazeWaypointsPath;
    static public Direction[] MazeWaypointsPathDirections;
    static public uint WayPointsCurrentPosition = 0;

    public enum Direction { NORTH, WEST, SOUTH, EAST };
    protected const ushort NumberOfDirections = 4;
    protected const int CIRCUMFERENCE_DEGREES = 360;

    public void findClosestFloor(Transform CurrentPosition, ref Direction PosibleDirection, ref uint RowIndex, ref uint ColumnIndex, bool IsDrone)
    {
        Vector3 ClosestPos = new Vector3();
        Vector3[] PosibleClosestPos = new Vector3[NumberOfDirections];
        uint ClosestPosMatIndexRow = 0, ClosestPosMatIndexRColumn = 0;

        float SmallestMagnitude = 100000.0f; //Really high number for initialization.
        for (uint row = 1; row < (MazeSpawner.Rows * 2) + 1; row += 2)
        {//FIND ORIGIN OF ALGORITHM.
            for (uint column = 1; column < (MazeSpawner.Columns * 2) + 1; column += 2)
            {
                if ((MazeMatrix3DPoints[row][column] != new Vector3(0, 0, 0)) || ((row == 1) && (column == 1)))
                {
                    if (((MazeMatrix3DPoints[row][column] - CurrentPosition.position).magnitude < SmallestMagnitude) &&
                        (row % 2 != 0) && (column % 2 != 0))
                    {
                        ClosestPos = MazeMatrix3DPoints[row][column];
                        ClosestPosMatIndexRColumn = column;
                        ClosestPosMatIndexRow = row;
                        SmallestMagnitude = (MazeMatrix3DPoints[row][column] - CurrentPosition.position).magnitude;
                    }
                }
            }
        }

        if (!IsDrone)
        {
            PosibleClosestPos[(int)Direction.NORTH] = MazeMatrix3DPoints[ClosestPosMatIndexRow + 1][ClosestPosMatIndexRColumn];
            PosibleClosestPos[(int)Direction.WEST] = MazeMatrix3DPoints[ClosestPosMatIndexRow][ClosestPosMatIndexRColumn - 1];
            PosibleClosestPos[(int)Direction.SOUTH] = MazeMatrix3DPoints[ClosestPosMatIndexRow - 1][ClosestPosMatIndexRColumn];
            PosibleClosestPos[(int)Direction.EAST] = MazeMatrix3DPoints[ClosestPosMatIndexRow][ClosestPosMatIndexRColumn + 1];

            float SmallestAngle = 100000.0f; //Really high number for initialization.
            for (ushort i = 0; i < NumberOfDirections; ++i)
            {
                if (PosibleClosestPos[(int)i] != new Vector3(0.0f, 0.0f, 0.0f))
                {
                    float ViewAngle = getAngleToAPosition(CurrentPosition.localEulerAngles, PosibleClosestPos[(int)i] - CurrentPosition.localPosition);
                    if (SmallestAngle > Mathf.Abs(ViewAngle))
                    {
                        PosibleDirection = (Direction)i;
                        if (i == (int)Direction.NORTH)
                        {
                            RowIndex = ClosestPosMatIndexRow + 2;
                            ColumnIndex = ClosestPosMatIndexRColumn;
                        }
                        else if (i == (int)Direction.WEST)
                        {
                            RowIndex = ClosestPosMatIndexRow;
                            ColumnIndex = ClosestPosMatIndexRColumn - 2;
                        }
                        else if (i == (int)Direction.SOUTH)
                        {
                            RowIndex = ClosestPosMatIndexRow - 2;
                            ColumnIndex = ClosestPosMatIndexRColumn;
                        }
                        else if (i == (int)Direction.EAST)
                        {
                            RowIndex = ClosestPosMatIndexRow;
                            ColumnIndex = ClosestPosMatIndexRColumn + 2;
                        }
                        SmallestAngle = ViewAngle;
                    }

                }
            }
        }
        else
        {
            RowIndex = ClosestPosMatIndexRow;
            ColumnIndex = ClosestPosMatIndexRColumn;
            PosibleDirection = Direction.WEST;
        }
    }
    public float getAngleToAPosition(Vector3 CurrentView, Vector3 GoToPosition)
    {
        float AngleToGoPosition = Mathf.Atan2(GoToPosition.x, GoToPosition.z) * Mathf.Rad2Deg;
        if (AngleToGoPosition < 0)
        {
            AngleToGoPosition += CIRCUMFERENCE_DEGREES;
        }

        if ((CurrentView.y - AngleToGoPosition) < 0)
        {
            return (CurrentView.y - AngleToGoPosition) + CIRCUMFERENCE_DEGREES;
        }
        else
        {
            return CurrentView.y - AngleToGoPosition;
        }
    }
    public abstract void buildWaypointsPathForCurrentConfiguration(Transform CurrentPosition);
    public abstract void basicPathCreator(Transform CurrentPosition, uint Type);
    public abstract uint getAutomaticMovementState(Transform CurrentPosition);
}