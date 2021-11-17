using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taper_Turn : MonoBehaviour
{
	[HideInInspector]  public LineMovement Line;
    private bool ok;
    
    void Start()
    {
        Line = Object.FindObjectOfType<LineMovement>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (this.enabled && !ok && other.gameObject == Line.gameObject)
        { 
        	Line.TurnBlock();
            ok = true;
            Instantiate(this.gameObject.GetComponent<Tapers>().PlayEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        
    }
}
