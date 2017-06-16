using Amazon.DynamoDBv2.DataModel;

public enum EventsTypesDB{
    ScoreEvent, UserEvent, GameEvent
}

public enum SubEventsTypesDB{
    ChangeRobots, BeforeChangeConfig, AfterChangeConfig, OpenMap, CloseMap, StartTouchMoving,
    EndTouchMoving, StartTouchLooking, EndTouchLooking, MenuGame, ActualPos, WallCollision,
    WallDestroyed, DeadEnd, StartLevel, EndLevel, GetGoal, FingersPosition
}

[DynamoDBTable("Events")]
public class EventsDBEntity
{
    [DynamoDBHashKey]
    public string DeviceID { get; set; }

    [DynamoDBProperty]
    public string GameID { get; set; }

    [DynamoDBProperty]
    public EventsTypesDB EventTypeID { get; set; }

    [DynamoDBProperty]
    public SubEventsTypesDB SubEventTypeID { get; set; }

    [DynamoDBProperty]
    public string EventDescription { get; set; }

    [DynamoDBProperty]
    public long TimestampLong { get; set; }
    
    [DynamoDBProperty]
    public LevelsManager.Levels LevelID { get; set; }
    
    [DynamoDBProperty]
    public string RobotConfiguration { get; set; }
}