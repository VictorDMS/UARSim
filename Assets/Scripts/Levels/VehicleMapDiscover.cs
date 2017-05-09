 using UnityEngine;
using System.Collections;

public class VehicleMapDiscover : MonoBehaviour
{
    [SerializeField] private GameObject Vehicle;

    void OnTriggerEnter(Collider other){

        RaycastHit HitInfo;
        Vector3 Direction = other.transform.position - Vehicle.transform.position; 
        Physics.Raycast(Vehicle.transform.position, Direction, out HitInfo, 200, 9);

        float FullDistance = (Vehicle.transform.position - other.transform.position).magnitude;
        print("Hit Distance: " + HitInfo.distance + "\nFullDistance: " + FullDistance);
        if ((HitInfo.distance - 0.005) < FullDistance) { 
            other.gameObject.layer = LayerMask.NameToLayer("MiniMap");
        }
    }
}
