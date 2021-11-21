using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	
	//Level Variables
	public LevelProperty property;
	[HideInInspector]public float levelPercentage;
	[HideInInspector]public int gemsObtained;
	[HideInInspector]public int CheckpointGot;
	[HideInInspector]public int CurrentCheckpoint;
	[HideInInspector]public bool isPlayerDie;
	[HideInInspector]public bool isGameFinished;
	[HideInInspector]public Gems[] gemsInScene;
	
	//UI Variables
	public Slider audioProgress;
	public Text percentageProgress;
	public Text gemsCount;
	public Text levelNameView;
	public CanvasGroup diePanel;
	public Transform checkpointParents;
	public GameObject DiePart, CheckpointPart;
	public CanvasGroup WhitePanel;
	[HideInInspector]public bool isOnCheckpointPanel;
	[HideInInspector]public int whichPanel;
	[HideInInspector]public float timePanel;
	[HideInInspector]public CanvasGroup[] checkpointsCrowns;
	
	//Audio Variables
	public enum AudioUsage
	{
		AsAudioClip,
		AsPath,
		NoClip
	}
	public AudioUsage audioLoadType;
	public AudioClip audioClip;
	public string audioPath;
	public float AudioOffset;
	public bool customEndDuration;
	public float EndDuration;
	[HideInInspector]
	public bool isAudioReady;
	public AudioSource audioSource;
	
	//Line Variables
	public SkinType skinType;
	public TailType tailType;
	public float lineSpeed = 12;
	public string longTailTag = "Tail", centerTailTag = "Center";
	
    //Actions
    public event Action<int, Vector3> OnPlayerTap;
    public event Action OnGameFinished;
    public event Action OnGameStarted;
    public event Action OnPlayerDies;
    public event Action<int> OnCheckpointObtained;
    public event Action<int> OnPlayerDiesInCheckpoint;
    public event Action<int> OnCheckpointReset;
    
    //Realtime Variables
    public bool isPaused;
    [HideInInspector]
	public bool isStarted;
    [HideInInspector]
	public bool isFinished;
    
    //Singleton
    public static GameManager mgr;
    
    //Instances
    public LineMovement[] mov;
    
    void Awake (){
    	mgr = this;
    }
    
    public static GameManager GetGameManager(){
    	if(GameManager.mgr == null){
    		GameManager.mgr = FindObjectOfType<GameManager>();
    	}
    	return GameManager.mgr;
    }
    
	// Use this for initialization
	private GameObject[] triggerHide;
	private float previousProgress;
	private int previousCrowns;
	private int previousGems;
	
	[HideInInspector]
	public GameInput theInputData;
	
	void Start () {
		previousProgress = float.Parse(Configuration.GetString("Progress-"+property.LevelID, "0"));
		previousCrowns = int.Parse(Configuration.GetString("Crown-"+property.LevelID, "0"));
		previousGems = int.Parse(Configuration.GetString("Gems-"+property.LevelID, "0"));
		if(mov.Length < 1){
			mov = FindObjectsOfType<LineMovement>();
		}
		Time.timeScale = 1;
		diePanel.alpha = 0;
		diePanel.gameObject.SetActive(false);
		triggerHide = GameObject.FindGameObjectsWithTag("Trigger");
		if (FindObjectOfType<GameInput> () != null) {
			theInputData = FindObjectOfType<GameInput> ();
		} else {
			Debug.LogError ("There's no GameInput found in the scene.");
		}
		audioSource = GetComponent<AudioSource> ();
		if (audioLoadType == AudioUsage.AsPath) {
			//Starting to import the audio
			StartCoroutine (importAudio ());
		}
		if(audioLoadType == AudioUsage.AsAudioClip){
			if(audioClip != null){
				//Audio clip doesn't need to be imported and it's ready
				isAudioReady = true;
			}else{
				if(property.clip == null){
					Debug.LogError("AudioClip of GameManager has not been assigned. Please assign an AudioClip in order to play the game");
				}else{
					audioClip = property.clip;
				}
			}
		}
		if (audioClip == null && !File.Exists(audioPath)) audioLoadType = AudioUsage.NoClip;
		if(audioLoadType == AudioUsage.NoClip){
			Debug.LogError("No audio was set");
		}
		gemsInScene = FindObjectsOfType<Gems>();
		foreach(GameObject hey in triggerHide){
			if(hey.GetComponent<MeshRenderer>() != null){
				hey.GetComponent<MeshRenderer>().enabled = false;
			}
		}
		checkpointsCrowns = checkpointParents.GetComponentsInChildren<CanvasGroup>();
	}
	
	// Update is called once per frame
	public void UnPause(){
		Time.timeScale = 1;
		audioSource.Play();
		isPaused = false;
	}
	void Update () {
		levelNameView.text = property.LevelName;
		if (!customEndDuration && audioSource.clip != null) {
			EndDuration = audioSource.clip.length;
		}
		//If the restartKey pressed, the scene will be reloaded
		if(Input.GetKeyDown(theInputData.restartKey)){
			RestartScene();
		}
		if(isStarted && !isPlayerDie){
			if(Input.GetKeyDown(theInputData.pauseKey)){
				if(!isPaused){
					Time.timeScale = 0;
					audioSource.Pause();
					isPaused = true;
				}else{
					UnPause();
					RestartScene();
				}
			}
		}
		//Set values and apply it to the UIi
		if(!isGameFinished){
			if(audioSource.clip != null){
				levelPercentage = audioSource.time / EndDuration;
			}
		}else{
			if(audioSource.clip != null){
				levelPercentage = 1;
			}
		}
		if(isPlayerDie){
			diePanel.gameObject.SetActive(true);
			if(diePanel.alpha < 1){
				diePanel.alpha += Time.deltaTime;
			}
			if(!isGameFinished){
				if(audioSource.volume > 0){
					audioSource.volume -= Time.deltaTime;
				}else{
					audioSource.volume = 0;
					audioSource.Pause();
				}
			}
			if(!isOnCheckpointPanel){
				if(CurrentCheckpoint > 0){
					if(whichPanel < CheckpointGot){
						checkpointsCrowns[whichPanel].alpha = 1;
						whichPanel++;
					}
					if(CheckpointGot == 0){
						foreach(CanvasGroup gr in checkpointsCrowns){
							gr.alpha = 0;
						}
					}
				}
			}
		}else{
			if(diePanel.alpha > 0){
				diePanel.alpha -= Time.deltaTime;
			}else{
				diePanel.gameObject.SetActive(false);
			}
			if(audioLoadType != AudioUsage.NoClip){
				if(audioSource.volume < 1){
					audioSource.volume += Time.deltaTime;
				}else{
					audioSource.volume = 1;
				}
			}
		}
		audioProgress.value = levelPercentage;
		percentageProgress.text = (levelPercentage * 100).ToString("0") + "%";
		gemsCount.text = gemsObtained.ToString() + "/" + gemsInScene.Length.ToString();
	}
	public void PlayerTap(int i, Vector3 v){
		OnPlayerTap.Invoke(i, v);
	}
	public void RestartScene(){
		CrossSceneLoading.LoadLevel(buildIndex());
	}
	public void AddGemsCount(int howMuch){
		gemsObtained += howMuch;
	}
	public void LoadSceneAsIndex(int index){
		CrossSceneLoading.LoadLevel(index);
	}
	public void LoadSceneAsName(string name){
		CrossSceneLoading.LoadLevel(name);
	}
	public int buildIndex(){
		return SceneManager.GetActiveScene().buildIndex;
	}
	IEnumerator importAudio(){
		if(File.Exists(audioPath)){
			#if UNITY_2017_1_OR_NEWER
			using (var download = new UnityEngine.Networking.UnityWebRequest(audioPath)){
				yield return download.SendWebRequest();
				var theClip = ((UnityEngine.Networking.DownloadHandlerAudioClip) download.downloadHandler).audioClip;
				if (theClip != null) {
					theClip.name = Path.GetFileNameWithoutExtension (audioPath);
					audioClip = theClip;
					isAudioReady = true;
				} else {
					Debug.LogError ("The audio file can't be loaded or isn't supported.");
				}
			}
			#else
			using (var download = new WWW (audioPath)) {
				yield return download;
				var theClip = download.GetAudioClip (false, true);
				if (theClip != null) {
					theClip.name = Path.GetFileNameWithoutExtension (audioPath);
					audioClip = theClip;
					isAudioReady = true;
				} else {
					Debug.LogError ("The audio file can't be loaded or isn't supported.");
				}
			}
			#endif
		}else{
			Debug.LogError("The file does not exist");
		}
	}
	
	public void PlayGame ()
	{
		isStarted = true;
		PlayAudio();
		foreach(var line in mov) {
			line.loopCount++;
			line.makeBlock = true;
		}
	}
	
	public void PlayAudio ()
	{
		if(audioLoadType != AudioUsage.NoClip){
			audioSource.clip = audioClip;
            // disable once CompareOfFloatsByEqualityOperator
            audioSource.Play();
            if (audioSource.time == 0)
            {
                if (AudioOffset < 0)
                {
                    audioSource.time = -AudioOffset;
                }
                else
                {
                    audioSource.PlayDelayed(AudioOffset);
                }
            }
		}
		if(OnGameStarted != null) OnGameStarted.Invoke();
	}
	public void ShowDiePanel(bool isWinning){
		if(CurrentCheckpoint == 0){
	        if (!isWinning)
	        {
	            OnPlayerDies.Invoke();
	            if(previousProgress < levelPercentage){
	            	Configuration.SetString("Progress-"+property.LevelID, levelPercentage.ToString());
	            }
	        }
	        else
	        {
	            OnGameFinished.Invoke();
	            Configuration.SetString("Progress-"+property.LevelID, "1");
	            if(previousCrowns < CheckpointGot)
	            	Configuration.SetString("Crown-"+property.LevelID, CheckpointGot.ToString());
	            if(previousGems < gemsObtained)
	            	Configuration.SetString("Gems-"+property.LevelID, gemsObtained.ToString());
	            //PlayerPrefs.SetString(levelProperty.LevelID + "-finish", "True");
	        }
			isPlayerDie = true;
			isGameFinished = isWinning;
			DiePart.SetActive(true);
			CheckpointPart.SetActive(false);
		}else{
			if (!isWinning)
	        {
				OnPlayerDiesInCheckpoint.Invoke(CheckpointGot);
				DiePart.SetActive(false);
				CheckpointPart.SetActive(true);
	            if(previousProgress < levelPercentage){
	            	Configuration.SetString("Progress-"+property.LevelID, levelPercentage.ToString());
	            }
	        }
	        else
	        {
	            OnGameFinished.Invoke();
				DiePart.SetActive(true);
				CheckpointPart.SetActive(false);
				isOnCheckpointPanel = false;
	            Configuration.SetString("Progress-"+property.LevelID, "1");
	            if(previousCrowns < CheckpointGot)
	            	Configuration.SetString("Crown-"+property.LevelID, CheckpointGot.ToString());
	            if(previousGems < gemsObtained)
	            	Configuration.SetString("Gems-"+property.LevelID, gemsObtained.ToString());
	        }
			isPlayerDie = true;
			isGameFinished = isWinning;
		}
	}
	public void AddCheckpoint(bool lostCheck = false){
		if(lostCheck){
			CheckpointGot--;
		}else{
			CheckpointGot++;
			CurrentCheckpoint++;
			if(OnCheckpointObtained != null){
				OnCheckpointObtained.Invoke(CurrentCheckpoint);
			}
		}
	}
	public void ConfirmCheckpoint(bool statement){
		if(statement){
			OnCheckpointReset.Invoke(CurrentCheckpoint);
			gemsObtained = 0;
			isPlayerDie = false;
			diePanel.alpha = 0;
		}else{
			isOnCheckpointPanel = false;
			DiePart.SetActive(true);
			CheckpointPart.SetActive(false);
		}
	}
}
[Serializable]
public class LevelAchievement {
	public float progress;
	public int gems;
	public int crowns;
}
