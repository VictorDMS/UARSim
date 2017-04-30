using UnityEngine;

public class VehicleCollider : MonoBehaviour{
    
    void Start(){
    }
    
    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag.Equals("Coin")){
            Destroy(other.gameObject);
            if (LevelsManager.foundGoal()){
                LevelsManager.loadLevel();
            }
        }
    }
}
