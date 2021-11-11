using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrossSceneLoading : MonoBehaviour {
	public Image image;
	private Color m_backColor = Color.black;
	private Color m_foreColor = Color.white;
	private Color backColor { get {return m_backColor;} 
		set {
			Color neutral = new Color(value.r, value.g, value.b, 1f);
			if(image != null) image.color = neutral; 
			m_backColor = neutral;
		}
	}
	private Color foreColor { 
		get {return m_foreColor;} 
		set {
			foreach (var graphic in foreGraphics)
			{
				graphic.color = value;
			}
			if(slider != null) slider.transform.Find ("Background").GetComponent<Image>().color = new Color(value.r, value.g, value.b, 0.5f);
			m_foreColor = value;
		}
	}
    public Slider slider;
    public Text progresstext;
    
    public Graphic[] foreGraphics;
    
	public bool TurnedOn;
	public float speedFade = 3f;
	
	//Save Temporarily
	[HideInInspector] public bool isIndexCall;
	[HideInInspector] public bool isNameCall;
	[HideInInspector] public int currentIndex;
	[HideInInspector] public string currentName;
	[HideInInspector] public int curIndexGoing;
	[HideInInspector] public string curNameGoing;
	[HideInInspector] public bool isCalled;
	[HideInInspector] public CanvasGroup canGroup;

	void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
	{
		loaded();
	}

	#region Singleton
	public static CrossSceneLoading myself;
	void Start(){
		if(myself == null){
			myself = this;
			SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
			DontDestroyOnLoad(this.gameObject);
		}else{
			if(myself != this){
				Destroy(this.gameObject);
			}
		}
		canGroup = GetComponent<CanvasGroup> ();
		canGroup.alpha = 0;
		TurnInto(false);
	}
	#endregion
	public static void LoadLevel(int SceneIndex, Color backColor = default(Color), Color foreColor = default(Color))
    {
    	if(myself != null){
    		myself.TurnedOn = true;
    		myself.currentIndex = SceneIndex;
    		myself.isIndexCall = true;
    		myself.curIndexGoing = SceneIndex;
    		if(backColor != default(Color)){
    			myself.backColor = backColor;
    		}
    		if(foreColor != default(Color)){
    			myself.foreColor = foreColor;
    		}
    		myself.ResetUI();
    	}else{
    		Debug.LogError("No instance of CrossSceneLoading is found. Force direct call");
    		SceneManager.LoadScene(SceneIndex);
    	}
    }
    public static void LoadLevel(string SceneName, Color backColor = default(Color), Color foreColor = default(Color))
    {
    	if(myself != null){
    		myself.TurnedOn = true;
    		myself.currentName = SceneName;
    		myself.isNameCall = true;
    		myself.curNameGoing = SceneName;
    		if(backColor != default(Color)){
    			myself.backColor = backColor;
    		}
    		if(foreColor != default(Color)){
    			myself.foreColor = foreColor;
    		}
    		myself.ResetUI();
    	}else{
    		Debug.LogError("No instance of CrossSceneLoading is found. Force direct call");
    		SceneManager.LoadSceneAsync(SceneName);
    	}
    }
    IEnumerator LoadAsynchronously(int SceneIndex)
    {
		AsyncOperation async = SceneManager.LoadSceneAsync (SceneIndex);
		async.allowSceneActivation = false;
		while (!async.isDone) {
			slider.value = async.progress;
			string count = (async.progress * 100).ToString("0") + "%";
			progresstext.text = count;
			if (async.progress >= 0.9f){
				slider.value = 1;
				progresstext.text = "100%";
				async.allowSceneActivation = true;
			}
			yield return new WaitForSeconds (1);
			yield return null;
		}
		TurnInto(true);
    }
    IEnumerator LoadAsynchronously(string SceneName)
    {
		AsyncOperation async = SceneManager.LoadSceneAsync (SceneName);
		async.allowSceneActivation = false;
		while (!async.isDone) {
			slider.value = async.progress;
			string count = (async.progress * 100).ToString("0") + "%";
			progresstext.text = count;
			if (async.progress >= 0.9f){
				slider.value = 1;
				progresstext.text = "100%";
				async.allowSceneActivation = true;
			}
			yield return new WaitForSeconds (1);
			//loaded();
			yield return null;
		}
		TurnInto(true);
    }
    public void ResetUI(){
    	slider.value = 0;
		progresstext.text = "0%";
    }
    public void loaded()
    {
		TurnedOn = false;
		isNameCall = false;
		isIndexCall = false;
    }
	void Update(){
		// disable once CompareOfFloatsByEqualityOperator
		if (TurnedOn) {
			if(!isCalled){
    			if(canGroup.alpha == 1){
					if(isIndexCall){
						StartCoroutine(LoadAsynchronously(currentIndex));
						isCalled = true;
					}
					if(isNameCall){			
						StartCoroutine(LoadAsynchronously(currentName));
						isCalled = true;
					}
    			}else{
					canGroup.alpha += speedFade * Time.deltaTime;
    			}
			}
		} else {
			// disable once CompareOfFloatsByEqualityOperator
			if(canGroup.alpha == 0){
				TurnInto(false);
				isCalled = false;
			}else{
				progresstext.text = "100%";
				slider.value = 100;
				canGroup.alpha -= speedFade * Time.deltaTime;
			}
		}
	}
    void TurnInto(bool to){
    	canGroup.interactable = to;
    	canGroup.blocksRaycasts = to;
    }
    void OnDestroy(){
    	SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }
    public int buildIndex(){
		return SceneManager.GetActiveScene().buildIndex;
	}
}