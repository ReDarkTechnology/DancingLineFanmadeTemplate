using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapers : MonoBehaviour
{
    public GameObject PlayEffect;
    public bool AutoMode;
    void Start()
    {
        if (AutoMode)
        {
            this.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.005f, 0.005f, 1.5f);
        }
        else
        {
            this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1.5f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(PlayEffect, this.transform.position,this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }
}
