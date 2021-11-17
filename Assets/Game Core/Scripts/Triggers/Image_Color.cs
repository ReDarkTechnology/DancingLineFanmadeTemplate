using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image_Color : MonoBehaviour
{
    public UnityEngine.UI.Image image;
    public Color newcolor;
    public float time;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
        	LeanTween.value (gameObject, image.color, newcolor, time).setOnUpdateColor
        		(value =>
                 {
            		image.color = value;
        		 }
        		);
        }
    }
}
