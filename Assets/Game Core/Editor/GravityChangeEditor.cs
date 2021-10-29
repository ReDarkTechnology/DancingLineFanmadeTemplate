using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor (typeof (GravityChange))]
public class GravityChangeEditor : Editor {
	SerializedProperty isTrigger;
	SerializedProperty targetWorldRotation;
	SerializedProperty useAlgorithm;
	SerializedProperty changeTurnBlock1;
	SerializedProperty changeTurnBlock2;
	SerializedProperty defaultGravity;
	SerializedProperty gravityValue;
	private void OnEnable () {
		isTrigger = serializedObject.FindProperty("isTrigger");
		targetWorldRotation = serializedObject.FindProperty("targetWorldRotation");
		useAlgorithm = serializedObject.FindProperty("useAlgorithm");
		changeTurnBlock1 = serializedObject.FindProperty("changeTurnBlock1");
		changeTurnBlock2 = serializedObject.FindProperty("changeTurnBlock2");
		defaultGravity = serializedObject.FindProperty("defaultGravity");
		gravityValue = serializedObject.FindProperty("gravityValue");
	}
	public override void OnInspectorGUI () {
		serializedObject.Update ();
		EditorGUILayout.LabelField ("Enable if you're using this as a trigger", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (isTrigger);
		DrawUILine ();
		EditorGUILayout.LabelField ("Perspective the world gravity", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (targetWorldRotation);
		EditorGUILayout.LabelField ("If enabled. The values below this field will be automatically generated (kinda glitchy)", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (useAlgorithm);
		EditorGUILayout.PropertyField (changeTurnBlock1);
		EditorGUILayout.PropertyField (changeTurnBlock2);
		DrawUILine ();
		EditorGUILayout.PropertyField (defaultGravity);
		EditorGUILayout.PropertyField (gravityValue);
		serializedObject.ApplyModifiedProperties();
	}
	public static void DrawUILine (int thickness = 2, int padding = 10) {
		Rect r = EditorGUILayout.GetControlRect (GUILayout.Height (padding + thickness));
		Color color = new Color (.65f, .65f, .65f);
		r.height = thickness;
		r.y += padding / 2;
		r.x -= 2;
		EditorGUI.DrawRect (r, color);
	}
}