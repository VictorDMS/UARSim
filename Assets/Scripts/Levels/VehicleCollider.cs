using System.Collections;
using UnityEngine;

public class VehicleCollider : MonoBehaviour{
    [SerializeField] private GameObject BrokenWallL3;
    [SerializeField] private GameObject BrokenWallL4;
    [SerializeField] private GameObject StopWatch;

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Coin")) {
            Destroy(other.gameObject);
            if (LevelsManager.foundGoal()) {
                EventsDBModel.logEvent(EventsTypesDB.GameEvent, SubEventsTypesDB.EndLevel, "");
                LevelsManager.endLevel();
            }
        }else if (other.tag.Equals("Wall") && 
            ((LevelsManager.getCurrentLevel() == LevelsManager.Levels.L3) || (LevelsManager.getCurrentLevel() == LevelsManager.Levels.L4))){
            StopWatch.GetComponent<StopwatchUpdater>().punishForBreakWall();
            StartCoroutine(BreakDeath(other.gameObject));
        }else if (other.tag.Equals("Wall") &&
            ((LevelsManager.getCurrentLevel() == LevelsManager.Levels.L1) || (LevelsManager.getCurrentLevel() == LevelsManager.Levels.L2))){
            EventsDBModel.logEvent(EventsTypesDB.GameEvent, SubEventsTypesDB.WallCollision, "");
        }else if (other.tag.Equals("FloorDeadEnd")) {
            EventsDBModel.logEvent(EventsTypesDB.GameEvent, SubEventsTypesDB.DeadEnd, "Vehicle Entering in Dead-End");
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.tag.Equals("Floor")) {
            EventsDBModel.logEvent(EventsTypesDB.GameEvent, SubEventsTypesDB.DeadEnd, "Vehicle Exiting from Dead-End");
        }
    }

    public IEnumerator BreakDeath(GameObject Wallsito){//Adding Quaternions is done via *
        if(LevelsManager.getCurrentLevel() == LevelsManager.Levels.L3)
            Instantiate(BrokenWallL3, Wallsito.transform.position, Wallsito.transform.rotation * BrokenWallL3.transform.rotation);
        if (LevelsManager.getCurrentLevel() == LevelsManager.Levels.L4)
            Instantiate(BrokenWallL4, Wallsito.transform.position, Wallsito.transform.rotation * BrokenWallL4.transform.rotation);

        Destroy(Wallsito);
        yield return null;
    }
}
