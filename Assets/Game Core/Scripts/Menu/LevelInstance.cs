using UnityEngine;
using UnityEngine.Playables;

public class LevelInstance : MonoBehaviour
{
	public LevelProperty property;
	public Transform cameraPosition;
	public Color backgroundColor = new Color(1, 0.9838397f, 0.7311321f, 0);
	public Color textColor = Color.black;
	public Color particleColor = Color.gray;
	[Range(0,1)]
	public float audioStart = 0.2f;
	[Range(0,1)]
	public float audioEnd = 0.65f;
	public GameObject[] crowns;
	
	public Animator[] animators;
	public PlayableDirector[] directors;
	
	public Requirements requirements = new Requirements();
	
	public void UpdateCrowns(){
		foreach(var a in crowns){
			a.SetActive(false);
		}
		var crown_ = int.Parse(Configuration.GetString("Crown-" + property.LevelID, "0"));
		for(int i = 0; i < crown_; i++){
			crowns[i].SetActive(true);
		}
	}
	public void SetAnimation(bool to){
		foreach(var a in animators){
			a.enabled = to;
		}
		foreach(var d in directors){
			d.enabled = to;
		}
	}
}

[System.Serializable]
public class Requirements {
	public bool FreeToUnlock;
	
	[Header("NotFreeToUnlock")]
	public LevelProperty RequiredLevel;
	public bool AllCrownsNeeded;
	public bool AllGemsNeeded;
}
