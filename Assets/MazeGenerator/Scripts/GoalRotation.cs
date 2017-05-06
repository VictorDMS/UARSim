using UnityEngine;

public class GoalRotation : MonoBehaviour {
    [SerializeField] private float RotationSpeed = 8.0f;
    void FixedUpdate () {
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0.0f, RotationSpeed, 0.0f);		
	}
}
