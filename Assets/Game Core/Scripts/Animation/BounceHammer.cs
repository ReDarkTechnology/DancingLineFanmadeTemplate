using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
public class BounceHammer : MonoBehaviour
{
	public bool process;
	[Header("Hammer")]
	public GameObject theHammer;
	public bool switchMode;
	public float speed = 0.5f;
	[System.Serializable]
	public class Limits
	{
		public float limitMinimum = -90;
		public float limitMaximum;
	}
	public Limits limit = new Limits();
	public LeanTweenType type1 = LeanTweenType.easeOutQuad;
	public LeanTweenType type2 = LeanTweenType.easeInQuad;
	[Header("String")]
	public GameObject theString;
	public bool isShaking;
	public float shakeAmount = 0.1f;
	Vector3 originalPos;
	
	void Start()
	{
		theHammer = transform.Find("Piano_Hammer").gameObject;
		theString = transform.Find("String").gameObject;
	}
	
	void Update(){
		if(process){
			Invoke("ShakeString", 0.5f);
			theHammer.LeanRotateX(-90, speed).setEase(type2);
			theHammer.LeanRotateX(0f,speed).setEase(type1).setDelay(0.5f);
			process = false;
		}
		if(isShaking){
			theString.transform.localPosition = originalPos + (Random.onUnitSphere * shakeAmount);
		}
	}
	
	public void ShakeString()
	{
		originalPos = theString.transform.localPosition;
		isShaking = true;
	}
}
