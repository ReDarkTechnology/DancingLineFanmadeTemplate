using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class Configuration
{
	public static string configPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "config.json";
	public static string dirPath = Application.persistentDataPath;
    // A string field class to store the data;
    [Serializable]
	public class StringField {
		public string name;
		public string value;
	}
	// A prefs data class for saving the entire fields into one data.
	[Serializable]
	public class PrefsData {
		public string computerUniqueID;
		public List<StringField> prefs;
	}
	/// <summary>
	/// Saving a string into the CustomPlayerPrefs database.
	/// </summary>
	/// <param name="name">The prefs name</param>
	/// <param name="value">The prefs value</param>
	public static void SetString(string name, string value){
		// Check if config existed
		try {
			if(ConfigExists()){
				// Fetch the data from the config
				var data = JsonUtility.FromJson<PrefsData>(GetFileData());
				// Search the data in the config.
				int result = ValueExists(data.prefs, name);
				// If result is above -1 then the value exist. Commit modify the field.
				if(result > -1){
					data.prefs[result].value = value;
				}else{
					// If result is -1 then the value didn't exist. Create a new field.
					var field = new StringField();
					field.name = name;
					field.value = value;
					data.prefs.Add(field);
				}
				// Save the config file.
				SaveFile(JsonUtility.ToJson(data));
			}else{
				// Create a new config as there's no config saved.
				// Initialize the prefs data.
				var data = new PrefsData();
				data.prefs = new List<StringField>();
				// Make a new field.
				var field = new StringField();
				field.name = name;
				field.value = value;
				data.prefs.Add(field);
				// Save the config file.
				SaveFile(JsonUtility.ToJson(data));
			}
		} catch {
			// Create a new config as there's no config saved.
			// Initialize the prefs data.
			var data = new PrefsData();
			data.prefs = new List<StringField>();
			//data.computerUniqueID = SystemInfo.deviceUniqueIdentifier;
			// Make a new field.
			var field = new StringField();
			field.name = name;
			field.value = value;
			data.prefs.Add(field);
			// Save the config file.
			SaveFile(JsonUtility.ToJson(data));
		}
	}
	/// <summary>
	/// Getting a string from the CustomPlayerPrefs database
	/// </summary>
	/// <param name="name">Field name</param>
	/// <param name="defaultValue">If there's no data found, return the default value</param>
	/// <returns>The fetched data, if the data is not found and there's no default value set, return null.</returns>
	public static string GetString(string name, string defaultValue = null){
		string result = defaultValue;
		try {
			if(ConfigExists()){
				var data = JsonUtility.FromJson<PrefsData>(GetFileData());
				int index = ValueExists(data.prefs, name);
				if(index > -1){
					result = data.prefs[index].value;
				}
			}
		} catch {
			return defaultValue;
		}
		return result;
	}
	static int ValueExists(List<StringField> val, string name){
		int index = 0;
		foreach(StringField a in val){
			if(a.name == name){
				return index;
			}
			index++;
		}
		return (-1);
	}
	static bool ConfigExists(){
		return File.Exists(configPath);
	}
	static string GetFileData(){
		return File.ReadAllText(configPath);
	}
	public static string GetRidOfLastPath(string dir){
		var e = dir.Split(Path.DirectorySeparatorChar);
		var y = dir.Remove(dir.Length - e[e.Length - 1].Length, e[e.Length - 1].Length);
		return y;
	}
	static void SaveFile(string data){
		if(!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
		File.WriteAllText(configPath, data);
	}
}