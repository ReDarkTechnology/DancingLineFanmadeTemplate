using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
	public Transform target;
	public Transform destination;
    public void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
			if(target == null) target = other.transform;
			if(destination == null) destination = other.transform;
			target.position = destination.position;
    	}
    }
}
