using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
	public UnityEngine.Events.UnityEvent action;
    private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			action.Invoke();
		}
	}
}
