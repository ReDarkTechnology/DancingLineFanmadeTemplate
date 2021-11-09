using System;
using System.Collections.Generic;
using UnityEngine;

public class TweenHost
{
	public GameObject obj;
	public TweenHost(GameObject obj){
		this.obj = obj;
	}
	public void CancelTweens(){
		obj.LeanCancel();
	}
	public void PauseTweens(){
		obj.LeanPause();
	}
	public void ContinueTweens(){
		obj.LeanResume();
	}
}