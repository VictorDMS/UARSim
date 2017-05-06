//CharacterEntity 
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Score")]
public class ScoreDBEntity
{
    [DynamoDBHashKey]   // Hash key.
    public string UserID { get; set; }

    [DynamoDBProperty]
    public string DeviceID { get; set; }

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
}