using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (Trig))]
[CanEditMultipleObjects]
public class TrigEditor : Editor {
	SerializedProperty TrigType;
	SerializedProperty TargetAngleRotation, Smoothness, RotationSmoothness, PivOff, CamDis;
	SerializedProperty ChangeTargetObject, TargetObjectToSee, TargetFactor;
	SerializedProperty HighJumpA, tlp;
	SerializedProperty TargetSpeedA;
	SerializedProperty DT, DS, CT, CS, CE;
	SerializedProperty rrs, mct, ls, mbt, bt;
	SerializedProperty TargetObject;
	SerializedProperty SetTo, endLerp;
	SerializedProperty dur;
	SerializedProperty shakeDur,shakeStrength;
	SerializedProperty animaori;
	SerializedProperty easeType;
	SerializedProperty targetObject;
	SerializedProperty moveObject;
	SerializedProperty rotateObject;
	SerializedProperty scaleObject;
	SerializedProperty easeTime;
	SerializedProperty targetPosition;
	SerializedProperty targetRotation;
	SerializedProperty targetScale;
	SerializedProperty transitionIntoObject;
	SerializedProperty currentVar;
	SerializedProperty animaTarg;
	private void OnEnable () {
		animaTarg = serializedObject.FindProperty("animaTarg");
		TargetFactor = serializedObject.FindProperty("TargetFactor");
        PivOff = serializedObject.FindProperty("TargetPivotOffset");
		ChangeTargetObject = serializedObject.FindProperty ("ChangeTargetObject");
		TargetObjectToSee = serializedObject.FindProperty ("TargetObjectToSee");
		endLerp = serializedObject.FindProperty ("endLerp");
		TargetObject = serializedObject.FindProperty ("TargetObject");
		SetTo = serializedObject.FindProperty ("SetTo");
		rrs = serializedObject.FindProperty ("renderers");
		ls = serializedObject.FindProperty ("lerpSpeed");
		mct = serializedObject.FindProperty ("meshColorTo");
		bt = serializedObject.FindProperty ("byTag");
		mbt = serializedObject.FindProperty ("meshByTag");
		DT = serializedObject.FindProperty ("DensityTo");
		DS = serializedObject.FindProperty ("DensitySpeed");
		CT = serializedObject.FindProperty ("ColorTo");
		CS = serializedObject.FindProperty ("ColorSpeed");
		CE = serializedObject.FindProperty ("changeEmmision");
		TrigType = serializedObject.FindProperty ("TriggerTypes");
		Smoothness = serializedObject.FindProperty ("TargetSmoothing");
		TargetAngleRotation = serializedObject.FindProperty ("TargetAngleRotation");
		HighJumpA = serializedObject.FindProperty ("HighJump");
		TargetSpeedA = serializedObject.FindProperty ("TargetSpeed");
		RotationSmoothness = serializedObject.FindProperty ("TargetRotationSmoothing");
		dur = serializedObject.FindProperty ("Duration");
		shakeDur = serializedObject.FindProperty ("ShakeDuration");
		shakeStrength = serializedObject.FindProperty ("ShakeStrength");
		tlp = serializedObject.FindProperty("TestLoopCount");
		animaori = serializedObject.FindProperty("animaTarg");
		CamDis = serializedObject.FindProperty("TargetCamDistance");
		easeType = serializedObject.FindProperty("easeType");
		targetObject = serializedObject.FindProperty("targetObject");
		moveObject = serializedObject.FindProperty("moveObject");
		rotateObject = serializedObject.FindProperty("rotateObject");
		scaleObject = serializedObject.FindProperty("scaleObject");
		easeTime = serializedObject.FindProperty("easeTime");
		targetPosition = serializedObject.FindProperty("targetPosition");
		targetRotation = serializedObject.FindProperty("targetRotation");
		targetScale = serializedObject.FindProperty("targetScale");
		transitionIntoObject = serializedObject.FindProperty("transitionIntoObject");
		currentVar = serializedObject.FindProperty("currentVar");
	}

	public static void DrawUILine (int thickness = 2, int padding = 10) {
		Rect r = EditorGUILayout.GetControlRect (GUILayout.Height (padding + thickness));
		Color color = new Color (.65f, .65f, .65f);
		r.height = thickness;
		r.y += padding / 2;
		r.x -= 2;
		EditorGUI.DrawRect (r, color);
	}

