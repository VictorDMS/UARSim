using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EventsDBModel : MonoBehaviour{

    static private List<EventsDBEntity> eventsDBEntities = new List<EventsDBEntity>();
    
    public static void CreateEventsInTable(){
        DBModel.Context.SaveAsync(eventsDBEntities, (result) => {
            if (result.Exception == null){
                Debug.Log("Events information stored into DB");
                eventsDBEntities.Clear();
            }else{
                Debug.Log("Error trying to store events in DB");
            }
        });
    }

    public static void logEvent(EventsTypesDB ETDB, SubEventsTypesDB SubETDB, string Description){
        string CurrentTimeStampString = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        long CurrentTimeStampLong = System.DateTime.UtcNow.ToBinary();

        EventsDBEntity Event = new EventsDBEntity {
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