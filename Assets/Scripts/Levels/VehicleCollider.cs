using UnityEngine;

public class VehicleCollider : MonoBehaviour{
    [SerializeField] private Transform BrokenWall;
    [SerializeField] private Transform Effect;

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Coin")) {
            Destroy(other.gameObject);
            if (LevelsManager.foundGoal()) {
                LevelsManager.loadLevel();
            }
        }else if (other.tag.Equals("Wall")){
            BreakDeath(other.gameObject);
        }
    }

    void BreakDeath(GameObject Wallsito){
        Instantiate(BrokenWall, Wallsito.transform.position, BrokenWall.transform.rotation);
        Destroy(Wallsito);
    }
}
