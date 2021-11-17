using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBlock : MonoBehaviour {

	public LineMovement LineMovement;
	private bool ok;
	
	void OnTriggerEnter (Collider other) {
		if (ok == false && other.GetComponent<LineMovement> () != null) {
			LineMovement.TurnBlock ();
			Debug.Log ("OK");
			ok = true;
		}
	}
}
