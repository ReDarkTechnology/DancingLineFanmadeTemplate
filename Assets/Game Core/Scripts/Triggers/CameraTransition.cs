using System;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
	public GameObject targetCamera;
	public bool enableLocalControlWhenFinished;
	private bool isBetweenCalled;
	[Header("Startup")]
	public LeanTweenType easeTypeIn = LeanTweenType.easeInQuint;
	public Transform targetIn;
	public float inSpeed = 0.5f;
	[Header("Inbetween")]
	public bool setManually;
	public Transform targetBetween;
	public Action OnTransitionChanging;
	public UnityEngine.Events.UnityAction OnTransitionChange;
	[Header("Ending")]
	public LeanTweenType easeTypeOut = LeanTweenType.easeOutQuint;
	public Transform targetOut;
	public float outSpeed = 0.5f;
	void Start(){
		/*if(targetCamera == null){
			targetCamera = FindObjectOfType<BetterCamera>();
		}*/
	}
	void Update(){
		if(isBetweenCalled){
			GameObject trans = targetCamera;
			trans.LeanMoveLocal(targetOut.localPosition, outSpeed).setEase(easeTypeOut);
			trans.LeanRotate(targetOut.eulerAngles, outSpeed).setEase(easeTypeOut);
			Invoke("SetTest", outSpeed);
			isBetweenCalled = false;
		}
	}
	public void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
			//targetCamera.disableLocalCameraControl = true;
			GameObject trans = targetCamera;
			trans.LeanMoveLocal(targetIn.localPosition, inSpeed).setEase(easeTypeIn);
			trans.LeanRotate(targetIn.eulerAngles, inSpeed).setEase(easeTypeIn);
			Invoke("NextTrigger", inSpeed);
		}
	}
	public void SetTest(){
		if(enableLocalControlWhenFinished){
			//targetCamera.disableLocalCameraControl = true;
		}
	}
	public void NextTrigger(){
		if(OnTransitionChanging != null){
			OnTransitionChanging.Invoke();
		}
		if(OnTransitionChange != null){
			OnTransitionChange.Invoke();
		}
		if(!setManually){
			Transform trans = targetCamera.transform;
			trans.localPosition = targetBetween.localPosition;
			trans.localEulerAngles = targetBetween.localEulerAngles;
		}
		isBetweenCalled = true;
	}
	public static implicit operator TweenHost(CameraTransition v){
		return new TweenHost(v.targetCamera);
	}
}
