using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Emi_Color : MonoBehaviour
{
    public Material need_set_mat;
    public Color start,newer;
    //alled before the first frame update
    void Start()
    {
        need_set_mat.EnableKeyword("_EMISSION");
        need_set_mat.SetColor("_EmissionColor", start);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            need_set_mat.EnableKeyword("_EMISSION");
            need_set_mat.SetColor("_EmissionColor", newer);
        }
    }
}
