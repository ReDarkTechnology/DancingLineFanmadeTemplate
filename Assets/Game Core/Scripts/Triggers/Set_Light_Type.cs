using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Light_Type : MonoBehaviour
{
    public enum shadowtype { Spot, Point, Directional };
    public Light set_light;
    public shadowtype types;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (types == shadowtype.Spot)
            {
                set_light.type = LightType.Spot;
            }
            if (types == shadowtype.Point)
            {
                set_light.type = LightType.Point;
            }
            if (types == shadowtype.Directional)
            {
                set_light.type = LightType.Directional;
            }
        }
    }
}
