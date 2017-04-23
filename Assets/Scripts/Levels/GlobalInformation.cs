using UnityEngine;
using System.Collections;

public class GlobalInformation : MonoBehaviour {

    public enum VehicleRobotConfiguration { RANDOM, RIGHT, LEFT, UNKNOWN }
    public enum QuadRobotConfiguration { SPIRAL, SCAN, RANDOM, UNKNOWN }
    public enum Level { LEV1, LEV2, LEV3, LEV4}


    static public QuadRobotConfiguration QuadCurrentConfig = QuadRobotConfiguration.UNKNOWN;
    static public VehicleRobotConfiguration LightCurrentConfig = VehicleRobotConfiguration.UNKNOWN;
    static public VehicleRobotConfiguration HeavyCurrentConfig = VehicleRobotConfiguration.UNKNOWN;
    static public VehicleRobotConfiguration UltraCurrentConfig = VehicleRobotConfiguration.UNKNOWN;

    static public Level CurrentLevel;
    static public bool ExitConfigMenu;
}
