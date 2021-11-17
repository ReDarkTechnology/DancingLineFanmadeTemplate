using UnityEngine;
using DG.Tweening;

public class CollisionColor : MonoBehaviour
{
    public Color Change_To_Color = new Color(0, 0, 0, 1);
    public float NeedTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            GetComponent<MeshRenderer>().material.DOColor(Change_To_Color, NeedTime);
        }
    }
}
