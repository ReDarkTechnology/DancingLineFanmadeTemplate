using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor (typeof (LevelProperty))]
public class LevelPropertyEditor : Editor {
	SerializedProperty LevelID;
	SerializedProperty LevelName;
	SerializedProperty LevelMusic;
	SerializedProperty sceneName;
	SerializedProperty clip;
	SerializedProperty totalGems;
	private void OnEnable () {
		LevelID = serializedObject.FindProperty("LevelID");
		LevelName = serializedObject.FindProperty("LevelName");
		LevelMusic = serializedObject.FindProperty("LevelMusic");
		sceneName = serializedObject.FindProperty("sceneName");
		clip = serializedObject.FindProperty("clip");
		totalGems = serializedObject.FindProperty("totalGems");
	}
	public override void OnInspectorGUI () {
		serializedObject.Update ();
		EditorGUILayout.LabelField ("Level Property - v.1.0.1", EditorStyles.boldLabel);
		DrawUILine( );
		EditorGUILayout.LabelField ("Used to store player preferences", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (LevelID);
		EditorGUILayout.LabelField ("The name of the level", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (LevelName);
		EditorGUILayout.LabelField ("The credit of the level music", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (LevelMusic);
		EditorGUILayout.LabelField ("The scene name", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (sceneName);
		EditorGUILayout.LabelField ("The audio clip of the level", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (clip);
		EditorGUILayout.LabelField ("The total gems in the level", EditorStyles.miniLabel);
		EditorGUILayout.PropertyField (totalGems);
		if(GUILayout.Button ("Play Audio")){
		    var targ = ((LevelProperty)target);
		    var gameObject = new GameObject();
		    var source = gameObject.GetComponent<AudioSource>();
		    if(source == null){
		        source = gameObject.AddComponent<AudioSource>();
		    }
		    source.clip = targ.clip;
            gameObject.name = targ.clip.name;
		    source.Play();
		}
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