using System;
using UnityEngine;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor1 = null, Floor2 = null, Floor3 = null, Floor4 = null, FloorForMap = null;
	public GameObject Wall1 = null, Wall2 = null, Wall3 = null, Wall4 = null, WallForMap = null, WallForLog = null;
	public GameObject Pillar1 = null, Pillar2 = null, Pillar3 = null, Pillar4 = null;
	public const int Rows = 10; //5;
	public const int Columns = 10; //5;
    public const float CellWidth = 4;//5;
    public const float CellHeight = 4;//5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null/*, GoalPrefabForMap = null*/;
	private BasicMazeGenerator mMazeGenerator = null;

    public void loadMaze(short NumberOfGoalPrefab){

        if(AutomaticMovement.MazeMatrix3DPoints != null && AutomaticMovement.MazeMatrix != null){
            Array.Clear(AutomaticMovement.MazeMatrix3DPoints, 0, AutomaticMovement.MazeMatrix3DPoints.Length);
            Array.Clear(AutomaticMovement.MazeMatrix, 0, AutomaticMovement.MazeMatrix.Length);
            removeOldMaze();
        }

        if (!FullRandom){
            UnityEngine.Random.seed = RandomSeed;
        }
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }
        mMazeGenerator.GenerateMaze();

        //NEW CODE VDMS
        AutomaticMovement.MazeMatrix = new int[(Rows * 2) + 1][];
        for (uint i = 0; i < (Rows * 2) + 1; ++i)
            AutomaticMovement.MazeMatrix[i] = new int[(Columns * 2) + 1];
        AutomaticMovement.MazeMatrix3DPoints = new Vector3[(Rows * 2) + 1][];
        for (uint i = 0; i < (Rows * 2) + 1; ++i)
            AutomaticMovement.MazeMatrix3DPoints[i] = new Vector3[(Columns * 2) + 1];
        //NEW CODE VDMS
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;
                tmp = Instantiate(getFloor(), new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;
                tmp = Instantiate(FloorForMap, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;

                //NEW CODE VDMS
                AutomaticMovement.MazeMatrix[(row * 2) + 1][(column * 2) + 1] = 1;
                AutomaticMovement.MazeMatrix3DPoints[(row * 2) + 1][(column * 2) + 1] = new Vector3(x, 0, z);
                //NEW CODE VDMS

                if (cell.WallRight)
                {
                    tmp = Instantiate(getWall(), new Vector3(x + CellWidth / 2, 0, z) + getWall().transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                    tmp.transform.parent = transform;
                    tmp = Instantiate(WallForMap, new Vector3(x + CellWidth / 2, 0, z) + WallForMap.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                    tmp.transform.parent = transform;
                    tmp = Instantiate(WallForLog, new Vector3(x + CellWidth / 2, 0, z) + WallForLog.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                    tmp.transform.parent = transform;
                    //NEW CODE VDMS
                    AutomaticMovement.MazeMatrix[(row * 2) + 1][(column * 2) + 1 + 1] = -1;
                    AutomaticMovement.MazeMatrix3DPoints[(row * 2) + 1][(column * 2) + 1 + 1] = new Vector3(-1, -1, -1);
                }
                else if (AutomaticMovement.MazeMatrix[(row * 2) + 1][(column * 2) + 1 + 1] == 0)
                {
                    AutomaticMovement.MazeMatrix[(row * 2) + 1][(column * 2) + 1 + 1] = 1;
                    AutomaticMovement.MazeMatrix3DPoints[(row * 2) + 1][(column * 2) + 1 + 1] = new Vector3(x + CellWidth / 2, 0, z) + getWall().transform.position;
                    //NEW CODE VDMS
                }

                if (cell.WallFront)
                {
                    tmp = Instantiate(getWall(), new Vector3(x, 0, z + CellHeight / 2) + getWall().transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                    tmp.transform.parent = transform;
                    tmp = Instantiate(WallForMap, new Vector3(x, 0, z + CellHeight / 2) + WallForMap.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                    tmp.transform.parent = transform;
                    tmp = Instantiate(WallForLog, new Vector3(x, 0, z + CellHeight / 2) + WallForLog.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                    tmp.transform.parent = transform;
                    /*NEW CODE VDMS*/
                    AutomaticMovement.MazeMatrix[(row * 2) + 1 + 1][(column * 2) + 1] = -1;
                    AutomaticMovement.MazeMatrix3DPoints[(row * 2) + 1 + 1][(column * 2) + 1] = new Vector3(-1, -1, -1);
                }
                else if (AutomaticMovement.MazeMatrix[(row * 2) + 1 + 1][(column * 2) + 1] == 0)
                {
                    AutomaticMovement.MazeMatrix[(row * 2) + 1 + 1][(column * 2) + 1] = 1;
                    AutomaticMovement.MazeMatrix3DPoints[(row * 2) + 1 + 1][(column * 2) + 1] = new Vector3(x, 0, z + CellHeight / 2) + getWall().transform.position;
                    /*NEW CODE VDMS*/
                }


                if (cell.WallLeft)
                {
                    tmp = Instantiate(getWall(), new Vector3(x - CellWidth / 2, 0, z) + getWall().transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                    tmp.transform.parent = transform;
                    tmp = Instantiate(WallForMap, new Vector3(x - CellWidth / 2, 0, z) + WallForMap.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                    tmp.transform.parent = transform;
                    tmp = Instantiate(WallForLog, new Vector3(x - CellWidth / 2, 0, z) + WallForLog.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                    tmp.transform.parent = transform;
                    /*NEW CODE VDMS*/
                    AutomaticMovement.MazeMatrix[(row * 2) + 1][(column * 2)] = -1;
                    AutomaticMovement.MazeMatrix3DPoints[(row * 2) + 1][(column * 2)] = new Vector3(-1, -1, -1);
                }
                else if (AutomaticMovement.MazeMatrix[(row * 2) + 1][(column * 2)] == 0)
                {
                    AutomaticMovement.MazeMatrix[(row * 2) + 1][(column * 2)] = 1;
                    AutomaticMovement.MazeMatrix3DPoints[(row * 2) + 1][(column * 2)] = new Vector3(x - CellWidth / 2, 0, z) + getWall().transform.position;
                    /*NEW CODE VDMS*/
                }

                if (cell.WallBack)
                {
                    tmp = Instantiate(getWall(), new Vector3(x, 0, z - CellHeight / 2) + getWall().transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                    tmp.transform.parent = transform;
                    tmp = Instantiate(WallForMap, new Vector3(x, 0, z - CellHeight / 2) + WallForMap.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                    tmp.transform.parent = transform;
                    tmp = Instantiate(WallForLog, new Vector3(x, 0, z - CellHeight / 2) + WallForLog.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                    tmp.transform.parent = transform;
                    AutomaticMovement.MazeMatrix[(row * 2)][(column * 2) + 1] = -1;
                    AutomaticMovement.MazeMatrix3DPoints[(row * 2)][(column * 2) + 1] = new Vector3(-1, -1, -1);
                    /*NEW CODE VDMS*/
                }
                else if (AutomaticMovement.MazeMatrix[(row * 2)][(column * 2) + 1] == 0)
                {
                    AutomaticMovement.MazeMatrix[(row * 2)][(column * 2) + 1] = 1;
                    AutomaticMovement.MazeMatrix3DPoints[(row * 2)][(column * 2) + 1] = new Vector3(x, 0, z - CellHeight / 2) + getWall().transform.position;
                    /*NEW CODE VDMS*/
                }
            }
        }

        for (uint a = 0; ((Rows * 2) + 1) > a; a++){
            for (uint b = 0; ((Rows * 2) + 1) > b; b++){
                if (AutomaticMovement.MazeMatrix[a][b] == -1){
                    AutomaticMovement.MazeMatrix[a][b] = 0;
                }
                if (AutomaticMovement.MazeMatrix3DPoints[a][b] == new Vector3(-1, -1, -1))
                    AutomaticMovement.MazeMatrix3DPoints[a][b] = new Vector3(0, 0, 0);
            }
        }

        if (getPillar() != null){
            for (int row = 0; row < Rows + 1; row++)
            {
                for (int column = 0; column < Columns + 1; column++)
                {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));
                    GameObject tmp = Instantiate(getPillar(), new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2) + getPillar().transform.position, getPillar().transform.localRotation) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }

        if (GoalPrefab != null){
            for (uint i = 0; i < NumberOfGoalPrefab; ++i){
                GameObject tmp;
                int RandomX = UnityEngine.Random.Range(0, Columns), RandomY = UnityEngine.Random.Range(0, Rows);
                while ((RandomX == 0) && (RandomY == 0))
                {
                    RandomX = UnityEngine.Random.Range(0, Columns);
                    RandomY = UnityEngine.Random.Range(0, Rows);
                }
                tmp = Instantiate(GoalPrefab, new Vector3(RandomX * (CellWidth + (AddGaps ? .2f : 0)), 1, RandomY * (CellHeight + (AddGaps ? .2f : 0))), Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;
                //tmp = Instantiate(GoalPrefabForMap, new Vector3(RandomX * (CellWidth + (AddGaps ? .2f : 0)), 1, RandomY * (CellHeight + (AddGaps ? .2f : 0))), Quaternion.Euler(0, 0, 0)) as GameObject;
                //tmp.transform.parent = transform;
                //GameObject tmp;
                //tmp = Instantiate(GoalPrefab, new Vector3(4,0,4), Quaternion.Euler(0, 0, 0)) as GameObject;
                //tmp.transform.parent = transform;
            }
        }
    }

    private void removeOldMaze(){
        foreach (Transform child in transform){
            DestroyObject(child.gameObject);
        }

        GameObject[] DestroyedWalls = GameObject.FindGameObjectsWithTag("DestroyedWall");
        foreach (GameObject w in DestroyedWalls){
            DestroyObject(w);
        }
    }

    private GameObject getFloor(){
        GameObject Floor = null;
        switch (LevelsManager.getCurrentLevel())
        {
            case LevelsManager.Levels.L1:
                Floor = Floor1;
                break;
            case LevelsManager.Levels.L2:
                Floor = Floor2;
                break;
            case LevelsManager.Levels.L3:
                Floor = Floor3;
                break;
            case LevelsManager.Levels.L4:
                Floor = Floor4;
                break;
            case LevelsManager.Levels.End:
            case LevelsManager.Levels.Start:
            default:
                break;
        }
        return Floor;
    }
    private GameObject getWall()
    {
        GameObject Wall = null;
        switch (LevelsManager.getCurrentLevel())
        {
            case LevelsManager.Levels.L1:
                Wall = Wall1;
                break;
            case LevelsManager.Levels.L2:
                Wall = Wall2;
                break;
            case LevelsManager.Levels.L3:
                Wall = Wall3;
                break;
            case LevelsManager.Levels.L4:
                Wall = Wall4;
                break;
            case LevelsManager.Levels.End:
            case LevelsManager.Levels.Start:
            default:
                break;
        }
        return Wall;
    }
    private GameObject getPillar()
    {
        GameObject Pillar = null;
        switch (LevelsManager.getCurrentLevel())
        {
            case LevelsManager.Levels.L1:
                Pillar = Pillar1;
                break;
            case LevelsManager.Levels.L2:
                Pillar = Pillar2;
                break;
            case LevelsManager.Levels.L3:
                Pillar = Pillar3;
                break;
            case LevelsManager.Levels.L4:
                Pillar = Pillar4;
                break;
            case LevelsManager.Levels.End:
            case LevelsManager.Levels.Start:
            default:
                break;
        }
        return Pillar;
    }
}
