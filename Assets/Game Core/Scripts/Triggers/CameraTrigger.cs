using System;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
	[Header("Important")]
	public BetterCamera targetCamera;
	[Header("Variables")]
	public bool setStart;
	public CameraValues startValues = new CameraValues();
	public bool setTween = true;
	public CameraValues tweenValues = new CameraValues();
	[Header("Tweening")]
	public LeanTweenType tweenType = LeanTweenType.easeOutCubic;
	public float tweenTime = 1f;
	public float tweenDelay;
	
	void Start(){
		if(targetCamera == null) targetCamera = FindObjectOfType<BetterCamera>();
	}
	private void OnTriggerEnter(Collider other){
		if(targetCamera == null) targetCamera = FindObjectOfType<BetterCamera>();
		if(targetCamera != null)
		{
			if(setStart){
				startValues.ApplyValues(targetCamera);
			}
			if(setTween){
				tweenValues.TweenValues(targetCamera, tweenType, tweenTime, tweenDelay);
			}
		}else{
			Debug.Log("Camera is not found...");
		}
	}
}
[Serializable]
public class CameraValues
{
	public bool changeRotation = true;
	public Vector3 targetRotation = new Vector3(45f, 60f, 0);
	
	public bool changeDistance;
	public float TargetDistance = 20f;

	public bool changeTimes;
	public float SmoothTime = 1f;
	[Range(0.001f, 10f)]
	public float SmoothFactor = 1f;
	public float needtime = 1f;
	
	public bool changeInsideRotation;
	public Vector3 insideCameraRotation;
	
	public static CameraValues GetCameraValues(BetterCamera cam){
		var values = new CameraValues();
		values.TakeValues(cam);
		return values;
	}
	
	public void TakeValues(BetterCamera cam){
		targetRotation = new Vector3(cam.targetX, cam.targetY, cam.targetZ);
		TargetDistance = cam.TargetDistance;
		SmoothTime = cam.SmoothTime;
		SmoothFactor = cam.SmoothFactor;
		needtime = cam.needtime;
		insideCameraRotation = cam.mainCamera.transform.localEulerAngles;
	}
	public void ApplyValues(BetterCamera cam){
		if(changeRotation)
		{
			cam.targetX = targetRotation.x;
			cam.targetY = targetRotation.y;
			cam.targetZ = targetRotation.z;
		}
		if(changeDistance) cam.TargetDistance = TargetDistance;
		if(changeTimes)
		{
			cam.SmoothTime = SmoothTime;
			cam.SmoothFactor = SmoothFactor;
			cam.needtime = needtime;
		}
		if(changeInsideRotation)cam.mainCamera.transform.localEulerAngles = insideCameraRotation;
	}
	public void TweenValues(BetterCamera cam, LeanTweenType tweenType, float tweenTime, float tweenDelay){
		if(changeRotation)
		{
			var camRotation = new Vector3(cam.targetX, cam.targetY, cam.targetZ);
			LeanTween.value(cam.gameObject, camRotation, targetRotation, tweenTime).setEase(tweenType).setDelay(tweenDelay).setOnUpdate(
				(Vector3 result) =>
				{
					cam.targetX = result.x;
					cam.targetY = result.y;
					cam.targetZ = result.z;
				}
			);
		}
		if(changeDistance){
			LeanTween.value(cam.gameObject, cam.TargetDistance, TargetDistance, tweenTime).setEase(tweenType).setDelay(tweenDelay).setOnUpdate(
				(float result) =>
				{
					cam.TargetDistance = result;
				}
			);
		}
		if(changeTimes)
		{
			LeanTween.value(cam.gameObject, cam.SmoothTime, SmoothTime, tweenTime).setEase(tweenType).setDelay(tweenDelay).setOnUpdate(
				(float result) =>
				{
					cam.SmoothTime = result;
				}
			);
			LeanTween.value(cam.gameObject, cam.SmoothFactor, SmoothFactor, tweenTime).setEase(tweenType).setDelay(tweenDelay).setOnUpdate(
				(float result) =>
				{
					cam.SmoothFactor = result;
				}
			);
			LeanTween.value(cam.gameObject, cam.needtime, needtime, tweenTime).setEase(tweenType).setDelay(tweenDelay).setOnUpdate(
				(float result) =>
				{
					cam.needtime = result;
				}
			);
		}
		if(changeInsideRotation){
			LeanTween.value(cam.gameObject, cam.mainCamera.transform.localEulerAngles, insideCameraRotation, tweenTime).setEase(tweenType).setDelay(tweenDelay).setOnUpdate(
				(Vector3 result) =>
				{
					cam.mainCamera.transform.localEulerAngles = result;
				}
			);
		}
	}
	
}