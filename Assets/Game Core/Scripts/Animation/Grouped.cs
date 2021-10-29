using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grouped : MonoBehaviour
{
	public bool start;
	public float delay = 1;
	public enum TriggerType{
		Hammers,
		PianoKeys
	}
	public TriggerType type;
	public BounceHammer[] hammers;
	public PianoKey[] keys;
	public int currentIndex;
	//Checkpoint
	int tempIndex;
	int CheckpointSort;
    void Start()
    {
    	GameManager core = FindObjectOfType<GameManager>();
    	core.OnCheckpointObtained += SetSort;
        core.OnCheckpointReset += ResetObject;
    	if(type == TriggerType.Hammers){
    		hammers = gameObject.GetComponentsInChildren<BounceHammer>();
    	}
    	if(type == TriggerType.PianoKeys){
    		keys = gameObject.GetComponentsInChildren<PianoKey>();
    	}
    }
    void Update()
    {
    	if(start){
    		Call();
    		start = false;
    	}
    }
    public void SetSort(int ham){
    	CheckpointSort = ham;
    	tempIndex = currentIndex;
    }
    public void ResetObject(int ha)
    {
        if (CheckpointSort == ha)
        {
        	currentIndex = tempIndex;
        }
    }
    public void ReCall(){
    	Call();
    }
    public void Call(bool repeat = true){
    	if(type == TriggerType.Hammers){
    		hammers[currentIndex].process = true;
    	}
    	if(type == TriggerType.PianoKeys){
    		keys[currentIndex].process = true;
    	}
    	currentIndex++;
    	if(repeat){
    		Invoke("ReCall", delay);
    	}
    }
}
