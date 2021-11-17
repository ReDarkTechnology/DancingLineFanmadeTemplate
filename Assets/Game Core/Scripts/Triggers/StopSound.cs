using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSound : MonoBehaviour
{
    public LineMovement line;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            line.GetComponent<AudioSource>().enabled = false;
        }
    }
}
