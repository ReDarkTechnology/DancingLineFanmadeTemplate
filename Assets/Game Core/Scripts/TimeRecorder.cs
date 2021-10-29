using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeRecorder : MonoBehaviour
{
	public AudioSource audioSource;
	public KeyCode code = KeyCode.UpArrow;
	public KeyCode saveKey = KeyCode.S;
	public List<float> timings;
	public string fileSavingName;
	float timeNow;
    // Start is called before the first frame update
    void Start()
    {
    	if(fileSavingName != null){
    		if(File.Exists(Path.Combine(Application.persistentDataPath, fileSavingName))){
    			string readString = File.ReadAllText(Path.Combine(Application.persistentDataPath, fileSavingName));
    			string[] strings = readString.Split(new char[] {','});
    			foreach(string ae in strings){
    				timings.Add(float.Parse(ae));
    			}
    		}
    	}
    }

    // Update is called once per frame
    void Update()
    {
    	timeNow += audioSource.time;
    	if(Input.GetKeyDown(code)){
    		timings.Add(timeNow);
    	}
    	if(Input.GetKeyDown(saveKey)){
    		SavePreference();
    	}
    }
    public void SavePreference(){
    	if(fileSavingName != null){
    		string to = timings[0].ToString();
	    	foreach(float yes in timings){
	    		if(yes != timings[0]){
	    			to += "," + yes.ToString();
	    		}
	    	}
	    	File.WriteAllText(Path.Combine(Application.persistentDataPath, fileSavingName), to);
    	}
    }
}
