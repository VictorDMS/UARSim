using UnityEngine;

public class DBModel : MonoBehaviour
{
    static public string getRobotConfigurationString()
    {
        string RobotConfigurationString = "";

        switch (ConfigBehavior.QuadCurrentConfig)
        {
            case ConfigBehavior.QuadRobotConfiguration.SPIRAL:
                RobotConfigurationString = "Drone:Spiral.";
                break;
            case ConfigBehavior.QuadRobotConfiguration.SCAN:
                RobotConfigurationString = "Drone:Scan.";
                break;
            case ConfigBehavior.QuadRobotConfiguration.RANDOM:
                RobotConfigurationString = "Drone:Random.";
                break;
        }
        switch (ConfigBehavior.LightCurrentConfig)
        {
            case ConfigBehavior.VehicleRobotConfiguration.LEFT:
                RobotConfigurationString += "Light:Left.";
                break;
            case ConfigBehavior.VehicleRobotConfiguration.RIGHT:
                RobotConfigurationString += "Light:Right.";
                break;
            case ConfigBehavior.VehicleRobotConfiguration.RANDOM:
                RobotConfigurationString += "Light:Random.";
                break;
        }
        switch (ConfigBehavior.UltraCurrentConfig)
        {
            case ConfigBehavior.VehicleRobotConfiguration.LEFT:
                RobotConfigurationString += "Ultra:Left.";
                break;
            case ConfigBehavior.VehicleRobotConfiguration.RIGHT:
                RobotConfigurationString += "Ultra:Right.";
                break;
            case ConfigBehavior.VehicleRobotConfiguration.RANDOM:
                RobotConfigurationString += "Ultra:Random.";
                break;
        }
        switch (ConfigBehavior.HeavyCurrentConfig)
        {
            case ConfigBehavior.VehicleRobotConfiguration.LEFT:
                RobotConfigurationString += "Heavy:Left.";
                break;
            case ConfigBehavior.VehicleRobotConfiguration.RIGHT:
                RobotConfigurationString += "Heavy:Right.";
                break;
            case ConfigBehavior.VehicleRobotConfiguration.RANDOM:
                RobotConfigurationString += "Heavy:Random.";
                break;
        }
        switch (ConfigBehavior.AutoConfiguration)
        {
            case ConfigBehavior.RobotTypes.DRONE:
                RobotConfigurationString += "Auto:Drone.";
                break;
            case ConfigBehavior.RobotTypes.LIGHT:
                RobotConfigurationString += "Auto:Light.";
                break;
            case ConfigBehavior.RobotTypes.ULTRA:
                RobotConfigurationString += "Auto:Ultra.";
                break;
            case ConfigBehavior.RobotTypes.HEAVY:
                RobotConfigurationString += "Auto:Heavy.";
                break;
        }

        return RobotConfigurationString;
    }
}