using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("ScoreUSAR")]
public class ScoreDBEntity{
    [DynamoDBHashKey]
    public string DeviceID { get; set; }

    [DynamoDBProperty]
    public string GameID { get; set; }

    [DynamoDBProperty]
    public int Level { get; set; }

    [DynamoDBProperty]
    public float TimeElapsed { get; set; }

    [DynamoDBProperty]
    public string Timestamp { get; set; }

    [DynamoDBProperty]
    public string RobotConfiguration { get; set; }

    [DynamoDBProperty]
    public string DisplayName { get; set; }

    [DynamoDBProperty]
    public string Email { get; set; }
}