	public override void OnInspectorGUI () {
		serializedObject.Update ();
		EditorGUILayout.LabelField ("Trigger", EditorStyles.boldLabel);
		EditorGUILayout.LabelField ("When the trigger visible in the editor...", EditorStyles.miniLabel);
		EditorGUILayout.LabelField ("In playmode the trigger will not visible.", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (TrigType);
		DrawUILine ();
		switch (TrigType.enumValueIndex) {
			case 0:
				EditorGUILayout.PropertyField (TargetAngleRotation);
				EditorGUILayout.PropertyField (CamDis);
				EditorGUILayout.PropertyField (Smoothness);
				EditorGUILayout.PropertyField(TargetFactor);
				EditorGUILayout.PropertyField (RotationSmoothness);
				EditorGUILayout.PropertyField (ChangeTargetObject);
                EditorGUILayout.PropertyField (PivOff);
				if (ChangeTargetObject.boolValue == true) {
					EditorGUILayout.PropertyField (TargetObjectToSee);
				}
                if(!((Trig)target).isSimulating){
	                if(GUILayout.Button("Preview")){
                		BetterCamera cam = FindObjectOfType<BetterCamera>();
                		cam.SetCurrentVar(-1);
                		Vector3 angle = TargetAngleRotation.vector3Value;
                		cam.targetX = angle.x;
                		cam.targetY = angle.y;
                		cam.targetZ = angle.z;
                		cam.TargetDistance = CamDis.floatValue;
                		cam.pivotOffset = PivOff.vector3Value;
                		cam.simulateInEditor = true;
                		((Trig)target).isSimulating = true;
	                }
                }
                if(((Trig)target).isSimulating){
	                if(GUILayout.Button("Reset")){
	                	BetterCamera cam = FindObjectOfType<BetterCamera>();
	                	try {
	                		cam.ResetCamera(-1);
	                	} catch {
	                		cam.SetCurrentVar(-1);
	                	}
	                	((Trig)target).isSimulating = false;
	                }
                }
				break;
			case 1:
				EditorGUILayout.PropertyField (HighJumpA);
				if(GUILayout.Button("Test Jump")){
					TestJumping();
				}
				EditorGUILayout.PropertyField (tlp);
				break;
			case 2:
				EditorGUILayout.PropertyField (TargetSpeedA);
				break;
			case 3:
				EditorGUILayout.PropertyField (DT);
				EditorGUILayout.PropertyField (DS);
				EditorGUILayout.PropertyField (CT);
				EditorGUILayout.PropertyField (CS);
				EditorGUILayout.PropertyField (endLerp);
				break;
			case 4:
				if (mbt.boolValue == true) {
					EditorGUILayout.PropertyField (bt);
				} else {
					EditorGUILayout.PropertyField (rrs);
				}
				EditorGUILayout.PropertyField (mct);
				EditorGUILayout.PropertyField (ls);
				EditorGUILayout.PropertyField (endLerp);
				EditorGUILayout.PropertyField (CE);
				DrawUILine ();
				EditorGUILayout.PropertyField (mbt);
				break;
			case 5:
				EditorGUILayout.PropertyField (TargetObject);
				EditorGUILayout.PropertyField (SetTo);
				break;
			case 6:
				EditorGUILayout.PropertyField (CT);
				EditorGUILayout.PropertyField (ls);
				EditorGUILayout.PropertyField (endLerp);
				break;
			case 7:
				EditorGUILayout.PropertyField (dur);
				break;
			case 8:
				EditorGUILayout.PropertyField (shakeDur);
				EditorGUILayout.PropertyField (shakeStrength);
				break;
			case 9:
				EditorGUILayout.PropertyField (animaTarg);
				break;
			case 10:
				EditorGUILayout.PropertyField (easeType);
				EditorGUILayout.PropertyField (targetObject);
				EditorGUILayout.PropertyField (moveObject);
				EditorGUILayout.PropertyField (rotateObject);
				EditorGUILayout.PropertyField (scaleObject);
				EditorGUILayout.PropertyField (easeTime);
				EditorGUILayout.PropertyField (targetPosition);
				EditorGUILayout.PropertyField (targetRotation);
				EditorGUILayout.PropertyField (targetScale);
				EditorGUILayout.PropertyField (transitionIntoObject);
				break;
		}

		serializedObject.ApplyModifiedProperties ();
	}
	public void TestJumping(){
		if (Application.isPlaying)
        {
			((Trig)target).TestJumping();
		}
	}
}