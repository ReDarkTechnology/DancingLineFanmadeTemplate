using System;
using UnityEngine;
// This class was owned by MaxIceFlame's template. The script is deprecated in this template.
[Obsolete("This camera script is deprecated, use BetterCamera instead!", false)]
public class FollowCamera : MonoBehaviour
{
	[Header("Camera Variables")]
	public Transform Line;
	public Vector3 pivotOffset = Vector3.zero;
	private float x;
	private float y;
	private float z;
	private Vector3 position = Vector3.zero;
	public float targetX = 45f;
	public float targetY = 60f;
	public float targetZ;
	private Vector3 Velocity = Vector3.zero;
	public float TargetDistance = 20f;
	public float SmoothTime = 1f;
	public float needtime = 1f;
	private float xVelocity = 1f;
	private float yVelocity = 1f;
	private float zVelocity = 1f;
	
	[Header("Camera Shake")]
	// How long the object should shake for.
	public float shakeDuration = 0f;
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	private const float decreaseFactor = 1.0f;
	
	private void Start()
	{
		this.position = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
		this.x = this.targetX;
		this.y = this.targetY;
		this.z = this.targetZ;
		Vector3 eulerAngles = base.transform.eulerAngles;
	}
	
	private void Update()
	{
		this.x = Mathf.SmoothDampAngle(this.x, this.targetX, ref this.xVelocity, this.needtime);
		this.y = Mathf.SmoothDampAngle(this.y, this.targetY, ref this.yVelocity, this.needtime);
		this.z = Mathf.SmoothDampAngle(this.z, this.targetZ, ref this.zVelocity, this.needtime);
		Quaternion rotation = Quaternion.Euler(this.x, this.y, this.z);
		base.transform.rotation = rotation;
		if (shakeDuration > 0) {
			transform.position = Vector3.SmoothDamp(base.transform.position, this.Line.position + rotation * new Vector3(0f, 0f, -this.TargetDistance) + this.pivotOffset, ref this.Velocity, this.SmoothTime) + UnityEngine.Random.insideUnitSphere * shakeAmount;
			shakeDuration -= decreaseFactor * Time.deltaTime;
		} else {
			transform.position = Vector3.SmoothDamp(base.transform.position, this.Line.position + rotation * new Vector3(0f, 0f, -this.TargetDistance) + this.pivotOffset, ref this.Velocity, this.SmoothTime);
			shakeDuration = 0f;
		}
	}
	
	public void ChangeVar(Vector3 targetRot, float targetSmooth, float targetRotSpeed, Vector3 targetPivotOffset, float targetDistance){
		targetX = targetRot.x;
		targetY = targetRot.y;
		targetZ = targetRot.z;
		SmoothTime = targetSmooth;
		needtime = targetRotSpeed;
		pivotOffset = targetPivotOffset;
		TargetDistance = targetDistance;
	}
	
	public void Shake(float Duration, float Strength){
		shakeAmount = Strength;
		shakeDuration = Duration;
	}
	public void SuddenZ(float to){
		z = to;	
	}
}
