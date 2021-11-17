using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrigger : MonoBehaviour
{

	public Vector3 Gravity = new Vector3(0f, -5f, 0f);

	public void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            Physics.gravity = Gravity;
        }
	}
}
