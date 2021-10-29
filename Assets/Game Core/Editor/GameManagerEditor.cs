using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor (typeof (GameManager))]
public class GameManagerEditor : Editor
{
    SerializedProperty audioLoadType;
	SerializedProperty audioClip;
	SerializedProperty audioPath;
	SerializedProperty AudioOffset;
	SerializedProperty customEndDuration;
	SerializedProperty EndDuration;
	SerializedProperty tailType;
	//SerializedProperty tapKeys;
	SerializedProperty lineSpeed;
	SerializedProperty longTailTag, centerTailTag;
	SerializedProperty audioProgress;
	SerializedProperty percentageProgress;
	SerializedProperty gemsCount;
	SerializedProperty property;
	SerializedProperty levelNameView;
	SerializedProperty diePanel;
	SerializedProperty checkpointsCrowns;
	SerializedProperty DiePart, CheckpointPart;
	SerializedProperty skinType;
	void OnEnable(){
		audioLoadType = serializedObject.FindProperty ("audioLoadType");
		audioClip = serializedObject.FindProperty ("audioClip");
		audioPath = serializedObject.FindProperty ("audioPath");
		AudioOffset = serializedObject.FindProperty ("AudioOffset");
		customEndDuration = serializedObject.FindProperty ("customEndDuration");
		EndDuration = serializedObject.FindProperty ("EndDuration");
		tailType = serializedObject.FindProperty ("tailType");
		//tapKeys = serializedObject.FindProperty ("tapKeys");
		lineSpeed = serializedObject.FindProperty("lineSpeed");
		longTailTag = serializedObject.FindProperty("longTailTag");
		centerTailTag = serializedObject.FindProperty("centerTailTag");
		audioProgress = serializedObject.FindProperty("audioProgress");
		percentageProgress = serializedObject.FindProperty("percentageProgress");
		gemsCount = serializedObject.FindProperty("gemsCount");
		property = serializedObject.FindProperty("property");
		levelNameView = serializedObject.FindProperty("levelNameView");
		diePanel = serializedObject.FindProperty("diePanel");
		checkpointsCrowns = serializedObject.FindProperty ("checkpointParents");
		DiePart = serializedObject.FindProperty ("DiePart");
		CheckpointPart = serializedObject.FindProperty ("CheckpointPart");
		skinType = serializedObject.FindProperty("skinType");
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
		EditorGUILayout.LabelField("Level Variables", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField (property);
		DrawUILine();
		EditorGUILayout.LabelField("UI Variables", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField (audioProgress);
		EditorGUILayout.PropertyField (percentageProgress);
		EditorGUILayout.PropertyField (gemsCount);
		EditorGUILayout.PropertyField (levelNameView);
		EditorGUILayout.PropertyField (diePanel);
		EditorGUILayout.PropertyField (checkpointsCrowns);
		EditorGUILayout.PropertyField (DiePart);
		EditorGUILayout.PropertyField (CheckpointPart);
		DrawUILine();
		EditorGUILayout.LabelField("Audio Variables", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField (audioLoadType);
		if(audioLoadType.enumValueIndex == 0){
			EditorGUILayout.PropertyField (audioClip);
		} else {
			EditorGUILayout.PropertyField (audioPath);
			EditorGUILayout.LabelField("Only supports .ogg and .wav files.", EditorStyles.miniLabel);
		}
		EditorGUILayout.PropertyField (AudioOffset);
		EditorGUILayout.PropertyField (customEndDuration);
		if(customEndDuration.boolValue){
			EditorGUILayout.PropertyField (EndDuration);
		}
		DrawUILine();
		EditorGUILayout.LabelField("Line Variables", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(skinType);
		EditorGUILayout.PropertyField (tailType);
		//EditorGUILayout.PropertyField (tapKeys);
		EditorGUILayout.PropertyField(lineSpeed);
		EditorGUILayout.PropertyField (longTailTag);
		EditorGUILayout.PropertyField (centerTailTag);
		serializedObject.ApplyModifiedProperties ();
	}
}
