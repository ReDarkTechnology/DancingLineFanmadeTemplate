using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BetterCamera : MonoBehaviour
{
	[Header("Camera Variables")]
	public Transform Line;
	public Camera mainCamera;

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
	[Range(0.001f, 10f)]
	public float SmoothFactor = 1f;
	public float needtime = 1f;

	private float xVelocity = 1f;
	private float yVelocity = 1f;
	private float zVelocity = 1f;

	[Header("Built-in Camera Shake")]
	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	private float decreaseFactor = 1.0f;

	[Header("Misc")]
	// Should the camera be simulated in editor mode?
	public bool simulateInEditor = true;

	[System.Serializable]
	public class VariablesSaving{
		public Transform Line;
		public Vector3 pivotOffset = Vector3.zero;
		public float targetX = 45f;
		public float targetY = 60f;
		public float targetZ;
		public float TargetDistance = 20f;
		public float SmoothTime = 1f;
		public float SmoothFactor = 1f;
		public float needtime = 1f;
		public float shakeDuration = 0f;
		public float shakeAmount = 0.7f;
		public Vector3 position;
		public Quaternion rotation;
		public bool simulateInEditor;
	}
	private VariablesSaving currentVariables;
    // Start is called before the first frame update
    void Start()
    {
        this.x = this.targetX;
		this.y = this.targetY;
		this.z = this.targetZ;
		transform.position = Line.position + pivotOffset;
		mainCamera.transform.localPosition = new Vector3(0, 0, -TargetDistance);
		base.transform.eulerAngles = new Vector3(this.x, this.y, this.z);
		GameManager manager = FindObjectOfType<GameManager>();
		manager.OnCheckpointReset += ResetCamera;
		manager.OnCheckpointObtained += SetCurrentVar;
    }

    // Update is called once per frame
    void Update()
    {
		if(Application.isPlaying){
			float speedTime = Time.deltaTime * SmoothTime;
			Vector3 targetDist = new Vector3(0, 0, -TargetDistance);
			this.x = Mathf.SmoothDampAngle(this.x, this.targetX, ref this.xVelocity, this.needtime);
			this.y = Mathf.SmoothDampAngle(this.y, this.targetY, ref this.yVelocity, this.needtime);
			this.z = Mathf.SmoothDampAngle(this.z, this.targetZ, ref this.zVelocity, this.needtime);
			base.transform.eulerAngles = new Vector3(this.x, this.y, this.z);
			if(Line != null) {
				transform.position = Vector3.Slerp(transform.position, Line.position + pivotOffset, SmoothFactor * Time.deltaTime);
			}else{
				transform.position = Vector3.Slerp(transform.position, transform.position + pivotOffset, SmoothFactor * Time.deltaTime);
			}
			if (shakeDuration > 0) {
				mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetDist, speedTime) + Random.onUnitSphere * shakeAmount;
				shakeDuration -= decreaseFactor * Time.deltaTime;
			} else {
				mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetDist, speedTime);
				shakeDuration = 0f;
			}
		}else{
			if(simulateInEditor){
				Vector3 targetDist = new Vector3(0, 0, -TargetDistance);
				base.transform.eulerAngles = new Vector3(this.targetX, this.targetY, this.targetZ);
				mainCamera.transform.localPosition = targetDist;
				if(Line != null) {
					transform.position = Line.position + pivotOffset;
				}else{
					transform.position = transform.position + pivotOffset;
				}
			}
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
    public VariablesSaving GetCurrentVariable(){
    	var varSav = new VariablesSaving();
    	varSav.Line = Line;
    	varSav.needtime = needtime;
    	varSav.pivotOffset = pivotOffset;
    	varSav.shakeAmount = shakeAmount;
    	varSav.shakeDuration = shakeDuration;
    	varSav.SmoothFactor = SmoothFactor;
    	varSav.SmoothTime = SmoothTime;
    	varSav.TargetDistance = TargetDistance;
    	varSav.targetX = targetX;
    	varSav.targetY = targetY;
    	varSav.targetZ = targetZ;
    	varSav.position = transform.position;
    	varSav.rotation = transform.rotation;
    	varSav.simulateInEditor = simulateInEditor;
    	return varSav;
    }
    public void ApplyVariableSaving(VariablesSaving values){
    	Line = values.Line;
    	needtime = values.needtime;
    	pivotOffset = values.pivotOffset;
    	shakeAmount = values.shakeAmount;
    	shakeDuration = values.shakeDuration;
    	SmoothFactor = values.SmoothFactor;
    	SmoothTime = values.SmoothTime;
    	TargetDistance = values.TargetDistance;
    	targetX = values.targetX;
    	targetY = values.targetY;
    	targetZ = values.targetZ;
    	transform.position = values.position;
    	transform.rotation = values.rotation;
    	simulateInEditor = values.simulateInEditor;
    }
	public void Shake(float Duration, float Strength){
		shakeAmount = Strength;
		shakeDuration = Duration;
	}
	public void SuddenZ(float to){
		z = to;	
	}
    public void SetLocalZ(float to){
    	mainCamera.transform.localEulerAngles = new Vector3(0,0,to);
    }
    public void ResetCamera(int getInt){
    	ApplyVariableSaving(currentVariables);
    	transform.position = Line.position + pivotOffset;
    	SetCurrentAsTarget();
    }
    public void SetCurrentVar(int getInt){
    	currentVariables = GetCurrentVariable();
    }
    public void SetStarterAtNow(){
    	x = transform.eulerAngles.x;
    	y = transform.eulerAngles.y;
    	z = transform.eulerAngles.z;
    }
    public void StayInPosition(){
    	GameObject currPos = GameObject.CreatePrimitive (PrimitiveType.Cube);
		currPos.transform.localScale = transform.localScale;
		currPos.GetComponent<BoxCollider> ().isTrigger = true;
		currPos.GetComponent<MeshRenderer>().enabled = false;
		currPos.transform.position = Line.position;
		currPos.transform.rotation = transform.rotation;
		Line = currPos.transform;
    }
    public void SetCurrentAsTarget(){
    	x = targetX;
    	y = targetY;
    	z = targetZ;
    }
}
