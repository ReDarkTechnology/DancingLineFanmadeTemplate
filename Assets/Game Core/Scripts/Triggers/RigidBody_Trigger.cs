using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBody_Trigger : MonoBehaviour
{
    public Rigidbody OBJ;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if (OBJ.useGravity == false)
                OBJ.useGravity = true;
            else
                OBJ.useGravity = false;
        }
    }
    
}
