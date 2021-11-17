using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCycle : MonoBehaviour
{
	public bool enableTrigger = true;
	public bool preferTiming = true;
	public float staticTiming;
	public bool DebugMode = true;
	public bool loop;
	public List<AnimationQueue> animations;
	[HideInInspector]public int currentAnimationIndex;
	
	// NOTE : This will be called if the queue changed, but it won't be called when the queue starts.
	public Action<int> OnQueueChanged;
	
	/// <summary>
	/// Starting the animation queue
	/// </summary>
	public void StartQueue(){
		if(animations.Count != 0){
			ProcessQueue(animations[0]);
		}else{
			if(DebugMode){
				Debug.LogError("No queue was prepared");
			}
		}
	}
	
	/// <summary>
	/// Process an AnimationQueue class into a thing.
	/// </summary>
	/// <param name="queue">The queue you want to process.</param>
	public void ProcessQueue(AnimationQueue queue){
		string debugLog = "";
		float timing = 0;
		if(preferTiming){
			timing = queue.time;
		}else{
			timing = staticTiming;
		}
		if(queue.TransitionIntoObject){
			queue.targetObject.LeanMoveLocal(queue.transitionObject.transform.localPosition, timing).setEase(queue.easeType);
			queue.targetObject.LeanRotate(queue.transitionObject.transform.eulerAngles, timing).setEase(queue.easeType);
			queue.targetObject.LeanScale(queue.transitionObject.transform.localScale, timing).setEase(queue.easeType);
			debugLog += queue.targetObject.name + " is transitioned as " + queue.transitionObject.name + ". ";
		}else{
			if(queue.moveObject){
				queue.targetObject.LeanMove(queue.targetPosition, timing).setEase(queue.easeType);
				debugLog += queue.targetObject.name + " is moved to " + queue.targetPosition.ToString() + ". ";
			}
			if(queue.rotateObject){
				queue.targetObject.LeanRotate(queue.targetEulerAngles, timing).setEase(queue.easeType);
				debugLog += queue.targetObject.name + " is rotated to " + queue.targetEulerAngles.ToString() + ". ";
			}
			if(queue.scaleObject){
				queue.targetObject.LeanScale(queue.targetScale, timing).setEase(queue.easeType);
				debugLog += queue.targetObject.name + " is scaled to " + queue.targetScale.ToString() + ". ";
			}
		}
		debugLog += "as time : " + timing + ". With delay : " + queue.delay + ". Ease type : " + queue.easeType.ToString();
		if(DebugMode){
			Debug.Log(debugLog);
		}
		if(preferTiming){
			Invoke("ContinueQueue", timing + queue.delay);
		}
	}
	
	/// <summary>
	/// Continuing the latest queue.
	/// </summary>
	public void ContinueQueue(){
		currentAnimationIndex++;
		if(currentAnimationIndex > animations.Count - 1){
			if(!loop){
				if(DebugMode){
					Debug.Log("Queue Ended");
				}
			}else{
				currentAnimationIndex = 0;
				StartQueue();
			}
		}else{
			ProcessQueue(animations[currentAnimationIndex]);
			if(OnQueueChanged != null){
				OnQueueChanged.Invoke(currentAnimationIndex);
			}
		}
	}
	
	//For triggering the queue
	public void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
			if(enableTrigger){
				StartQueue();
			}
    	}
    }
}
[System.Serializable]
public class AnimationQueue{
	public string ElementName;
	public float time;
	public float delay;
	public LeanTweenType easeType;
	public GameObject targetObject;
	public bool TransitionIntoObject;
	public GameObject transitionObject;
	public bool moveObject;
	public Vector3 targetPosition;
	public bool rotateObject;
	public Vector3 targetEulerAngles;
	public bool scaleObject;
	public Vector3 targetScale;
}
