using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class setFogLinear : MonoBehaviour
{

	public bool UseFog = true;
	public Color Fogcolor = new Color(0, 0, 0, 1);
	public float FogStart;
	public float FogEnd;
	public float NeedTime;

    void OnTriggerEnter(Collider other)

    {
        if (other.tag == "Player")
        {
            RenderSettings.fog = UseFog;
            DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x, FogStart, NeedTime);
            DOTween.To(() => RenderSettings.fogEndDistance, x => RenderSettings.fogEndDistance = x, FogEnd, NeedTime);
            DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, Fogcolor, NeedTime);
        }
    }
}
