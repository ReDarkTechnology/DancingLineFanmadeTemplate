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
}
