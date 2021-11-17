using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public GameObject[] GameObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            for(int i = 0; i < GameObject.Length; i++)
            { 
                if (GameObject[i].activeSelf == true) 
                {
                    GameObject[i].SetActive(false);
                }
                else
                {
                    GameObject[i].SetActive(true);
                }
            }
        }
    }
}
