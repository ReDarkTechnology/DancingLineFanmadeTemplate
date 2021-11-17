using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAni : MonoBehaviour {
	public Animator[] Obj;
    void Start()
    {
        for (int i = 0; i < Obj.Length; i++)
        {
            Obj[i].enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
	{ 
		if (other.gameObject.tag == "Player") 
		{
			for (int i = 0; i < Obj.Length; i++)
			{
				Obj [i].enabled = true ;
			}
		}
	}	
}
	