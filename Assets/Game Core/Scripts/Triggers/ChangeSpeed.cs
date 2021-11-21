using UnityEngine;

public class ChangeSpeed : MonoBehaviour {
	
	public float speed = 12;

	void OnTriggerEnter (Collider other){
		if(other.tag == "Player"){
			var mov = other.GetComponent<LineMovement>();
			if(mov != null) mov.speed = speed;
		}
	}
}
