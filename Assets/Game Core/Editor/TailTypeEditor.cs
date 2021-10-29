using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor (typeof (TailType))]
public class TailTypeEditor : Editor {
	SerializedProperty primitiveType;
	SerializedProperty defaultScale;
	SerializedProperty colliderSize;
	SerializedProperty colliderIsTrigger;
	SerializedProperty addRigidbody;
	SerializedProperty constraintsAsObject;
	SerializedProperty rigidbodyMass;
	SerializedProperty spawnZOffset;
	void OnEnable(){
		primitiveType = serializedObject.FindProperty ("primitiveType");
		defaultScale = serializedObject.FindProperty ("defaultScale");
		colliderSize = serializedObject.FindProperty ("colliderSize");
		spawnZOffset = serializedObject.FindProperty ("spawnZOffset");
		colliderIsTrigger = serializedObject.FindProperty ("colliderIsTrigger");
		addRigidbody = serializedObject.FindProperty ("addRigidbody");
		constraintsAsObject = serializedObject.FindProperty ("constraintsAsObject");
		rigidbodyMass = serializedObject.FindProperty ("rigidbodyMass");
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
		EditorGUILayout.PropertyField (primitiveType);
		EditorGUILayout.PropertyField (defaultScale);
		EditorGUILayout.PropertyField (colliderSize);
		EditorGUILayout.PropertyField (spawnZOffset);
		EditorGUILayout.PropertyField (colliderIsTrigger);
		EditorGUILayout.PropertyField (addRigidbody);
		if (addRigidbody.boolValue) {
			EditorGUILayout.PropertyField (constraintsAsObject);
			EditorGUILayout.PropertyField (rigidbodyMass);
		}
		serializedObject.ApplyModifiedProperties ();
	}
}
