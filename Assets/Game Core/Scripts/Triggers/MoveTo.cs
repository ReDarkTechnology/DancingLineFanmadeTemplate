using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
	public GameObject TheObject;
	public Vector3 target;
	public float time = 1;
	public LeanTweenType type;
    public void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
			TheObject.LeanMoveLocal(target, time).setEase(type);
    	}
    }
	public static implicit operator TweenHost(MoveTo v){
		return new TweenHost(v.TheObject);
	}
}
