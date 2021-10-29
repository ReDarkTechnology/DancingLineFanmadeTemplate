using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineMovement : MonoBehaviour {
	//------Shown in inspector------
	/// <summary>
	/// Leave empty in the inspector if you want (must have GameManager in scene)
	/// </summary>
	public GameManager manager;
	/// <summary>
	/// Is the game started?
	/// </summary>
	[Header("Auto Defined")]
	public bool isStarted;
	/// <summary>
	/// Is the player controllable?
	/// </summary>
	public bool isControllable = true;
	/// <summary>
	/// Is the player alive (not dead)?
	/// </summary>
	public bool isAlive = true;
	/// <summary>
	/// Is the game finished?
	/// </summary>
	public bool isFinished;
	/// <summary>
	/// If ticked, there's an additional cube every tap
	/// </summary>
	[Header("Important Variables")]
	public bool hasCenterTail;
	/// <summary>
	/// For autoplay purposes
	/// </summary>
	public bool hasAutoplayTriggers;
	/// <summary>
	/// The tail scale depends on the TailType value on GameManager
	/// </summary>
	public bool tailScaleAsPrefferedType;
	/// <summary>
	/// You need to start with a button on a function call StartGame()
	/// </summary>
	public bool noStart;
	/// <summary>
	/// Adjust a cube ground every time the player hits a box collider
	/// </summary>
	public bool adjustGround;
	/// <summary>
	/// Turning rotations
	/// </summary>
	public Vector3 turnBlock1 = new Vector3(0, 90, 0), turnBlock2, finishBlock = new Vector3(0, 45, 0);
	/// <summary>
	/// Player won't spawn tails at objects with these tags
	/// </summary>
	public string[] deadlyTags = {
		"Obstacle"
	};
	/// <summary>
	/// A prefab that'll be instantiated when the player started to touch the floor after flying/floating
	/// </summary>
	public ParticleSystem particeFall;
	/// <summary>
	/// For managing purposes
	/// </summary>
	[Header("Parents")]
	public Transform longTailParents;
	public Transform centerTailParents;
	public Transform autoPlayParents;
	//Hidden in Inspector
	[HideInInspector] public int loopCount = 1;
	[HideInInspector] public GameObject currentTail;
	[HideInInspector] public GameObject anotherTail;
	[HideInInspector] public GameObject miscTail;
	[HideInInspector] public GameObject spawnedTail;
	[HideInInspector] public RaycastHit hit;
	[HideInInspector] public Vector3 pos;
	[HideInInspector] public Quaternion rot;
	[HideInInspector] public GameInput theInputData;
	[HideInInspector] public SkinType skinType;
	[HideInInspector] public Mesh skinMesh;
	[HideInInspector] public GameObject decoration;
	[HideInInspector] public List<MeshRenderer> spawnedObjects;
	[HideInInspector] public Color previousColor;
	[HideInInspector] public MeshRenderer m_renderer;
	[HideInInspector] public BetterCamera cam;
	//Private variables
	bool onGrounded, makeBlock, hasNoGrounded = false, stopGoing;
	bool onceSpawn;
	bool allowedToSpawn = true;
	// Use this for initialization
	void Start () {
		cam = FindObjectOfType<BetterCamera>();
		m_renderer = GetComponent<MeshRenderer>();
		previousColor = m_renderer.material.color;
		hasNoGrounded = false;
		if (FindObjectOfType<GameInput> () != null) {
			theInputData = FindObjectOfType<GameInput> ();
		} else {
			Debug.LogError ("There's no GameInput found in the scene.");
		}
		if (manager == null) {
			if (FindObjectOfType<GameManager> () != null) {
				manager = FindObjectOfType<GameManager> ();
			} else {
				Debug.LogError ("There's no GameManager found in the scene.");
			}
		}
		skinType = manager.skinType;
		if(skinType.skinMesh != null){
			skinMesh = skinType.skinMesh;
			GetComponent<MeshFilter>().mesh = skinMesh;
		}
		if(skinType.additionalDecoration != null){
			decoration = Instantiate(skinType.additionalDecoration, this.transform);
		}
		manager.OnCheckpointReset += Revive;
	}
	
	// Update is called once per frame
	void Update () {
		onGrounded = isGrounded ();
		pos = transform.position;
		rot = transform.rotation;
		if (makeBlock) {
			SpawnTail (manager.tailType);
			makeBlock = false;
		}
		if (isStarted) {
			if(!stopGoing){
				transform.Translate(Vector3.forward * Time.deltaTime * manager.lineSpeed);
				if (onGrounded && hit.collider.tag != "Obstacle" && hit.collider.tag != "Trigger") {
					if(hasNoGrounded){
						SpawnParticleFall();
						SpawnTail(manager.tailType);
						hasNoGrounded = false;
					}
					if (currentTail != null)
					{
						currentTail.transform.Translate(Vector3.forward * Time.deltaTime * 0.5f * manager.lineSpeed);
						currentTail.transform.localScale += new Vector3(0, 0, Time.deltaTime * 1f * manager.lineSpeed);
						currentTail.tag = manager.longTailTag;
					}
					if(anotherTail != null){
						anotherTail.transform.Translate(Vector3.forward * Time.deltaTime * 0.5f * manager.lineSpeed);
						anotherTail.transform.localScale += new Vector3(0, 0, Time.deltaTime * 1f * manager.lineSpeed);
						anotherTail.tag = manager.longTailTag;
					}
					if(skinType.appearEveryFrame.enable){
						if(allowedToSpawn){
							miscTail = Instantiate(skinType.appearEveryFrame.instance, transform.position, transform.rotation);
							if(!tailScaleAsPrefferedType){
								miscTail.transform.localScale = transform.localScale;
							}
							var tailRend = miscTail.GetComponent<MeshRenderer>();
							if(tailRend != null){
								tailRend.material = m_renderer.material;
							}
							miscTail.transform.position = pos + (transform.forward);
							miscTail.transform.rotation = transform.rotation;
							if(longTailParents != null){
								miscTail.transform.SetParent(longTailParents);
							}
							// disable once CompareOfFloatsByEqualityOperator
							if(skinType.appearEveryFrame.spawnDelay != 0){
								Invoke("ReturnSpawnFrame", skinType.appearEveryFrame.spawnDelay);
								allowedToSpawn = false;
							}
							spawnedObjects.Add(tailRend);
						}
					}
				}else{
					anotherTail = null;
					currentTail = null;
					hasNoGrounded = true;
				}
				if(skinType.followingObject.enable){
					if(!onceSpawn){
						spawnedTail = Instantiate(skinType.followingObject.theObject, transform.position, transform.rotation);
						if(!tailScaleAsPrefferedType){
							spawnedTail.transform.localScale = transform.localScale;
						}
						var tailrend = spawnedTail.GetComponent<MeshRenderer>();
						if(tailrend != null){
							tailrend.material = m_renderer.material;
						}
						spawnedObjects.Add(tailrend);
						spawnedTail.transform.position = pos + (transform.forward);
						spawnedTail.transform.rotation = transform.rotation;
						if(longTailParents != null){
							spawnedTail.transform.SetParent(longTailParents);
						}
						onceSpawn = true;
					}
					spawnedTail.transform.position = Vector3.Slerp(currentTail.transform.position, transform.position, 25 * Time.deltaTime);
				}
			}
		}
		if(isAlive){
			foreach (KeyCode key in theInputData.tapKeys) {
				if(isControllable){
					if (!isStarted && !EventSystem.current.IsPointerOverGameObject() && !noStart) {
						if (Input.GetKeyDown (key)) {
							StartGame (1);
						}
					}
					if (isStarted) {
						if(!manager.isPaused){
							if (isGrounded ()) {
								if (Input.GetKeyDown (key)) {
									TurnBlock ();
								}
							}	
						}else{
							if (Input.GetKeyDown (key)) {
								loopCount--;
								manager.UnPause();
								TurnBlock ();
							}
						}
					}
				}
			}
		}
		if(m_renderer.material.color != previousColor){
			previousColor = m_renderer.material.color;
			foreach(MeshRenderer obj in spawnedObjects){
				if(obj != null){
					obj.material.color = previousColor;
				}
			}
		}
	}
	private ParticleSystem system;
	public void SpawnParticleFall () {
		if(particeFall != null){
			if(system == null){
				system = Instantiate(particeFall);
			}
			system.transform.position = transform.position;
			system.Play();
		}
	}
	public void SpawnTail(TailType type){
		if(!skinType.disableDefaultTail){
			currentTail = GameObject.CreatePrimitive (type.primitiveType);
			if(tailScaleAsPrefferedType){
				currentTail.transform.localScale = type.defaultScale;
			}else{
				currentTail.transform.localScale = transform.localScale;
			}
			var coll = currentTail.GetComponent<BoxCollider> ();
			coll.size = type.colliderSize;
			coll.isTrigger = type.colliderIsTrigger;
			var mesh = currentTail.GetComponent<MeshRenderer>();
			mesh.material = m_renderer.material;
			spawnedObjects.Add(mesh);
			currentTail.transform.position = pos + (transform.forward * type.spawnZOffset);
			currentTail.transform.rotation = transform.rotation;
			if (type.addRigidbody) {
				var rigid = currentTail.AddComponent<Rigidbody> ();
				rigid.constraints = type.constraintsAsObject.constraints;
				rigid.mass = type.rigidbodyMass;
			}
			if(longTailParents != null){
				currentTail.transform.SetParent(longTailParents);
				currentTail.layer = longTailParents.gameObject.layer;
			}
		}
		if(skinType.appearEveryTap.enable){
			anotherTail = Instantiate(skinType.appearEveryTap.instance, transform.position, transform.rotation);
			if(!tailScaleAsPrefferedType){
				anotherTail.transform.localScale = transform.localScale;
			}
			var rend = anotherTail.GetComponent<MeshRenderer>();
			if(rend != null){
				rend.material = m_renderer.material;
			}
			spawnedObjects.Add(rend);
			anotherTail.transform.position = pos + (transform.forward);
			anotherTail.transform.rotation = transform.rotation;
			if(longTailParents != null){
				anotherTail.transform.SetParent(longTailParents);
				anotherTail.layer = longTailParents.gameObject.layer;
			}
		}
		if(hasCenterTail){
			GameObject centerTail = GameObject.CreatePrimitive (PrimitiveType.Cube);
			centerTail.transform.localScale = transform.localScale;
			centerTail.GetComponent<BoxCollider> ().isTrigger = true;
			var mesh = centerTail.GetComponent<MeshRenderer>();
			mesh.material.color = m_renderer.material.color;
			spawnedObjects.Add(mesh);
			centerTail.transform.position = pos;
			centerTail.transform.rotation = transform.rotation;
			centerTail.tag = manager.centerTailTag;
			if(centerTailParents != null){
				centerTail.transform.SetParent(centerTailParents);
			}
		}
	}
	public bool isGrounded()
	{
		return Physics.Raycast(transform.position, -transform.up, out hit, transform.localScale.y * 0.5f + 0.03f);
	}
	public void ReturnSpawnFrame(){
		allowedToSpawn = true;
	}
	public void TurnPlayer(Vector3 to, bool addLoopCount = false){
		//manager.AddClass("Turn1", pos + (transform.forward * 0.9f));
		transform.eulerAngles = to;
		if(addLoopCount){
			loopCount++;
		}
		makeBlock = true;
	}
	public void TurnBlock(){
		if (loopCount % 2 != 0)
		{
			if(hasAutoplayTriggers){
				GameObject autoTrigger = GameObject.CreatePrimitive (PrimitiveType.Cube);
				autoTrigger.GetComponent<BoxCollider> ().isTrigger = true;
				var mesh = autoTrigger.GetComponent<MeshRenderer>();
				mesh.enabled = false;
				autoTrigger.transform.position = pos + transform.forward;
				autoTrigger.transform.rotation = transform.rotation;
				autoTrigger.gameObject.name = "Turn1";
				spawnedObjects.Add(mesh);
				//autoTrigger.tag = "Turn1";
				if(autoPlayParents != null){
					autoTrigger.transform.SetParent(autoPlayParents);
				}
			}else{
				//manager.AddClass("Turn1", pos + (transform.forward * 0.9f));
				transform.eulerAngles = turnBlock1;
				loopCount++;
			}
		}
		else
		{
			if(hasAutoplayTriggers){
				GameObject autoTrigger = GameObject.CreatePrimitive (PrimitiveType.Cube);
				autoTrigger.GetComponent<BoxCollider> ().isTrigger = true;
				var mesh = autoTrigger.GetComponent<MeshRenderer>();
				mesh.enabled = false;
				autoTrigger.transform.position = pos + transform.forward;
				autoTrigger.transform.rotation = transform.rotation;
				autoTrigger.gameObject.name = "Turn2";
				spawnedObjects.Add(mesh);
				//autoTrigger.tag = "Turn2";
				if(autoPlayParents != null){
					autoTrigger.transform.SetParent(autoPlayParents);
				}
			}else{
				//manager.AddClass("Turn2", pos + (transform.forward * 0.9f));
				transform.eulerAngles = turnBlock2;
				loopCount++;
			}
		}
		manager.PlayerTap(loopCount, transform.eulerAngles);
		makeBlock = true;
	}
	public void StartGame(int additionalTurn = 0){
		additionalTurn = 2;
		if (manager.isAudioReady) {
			if (!isStarted) {
				manager.PlayAudio ();
				isStarted = true;
				makeBlock = true;
				loopCount += additionalTurn;
				if(cam != null){
					cam.enabled = true;
				}
				manager.PlayerTap(loopCount, transform.eulerAngles);
			}
		}
	}
	public void GameOver(bool isWinning, bool isFloating){
		if(!isWinning){
			if(!isFloating){
				#region Spawn Cubes When Dying
				//DESTRUCTION
				GameObject BrokeCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		        BrokeCube.transform.position = pos + new Vector3(1,0,0);
		        BrokeCube.transform.rotation = transform.rotation;
		        var rend0 = BrokeCube.GetComponent<MeshRenderer>();
		        rend0.material = m_renderer.material;
		        BrokeCube.AddComponent<Rigidbody>().velocity = Random.onUnitSphere * Random.Range(1, 50);
		        BrokeCube.tag = manager.longTailTag;
		        GameObject BrokeCube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		        BrokeCube1.transform.position = pos + new Vector3(0, 0, -1);
		        BrokeCube1.transform.rotation = transform.rotation;
		        var rend1 = BrokeCube1.GetComponent<MeshRenderer>();
		        rend1.material = m_renderer.material;
		        BrokeCube1.AddComponent<Rigidbody>().velocity = Random.onUnitSphere * Random.Range(1, 50);
		        BrokeCube1.tag = manager.longTailTag;
		        GameObject BrokeCube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		        BrokeCube2.transform.position = pos + new Vector3(0, 1, 0);
		        BrokeCube2.transform.rotation = transform.rotation;
		        var rend2 = BrokeCube2.GetComponent<MeshRenderer>();
		        rend2.material = m_renderer.material;
		        BrokeCube2.AddComponent<Rigidbody>().velocity = Random.onUnitSphere * Random.Range(1, 50);
		        BrokeCube2.tag = manager.longTailTag;
		        spawnedObjects.Add(rend0);
		        spawnedObjects.Add(rend1);
		        spawnedObjects.Add(rend2);
				#endregion
				stopGoing = true;
			}else{
				stopGoing = false;
			}
			isControllable = false;
			isAlive = false;
			manager.ShowDiePanel(false);
		}else{
			transform.eulerAngles = finishBlock;
			makeBlock = true;
			isFinished = true;
			isControllable = false;
			manager.ShowDiePanel(true);
		}
	}
	public void OnCollisionEnter(Collision col)
    {
		foreach(string taga in deadlyTags){
			if((taga == col.gameObject.tag) && isAlive && !isFinished){
				GameOver(false, false);
			}
		}
		if(adjustGround){
			if(col.gameObject.GetComponent<MeshRenderer>() != null){
				float TargetY = col.gameObject.transform.localScale.y / 2 * -1;
				col.gameObject.transform.position = new Vector3 (col.gameObject.transform.position.x,TargetY - 0.5f + transform.position.y, col.gameObject.transform.position.z);
			}
		}
	}
	public void OnTriggerEnter(Collider other){
		foreach(string tage in deadlyTags){
			if((tage == other.gameObject.tag) && isAlive && !isFinished){
				if(!isFinished)
				GameOver(false, true);
			}
		}
		if(other.tag == "Finish"){
			GameOver(true, false);
		}
		if(other.gameObject.name.Contains("Turn1")){
			transform.eulerAngles = turnBlock1;
			loopCount++;
			makeBlock = true;
		}
		if(other.gameObject.name.Contains("Turn2")){
			transform.eulerAngles = turnBlock2;
			loopCount++;
			makeBlock = true;
		}
	}
	public void Revive(int check){
		stopGoing = false;
		isControllable = true;
		isAlive = true;
		isStarted = false;
		DestroySpawnedObjects();
	}
	public void DestroyWithTag(string tag){
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
		foreach(GameObject ae in objects){
			Destroy(ae);
		}
	}
	public void DestroySpawnedObjects(){
		foreach(MeshRenderer obj in spawnedObjects){
			if(obj != null){
				Destroy(obj.gameObject);
			}
		}
		spawnedObjects.Clear();
	}
}
