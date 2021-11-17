using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Set_Color_Light : MonoBehaviour
{
    public Light NeedLight;
    public Color NewColor;
    public float NeedTime;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            NeedLight.DOColor(NewColor, NeedTime);
        }
    }
}
