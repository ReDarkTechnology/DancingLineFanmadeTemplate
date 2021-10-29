using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelList : MonoBehaviour
{
	[Header("Level Listing")]
	public bool isVisible = true;
	public GameObject[] LevelsObjects;
	public LevelInstance[] levelInstances;
	public int levelIndex;
	public Text levelTitle;
	public Text levelGems;
	public Text levelPercentage;
	public Image[] levelUIImages;
	public float fadeSpeed = 0.5f;
	public Button nextButton;
	public Button previousButton;
	public Button playButton;
	public Image lockSprite;
	public ParticleSystem particleBackground;
	public AudioSource source;
	[Range(0,1)]
	public float startTime = 0.2f;
	[Range(0,1)]
	public float endTime = 0.65f;
	
	[Header("Settings")]
	public GameObject[] SettingsObject;
	private static LevelList m_self;
	public static LevelList self {
		get {
			if(m_self == null) m_self = FindObjectOfType<LevelList>();
			return m_self;
		}
	}
	
	public void SetUIImagesColorTo(Color to){
		foreach(var a in levelUIImages){
			a.color = to;
		}
	}
	void Start(){
		currentOrder = null;
		m_self = this;
		levelIndex = int.Parse(Configuration.GetString("MenuSelected", "0"));
		ViewLevel(false);
	}
	public void ViewLevel(bool fade = true){
		Configuration.SetString("MenuSelected", levelIndex.ToString());
		if(Application.isPlaying){
			LeanTween.cancelAll();
		}
		var a = levelInstances[levelIndex];
		startTime = a.audioStart;
		endTime = a.audioEnd;
		if(Application.isPlaying){
			source.clip = a.property.clip;
			StartCoroutine(PlayAudio(0.1f));
		}
		levelTitle.text = a.property.LevelName;
		levelPercentage.text = (float.Parse(Configuration.GetString("Progress-"+a.property.LevelID, "0")) * 100).ToString("0") + "%";
		levelGems.text = (int.Parse(Configuration.GetString("Gems-"+a.property.LevelID, "0"))) + "/" + a.property.totalGems;
		a.UpdateCrowns();
		a.SetAnimation(true);
		nextButton.interactable = (levelIndex < levelInstances.Length - 1);
		previousButton.interactable = (levelIndex > 0);
		if(a.requirements.FreeToUnlock){
			lockSprite.gameObject.SetActive(false);
			playButton.interactable = true;
		}else{
			bool IsPassBy = true;
			if(a.requirements.RequiredLevel != null){
				var req = a.requirements.RequiredLevel;
				IsPassBy = req.GetProgress() >= 1;
				if(a.requirements.AllCrownsNeeded) IsPassBy = req.GetCrowns() >= 3;
				if(a.requirements.AllGemsNeeded) IsPassBy = req.GetGems() >= req.totalGems;
			}
			lockSprite.gameObject.SetActive(!IsPassBy);
			playButton.interactable = IsPassBy;
		}
		if(!Application.isPlaying){
			fade = false;
		}
		if(fade){
			Vector3 start = Camera.main.transform.position;
			LeanTween.value(this.gameObject, start, a.cameraPosition.transform.position, fadeSpeed).setEase(LeanTweenType.easeOutCubic).setOnUpdate((Vector3 val) => {
		     	Camera.main.transform.position = val;
		     });
			Color camColor = Camera.main.backgroundColor;
			LeanTween.value(this.gameObject, camColor, a.backgroundColor, fadeSpeed).setEase(LeanTweenType.easeOutCubic)
				.setOnUpdate((Color val) => {
             		Camera.main.backgroundColor = val;
             	}
		    );
			var main = particleBackground.main;
			ParticleSystem.MinMaxGradient particleColor = main.startColor;
			LeanTween.value(this.gameObject, main.startColor.color, a.particleColor, fadeSpeed).setEase(LeanTweenType.easeOutCubic)
				.setOnUpdate((Color val) => {
                 	main.startColor = val;
		     	}
		     );
			Color prevColor = levelTitle.color;
			LeanTween.value(this.gameObject, prevColor, a.textColor, fadeSpeed).setEase(LeanTweenType.easeOutCubic)
				.setOnUpdate((Color val) => {
				             	levelTitle.color = val;
				             	SetUIImagesColorTo(val);
				             	levelPercentage.color = val;
				             	levelGems.color = val;
				             	lockSprite.color = val;
				             	previousButton.targetGraphic.color = val;
				             	nextButton.targetGraphic.color = val;
             	}
		    );
		}else{
			Camera.main.transform.position = a.cameraPosition.transform.position;
			Camera.main.backgroundColor = a.backgroundColor;
			var main = particleBackground.main;
         	main.startColor = a.particleColor;
         	levelTitle.color = a.textColor;
         	SetUIImagesColorTo(a.textColor);
         	levelPercentage.color = a.textColor;
			lockSprite.color = a.textColor;
         	levelGems.color = a.textColor;
         	previousButton.targetGraphic.color = a.textColor;
         	nextButton.targetGraphic.color = a.textColor;
		}
	}
	IEnumerator PlayAudio(float delay){
		yield return new WaitForSeconds(delay);
		var start = GettingAudioPart(startTime);
		var end = GettingAudioPart(endTime);
		var delaya = (end - start) - 0.5f;
		var actualDelay = (end - start) + 0.1f;
		if(Application.isPlaying){
			if(currentOrder == null){
				fadeIn = LeanTween.value(0, 1, 0.5f).setOnUpdate((float val) => {
		         	if(source != null)source.volume = val;
		        });
				source.Play();
				source.time = start;
				currentOrder = new OrderString();
				StartCoroutine(
					RepeatAudio(actualDelay, currentOrder)
				);
				fadeOut = LeanTween.value(1, 0, 0.5f).setOnUpdate((float val) => {
		         	if(source != null)source.volume = val;
				}).setDelay(delaya);
				//Debug.Log(delaya + " - " + start + " : " + end);
			}else{
				currentOrder.execute = false;
				currentOrder = new OrderString();
				fadeOut = LeanTween.value(source.volume, 0, 0.5f).setOnUpdate((float val) => {
		         	if(source != null)source.volume = val;
		        });
				StartCoroutine(
					RepeatAudio(0.6f, currentOrder)
				);
				//Debug.Log(delaya + " - " + start + " : " + end);
			}
		}
	}
	[System.Serializable]
	public class OrderString{
		public bool execute = true;
	}
	OrderString currentOrder;
	public LTDescr fadeIn;
	public LTDescr fadeOut;
	IEnumerator RepeatAudio(float delay, OrderString order){
		yield return new WaitForSeconds(delay);
		var start = GettingAudioPart(startTime);
		var end = GettingAudioPart(endTime);
		if(Application.isPlaying){
			if(order == currentOrder){
				var delaya = (end - start) - 0.5f;
				var actualDelay = (end - start) + 0.1f;
				fadeIn = LeanTween.value(0, 1, 0.5f).setOnUpdate((float val) => {
				                                                 	if(source != null) source.volume = val;
		        });
				source.Play();
				source.time = start;
				currentOrder = new OrderString();
				StartCoroutine(
					RepeatAudio(actualDelay, currentOrder)
				);
				fadeOut = LeanTween.value(1, 0, 0.5f).setOnUpdate((float val) => {
		         	if(source != null)source.volume = val;
				}).setDelay(delaya);
				//Debug.Log(delaya + " - " + start + " : " + end);
			}
		}
	}
	public float GettingAudioPart(float part){
		var a = levelInstances[levelIndex];
		return part * a.property.clip.length;
	}
	public void NextLevel(){
		levelIndex = Mathf.Clamp(levelIndex + 1, 0, levelInstances.Length - 1);
		ViewLevel();
	}
	public void PreviousLevel(){
		levelIndex = Mathf.Clamp(levelIndex - 1, 0, levelInstances.Length - 1);
		ViewLevel();
	}
	public void PlayLevel(){
		var a = levelInstances[levelIndex];
		CrossSceneLoading.LoadLevel(a.property.sceneName);
	}
	public void ChangePanel(){
		if(BottomNavigation.GetCurrentIndex() == 1){
			foreach(var a in LevelsObjects){
				a.SetActive(false);
			}
			foreach(var a in SettingsObject){
				a.SetActive(true);
			}
		}else{
			foreach(var a in LevelsObjects){
				a.SetActive(true);
			}
			foreach(var a in SettingsObject){
				a.SetActive(false);
			}
		}
	}
}
