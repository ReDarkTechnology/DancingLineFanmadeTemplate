using UnityEngine;
using DG.Tweening;

public class Set_Color : MonoBehaviour
{

	public Material Need_Change_Material;
	public Color Start_Color,End_Color;

	public float Needtime = 0;

    void Start()
    {
        Need_Change_Material.color = Start_Color;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Need_Change_Material.DOColor(End_Color, Needtime);
        }
    }
}
