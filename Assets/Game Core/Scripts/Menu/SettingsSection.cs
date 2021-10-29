using System;
using UnityEngine.UI;
using UnityEngine;

public class SettingsSection : MonoBehaviour
{
	void Start(){
		UpdateText();
	}
	
	[Header("Quality")]
	public Text qualityText;
	public void ChangeQuality(int index){
		QualitySettings.SetQualityLevel(Mathf.Clamp(index, 0, QualitySettings.names.Length - 1));
		UpdateText();
	}
	public void ToggleQuality(){
		var nowIndex = QualitySettings.GetQualityLevel();
		var maxIndex = QualitySettings.names.Length - 1;
		nowIndex++;
		if(nowIndex > maxIndex){
			nowIndex = 0;
		}
		ChangeQuality(nowIndex);
	}
	public void UpdateText(){
		if(qualityText != null) qualityText.text = "Quality : " + QualitySettings.names[QualitySettings.GetQualityLevel()];
	}
}
