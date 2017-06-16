using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using UnityEngine;

public class DBModel : MonoBehaviour
{
    [SerializeField] public string cognitoIdentityPoolString;
    static public CognitoAWSCredentials credentials;
    static public IAmazonDynamoDB _client;
    static public DynamoDBContext _context;
    static public DynamoDBContext Context
    {
        get
        {
            if (_context == null)
                _context = new DynamoDBContext(_client);
            return _context;
        }
    }

    private void Start()
    {
        UnityInitializer.AttachToGameObject(gameObject);
        credentials = new CognitoAWSCredentials(cognitoIdentityPoolString, RegionEndpoint.EUCentral1);
        credentials.GetIdentityIdAsync(delegate (AmazonCognitoIdentityResult<string> result)
        {
            if (result.Exception != null)
            {
                Debug.LogError("exception hit: " + result.Exception.Message);
            }
            var ddbClient = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUCentral1);

            _client = ddbClient;
        });
    }

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