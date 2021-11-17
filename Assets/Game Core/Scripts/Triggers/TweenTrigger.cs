using UnityEngine;

public class TweenTrigger : MonoBehaviour
{
	public TriggerActions tweenAction;
	public GameObject tweenedObject;
	private void OnTriggerEnter(Collider other){
		if(other.tag == "Player") DoAction(tweenedObject);
	}
	public void DoAction(GameObject obj){
		switch(tweenAction){
			case TriggerActions.Cancel:
				obj.LeanCancel();
				break;
			case TriggerActions.Pause:
				obj.LeanPause();
				break;
			case TriggerActions.Resume:
				obj.LeanResume();
				break;
		}
	}
}
public enum TriggerActions
{
	Cancel,
	Pause,
	Resume
}