using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEveryBeat : MonoBehaviour
{
	public bool isEnabled;
	public float BPM = 120;
	public UnityEngine.Events.UnityEvent OnBeatStarted;
	public bool ignoreTakenStart;
	public UnityEngine.Events.UnityEvent OnBeatTaken;
	public enum TimeType{
		DeltaTime,
		FixedDeltaTime,
		SmoothDeltaTime
	}
	public TimeType timeType;
	public KeyCode startKey = KeyCode.Space;
	float currentFloat;
	bool alreadyStarted;
    void Update()
    {
    	if(Input.GetKeyDown(startKey)){
    		isEnabled = true;
    	}
    	if(isEnabled){
    		if(!alreadyStarted){
    			OnBeatStarted.Invoke();
    			if(!ignoreTakenStart){
    				OnBeatTaken.Invoke();
    			}
    			alreadyStarted = true;
    		}
	    	float timeUsed = 0;
	    	switch(timeType){
	    		case TimeType.DeltaTime:
	    			timeUsed = Time.deltaTime;
	    			break;
	    		case TimeType.FixedDeltaTime:
	    			timeUsed = Time.fixedDeltaTime;
	    			break;
	    		case TimeType.SmoothDeltaTime:
	    			timeUsed = Time.smoothDeltaTime;
	    			break;
	    	}
	    	float bpmCounted = (timeUsed * BPM) / 120;
    		if(currentFloat + bpmCounted < 1){
    			currentFloat += bpmCounted;
    		} else {
    			OnBeatTaken.Invoke();
    			currentFloat = currentFloat + bpmCounted - 1;
    		}
    	}
    }
    public void DebugLog(string message){
    	Debug.Log(message);
    }
    public void SetBPM(float to){
    	BPM = to;
    }
}
