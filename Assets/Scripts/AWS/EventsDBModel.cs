using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EventsDBModel : MonoBehaviour{

    static private List<EventsDBEntity> eventsDBEntities = new List<EventsDBEntity>();
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
        credentials.GetIdentityId();
        _client = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUCentral1);

        Table.LoadTableAsync(_client, "EventsUSAR", loadTableResult => {
            if (loadTableResult.Exception != null)
            {
                Debug.Log("\n failed to load events from AWS");
            }
            else
            {
                Debug.Log("\n loaded events from AWS");
            }
        });
    }

    public static void CreateEventsInTable(){
        var EventsBatch = Context.CreateBatchWrite<EventsDBEntity>();
        EventsBatch.AddPutItems(eventsDBEntities);

        EventsBatch.ExecuteAsync((result) => {
            if(result.Exception == null){
                Debug.Log("Event information stored into DB");
                eventsDBEntities.Clear();
            }else{
                Debug.Log("Error trying to store events in DB");
                Debug.Log(result.ToString());
                Debug.Log(result.Exception.Message);
            }
        });
    }

    public static void logEvent(EventsTypesDB ETDB, SubEventsTypesDB SubETDB, string Description){
        long CurrentTimeStampLong = System.DateTime.UtcNow.ToBinary();
        string CurrentTimeStampString = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

        EventsDBEntity Event = new EventsDBEntity {
            UUID = SystemInfo.deviceUniqueIdentifier + "_" + CurrentTimeStampString + "_" + Random.Range(0, 1000000).ToString(),
            EventTypeID = ETDB,
            SubEventTypeID = SubETDB,
            EventDescription = Description,
            TimestampLong = CurrentTimeStampLong,
            GameID = GameManager.GameID,
            LevelID = LevelsManager.getCurrentLevel(),
            DeviceID = SystemInfo.deviceUniqueIdentifier,
            RobotConfiguration = DBModel.getRobotConfigurationString()
        };
        eventsDBEntities.Add(Event);
    }
}