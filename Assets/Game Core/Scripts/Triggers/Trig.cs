using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trig : MonoBehaviour {
	public enum TrigType {
		Camera,
		Jump,
		Speed,
		Fog,
		MeshColor,
		SetActive,
		CameraBackground,
		StopAWhile,
		ShakeCam,
		StartingAnimation,
		Animation
	}
	public TrigType TriggerTypes;
	public BetterCamera cam;
	public Vector3 TargetAngleRotation = new Vector3(45,45,0), TargetPivotOffset;
	public float TargetCamDistance = 20;
	public float TargetSmoothing = 1,TargetRotationSmoothing = 1;
	public float TargetFactor = 1;
	public bool ChangeTargetObject;
	public Transform TargetObjectToSee;
	[HideInInspector] public bool isSimulating = false;
	[HideInInspector] public BetterCamera.VariablesSaving currentVar;
	LineMovement Got;
	
	//Jump
	public float HighJump = 500;
	[Range(1,2)]
	public int TestLoopCount = 2;
	//Speed
	public float TargetSpeed = 10;
	[HideInInspector] 
	public float previousSpeed;
	
	//Fog
	public float DensityTo = 0.01f;
	public float DensitySpeed = 5;
	public Color ColorTo = Color.white;
	public float ColorSpeed = 5, endLerp = 5;
	bool lerp;
	//MeshColor
	public MeshRenderer renderers;
	public bool materialChange;
	public Material targetMaterial;
	public Color meshColorTo = Color.black;
	public bool changeEmmision;
	public float lerpSpeed = 5;
	public bool meshByTag;
		//ifByTag
		public string byTag;
	//SetActive
	public GameObject TargetObject;
	public bool SetTo;
	//StopAWhile
	public float Duration = 12.5f;
	//Shake
	public float ShakeDuration = 0.3f;
	public float ShakeStrength = 1;
	//JumpTest
	public void TestJumping(){
		if(TestLoopCount == 1){
			Got.transform.position = transform.position + new Vector3(-1,0,0);
			Got.loopCount = TestLoopCount;
			Got.StartGame();
		}else{
			Got.transform.position = transform.position + new Vector3(0,0,-1);
			Got.loopCount = TestLoopCount;
			Got.StartGame();
		}
	}
	//Animation
	public Animator animaTarg;
	//Animate
	public LeanTweenType easeType;
	public Transform targetObject;
	public bool moveObject;
	public bool rotateObject;
	public bool scaleObject;
	public float easeTime;
	public Vector3 targetPosition;
	public Vector3 targetRotation;
	public Vector3 targetScale;
	public Transform transitionIntoObject;
	
	//NO WAY PLEASE THIS IS FOR CHECKPOINT QWQ
	public int checkpointGot;
	public Vector3 CurrentAngleRot, CurrentPivotOffset;
	public float CurrentCamDistance = 20;
	public float CurrentSmoothing = 1,CurrentRotationSmoothing = 1;
	public float CurrentFactor = 1;
	public Transform CurrentObjectToSee;
	
	List<MeshRenderer> cachedRenderers = new List<MeshRenderer>();
	// Use this for initialization
	void Start () {
		cam = FindObjectOfType<BetterCamera>();
		Got = FindObjectOfType<LineMovement>();
		FindObjectOfType<GameManager>().OnCheckpointReset += ResetTrigger;
	}
	
	// Update is called once per frame
	void Update () {
		if (lerp && TriggerTypes == TrigType.MeshColor) {
			if (meshByTag) {
				foreach (MeshRenderer nui in cachedRenderers) {
					nui.material.color = Color.Lerp (nui.material.color, meshColorTo, lerpSpeed * Time.deltaTime);
					nui.material.SetColor("_EmissionColor", Color.Lerp (nui.material.color, meshColorTo, lerpSpeed * Time.deltaTime));
				}
			} else {
				if(!materialChange){
					if(renderers != null){
						renderers.material.color = Color.Lerp (renderers.material.color, meshColorTo, lerpSpeed * Time.deltaTime);
						if(changeEmmision){
							renderers.material.SetColor("_EmissionColor", Color.Lerp (renderers.material.color, meshColorTo, lerpSpeed * Time.deltaTime));
						}
					}
				}else{
					if(targetMaterial != null){
						targetMaterial.color = Color.Lerp (renderers.material.color, meshColorTo, lerpSpeed * Time.deltaTime);
						if(changeEmmision){
							targetMaterial.SetColor("_EmissionColor", Color.Lerp (renderers.material.color, meshColorTo, lerpSpeed * Time.deltaTime));
						}
					}
				}
			}
		}
		if (lerp && TriggerTypes == TrigType.Fog) {
			// disable CompareOfFloatsByEqualityOperator
			if (DensitySpeed == 0 && ColorSpeed == 0) {
				lerp = false;
			} else {
				if (DensitySpeed > 0) {
					RenderSettings.fogDensity = Mathf.Lerp (RenderSettings.fogDensity, DensityTo, DensitySpeed * Time.deltaTime);
				}
				if (ColorSpeed > 0) {
					RenderSettings.fogColor = Color.Lerp (RenderSettings.fogColor, ColorTo, ColorSpeed * Time.deltaTime);
				}
			}
		}
		if (lerp && TriggerTypes == TrigType.CameraBackground) {
			if (lerpSpeed == 0) {
				lerp = false;
			} else {
                Camera.main.GetComponent<Camera>().backgroundColor = Color.Lerp(Camera.main.GetComponent<Camera>().backgroundColor, ColorTo, lerpSpeed * Time.deltaTime);
			}
		}
	}
	private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			checkpointGot = FindObjectOfType<GameManager>().CurrentCheckpoint;
			Got = other.GetComponent<LineMovement>();
			switch (TriggerTypes) {
			case TrigType.Camera:
					CurrentAngleRot = new Vector3(cam.targetX,cam.targetY, cam.targetZ);
					CurrentCamDistance=cam.TargetDistance;
					CurrentObjectToSee=cam.Line;
					CurrentPivotOffset=cam.pivotOffset;
					CurrentRotationSmoothing=cam.needtime;
					CurrentSmoothing = cam.SmoothTime;
					CurrentFactor = cam.SmoothFactor;
					cam.ChangeVar (TargetAngleRotation, TargetSmoothing, TargetRotationSmoothing,TargetPivotOffset, TargetCamDistance);
					cam.SmoothFactor =  TargetFactor;
					if (ChangeTargetObject) {
	                    cam.Line = TargetObjectToSee;
					}
				break;
			case TrigType.Jump:
				//other.GetComponent<LineMovement> ().NoTail = true;
				Invoke ("ReturnTail", HighJump / 1000);
                //other.GetComponent<LineMovement>().recentlyJumping = true;
                other.GetComponent<Rigidbody>().mass = 1;
				other.GetComponent<Rigidbody>().AddForce (Vector3.up * HighJump);
				break;
			case TrigType.Speed:
				FindObjectOfType<GameManager>().lineSpeed = TargetSpeed;
				break;
			case TrigType.Fog:
				lerp = true;
				Invoke ("EndedLerping", endLerp);
				break;
			case TrigType.MeshColor:
				lerp = true;
				if(meshByTag){
					GameObject[] collection = GameObject.FindGameObjectsWithTag (byTag);
					cachedRenderers.Clear();
					foreach(var a in collection){
						var rend = a.GetComponent<MeshRenderer>();
						if(rend != null){
							cachedRenderers.Add(rend);
						}
					}
				}
				Invoke ("EndedLerping", endLerp);
				break;
			case TrigType.SetActive:
				TargetObject.SetActive (SetTo);
				break;
			case TrigType.CameraBackground:
				lerp = true;
				Invoke ("EndedLerping", endLerp);
				break;
			case TrigType.StopAWhile:
				Invoke ("RevokeStopping", Duration);
				Got.isStarted = false;
				Got.isControllable = false;
				break;
			case TrigType.ShakeCam:
				cam.Shake (ShakeDuration,ShakeStrength);
				break;
			case TrigType.StartingAnimation:
				animaTarg.enabled = true;
				break;
			case TrigType.Animation:
				if(targetObject.GetComponent<TransformKeeper>() == null){
					targetObject.gameObject.AddComponent<TransformKeeper>();
				}
				if(transitionIntoObject != null){
					if(moveObject){
						targetObject.LeanMoveLocal(transitionIntoObject.localPosition, easeTime).setEase(easeType);
					}
					if(rotateObject){
						targetObject.LeanRotate(transitionIntoObject.eulerAngles, easeTime).setEase(easeType);
					}
					if(scaleObject){
						targetObject.LeanScale(transitionIntoObject.localScale, easeTime).setEase(easeType);
					}
				}else{
					if(moveObject){
						targetObject.LeanMoveLocal(targetPosition, easeTime).setEase(easeType);
					}
					if(rotateObject){
						targetObject.LeanRotate(targetRotation, easeTime).setEase(easeType);
					}
					if(scaleObject){
						targetObject.LeanScale(targetScale, easeTime).setEase(easeType);
					}
				}
				break;
			}
		}
	}
	void EndedLerping(){
		lerp = false;
	}
	/*void ReturnTail(){
		Got.GetComponent<LineMovement> ().MakeBlock = false;
		Got.GetComponent<LineMovement> ().NoTail = false;
	}*/
	public void ResetTrigger(int checkSort){
		if(checkpointGot == checkSort){
			switch (TriggerTypes) {
				case TrigType.Camera:
					cam.targetX = CurrentAngleRot.x;
					cam.targetY = CurrentAngleRot.y;
					cam.targetZ = CurrentAngleRot.z;
					cam.TargetDistance = CurrentCamDistance;
					cam.Line = CurrentObjectToSee;
					cam.pivotOffset = CurrentPivotOffset;
					cam.needtime = CurrentRotationSmoothing;
					cam.SmoothTime = CurrentSmoothing;
					cam.SmoothFactor = CurrentFactor;
					break;
			}
		}
	}
	void RevokeStopping(){
		Got.isStarted = true;
		Got.isControllable = true;
	}
}
