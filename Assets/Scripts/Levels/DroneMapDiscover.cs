using UnityEngine;
using System.Collections;

public class DroneMapDiscover : MonoBehaviour {
	void OnTriggerEnter(Collider other){
        if ((other.gameObject.layer == LayerMask.NameToLayer("HideLayer"))){
            other.gameObject.layer = LayerMask.NameToLayer("MiniMap");
        }
	}
}
