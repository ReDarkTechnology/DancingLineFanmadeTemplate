using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Set_Lens_Bright : MonoBehaviour
{
    public LensFlare sets;
    public float newbright, times;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DOTween.To(() => sets.brightness, x => sets.brightness = x, newbright, times);
        }
    }
}
