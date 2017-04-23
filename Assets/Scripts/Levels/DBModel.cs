using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("VDMSTable")]
public class DBModel
{
    [DynamoDBHashKey]
    public string Description { get; set; }

    [DynamoDBProperty]
    public string EventType { get; set; }

    [DynamoDBProperty]
    public int TimeStamp { get; set; }
}