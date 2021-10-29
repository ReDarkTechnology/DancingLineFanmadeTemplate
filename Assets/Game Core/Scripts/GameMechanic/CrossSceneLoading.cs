using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrossSceneLoading : MonoBehaviour {
    public Slider slider;
    public Text progresstext;
	public bool TurnedOn;
	bool isCalled;
	public float speedFade = 3f;
	//Save Temporarily
	[HideInInspector]public bool isIndexCall;
	[HideInInspector]public bool isNameCall;
	[HideInInspector]public int currentIndex;
	[HideInInspector]public string currentName;
	[HideInInspector]public int curIndexGoing;
	[HideInInspector]public string curNameGoing;

	void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
	{
		loaded();
	}

	#region Singleton
	public static CrossSceneLoading myself;
	void Awake(){
		if(myself == null){
			myself = this;
			SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
			DontDestroyOnLoad(this.gameObject);
		}else{
			if(myself != this){
				Destroy(this.gameObject);
			}
		}
		GetComponent<CanvasGroup> ().alpha = 0;
		TurnInto(false);
	}
	#endregion
    public static void LoadLevel(int SceneIndex)
    {
    	if(myself != null){
    		myself.TurnedOn = true;
    		myself.currentIndex = SceneIndex;
    		myself.isIndexCall = true;
    		myself.curIndexGoing = SceneIndex;
    		myself.ResetUI();
    	}else{
    		Debug.LogError("No instance of CrossSceneLoading is found. Force direct call");
    		SceneManager.LoadScene(SceneIndex);
    	}
    }
    public static void LoadLevel(string SceneName)
    {
    	if(myself != null){
    		myself.TurnedOn = true;
    		myself.currentName = SceneName;
    		myself.isNameCall = true;
    		myself.curNameGoing = SceneName;
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
    			if(GetComponent<CanvasGroup>().alpha == 1){
					if(isIndexCall){
						StartCoroutine(LoadAsynchronously(currentIndex));
						isCalled = true;
					}
					if(isNameCall){			
						StartCoroutine(LoadAsynchronously(currentName));
						isCalled = true;
					}
    			}else{
					GetComponent<CanvasGroup> ().alpha += speedFade * Time.deltaTime;
    			}
			}
		} else {
			// disable once CompareOfFloatsByEqualityOperator
			if(GetComponent<CanvasGroup>().alpha == 0){
				TurnInto(false);
				isCalled = false;
			}else{
				progresstext.text = "100%";
				slider.value = 100;
				GetComponent<CanvasGroup> ().alpha -= speedFade * Time.deltaTime;
			}
		}
	}
    void TurnInto(bool to){
    	GetComponent<CanvasGroup> ().interactable = to;
    	GetComponent<CanvasGroup> ().blocksRaycasts = to;
    }
    public int buildIndex(){
		return SceneManager.GetActiveScene().buildIndex;
	}
}