using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Light_Shadow : MonoBehaviour
{
    public enum shadowtype { No,Soft,Hard};
    public Light set_light;
    public shadowtype types = shadowtype.Hard;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(types == shadowtype.Hard)
            {
                set_light.shadows = LightShadows.Hard;
            }
            if (types == shadowtype.Soft)
            {
                set_light.shadows = LightShadows.Soft;
            }
            if (types == shadowtype.No)
            {
                set_light.shadows = LightShadows.None;
            }
        }
    }
}
