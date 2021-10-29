using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerIt : MonoBehaviour
{
	public Grouped grouped;
	public GameObject[] setActive;
	public void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
			if(grouped != null){
				grouped.Call(false);
			}
			if(setActive.Length > 0){
				foreach(GameObject h in setActive){
					h.SetActive(!h.activeSelf);
				}
			}
    	}
    }
}
