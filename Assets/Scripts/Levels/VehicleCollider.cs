using System.Collections;
using UnityEngine;

public class VehicleCollider : MonoBehaviour{
    [SerializeField] private GameObject BrokenWall;
    [SerializeField] private GameObject StopWatch;

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Coin")) {
            Destroy(other.gameObject);
            if (LevelsManager.foundGoal()) {
                LevelsManager.endLevel();
            }
        }else if (other.tag.Equals("Wall") && (LevelsManager.getCurrentLevel() == LevelsManager.Levels.L3)){
            StopWatch.GetComponent<StopwatchUpdater>().punishForBreakWall();
            StartCoroutine(BreakDeath(other.gameObject));
        }
    }

    public IEnumerator BreakDeath(GameObject Wallsito){//Adding Quaternions is done via *
        Instantiate(BrokenWall, Wallsito.transform.position, Wallsito.transform.rotation * BrokenWall.transform.rotation);
        Destroy(Wallsito);
        yield return null;
    }
}
