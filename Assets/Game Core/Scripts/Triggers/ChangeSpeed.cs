using UnityEngine;

public class ChangeSpeed : MonoBehaviour {
	
	private GameManager manager;
	public float speed = 12;
	
	void Start () {
		manager = FindObjectOfType<GameManager>();
	}

	void OnTriggerEnter (Collider other){
		if(other.tag == "Player")
			manager.lineSpeed = speed;
	}
}
