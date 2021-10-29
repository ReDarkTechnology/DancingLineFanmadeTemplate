using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
    public bool process;
	public bool switchMode;
	public float speed = 0.2f;
	public LeanTweenType type1 = LeanTweenType.easeInQuad;
	public LeanTweenType type2 = LeanTweenType.easeOutQuad;
    void Update()
    {
        if(process){
			gameObject.LeanRotateZ(363, speed).setEase(type2);
			gameObject.LeanRotateZ(360,speed).setEase(type1).setDelay(speed);
			process = false;
		}
    }
}
