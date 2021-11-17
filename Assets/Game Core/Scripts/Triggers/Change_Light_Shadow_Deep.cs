using UnityEngine;
using DG.Tweening;

public class Change_Light_Shadow_Deep : MonoBehaviour
{
    public Light set;
    [Range(0, 1)] public float new_shadow_deep;
    public float times;
    
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            DOTween.To(() => set.shadowStrength, y => set.shadowStrength = y, new_shadow_deep, times);
        }
    }
}
