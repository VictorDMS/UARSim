using UnityEngine;
using System.Collections;

public class MapDiscover : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other){
		other.gameObject.layer = LayerMask.NameToLayer("MiniMap");
	}
}
