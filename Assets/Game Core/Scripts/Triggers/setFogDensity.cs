using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class setFogDensity : MonoBehaviour
{
    public bool UseFog = true;
    public Color NewColor = new Color(0,0,0,1);
    public float NewDensity, NeedTime;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            RenderSettings.fog = UseFog;
            DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, NewDensity, NeedTime);
            DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, NewColor, NeedTime);
        }
    }
}
