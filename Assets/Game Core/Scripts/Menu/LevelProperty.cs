using UnityEngine;
[CreateAssetMenu(fileName = "Level Property", menuName = "Line Game/Local Level Property", order = 0)]
public class LevelProperty : ScriptableObject
{
	public string LevelID;
	public string LevelName;
	public string LevelMusic;
	public string sceneName;
	public int totalGems;
	public AudioClip clip;
	
	public float GetProgress () {
		return float.Parse(Configuration.GetString("Progress-" + LevelID, "0"));
	}
	public int GetGems() {
		return int.Parse(Configuration.GetString("Gems-" + LevelID, "0"));
	}
	public int GetCrowns() {
		return int.Parse(Configuration.GetString("Crown-" + LevelID, "0"));
	}
	
	public void SaveProgress (float progress, bool ignoreHighScore = false) {
		if(!ignoreHighScore){
			if(GetProgress() < progress) 
				Configuration.SetString("Progress-" + LevelID, progress.ToString());
		}else{
			Configuration.SetString("Progress-" + LevelID, progress.ToString());
		}
	}
	public void SaveGems (int gems, bool ignoreHighScore = false) {
		if(!ignoreHighScore){
			if(GetGems() < gems)
				Configuration.SetString("Gems-" + LevelID, gems.ToString());
		}else{
			Configuration.SetString("Gems-" + LevelID, gems.ToString());
		}
	}
	public void SaveCrowns (int crowns, bool ignoreHighScore = false) {
		if(!ignoreHighScore){
			if(GetProgress() < crowns) 
				Configuration.SetString("Crown-" + LevelID, crowns.ToString());
		}else{
			Configuration.SetString("Crown-" + LevelID, crowns.ToString());
		}
	}
}
