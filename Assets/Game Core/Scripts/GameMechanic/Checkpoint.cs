using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
	public GameObject Crown;
	public SpriteRenderer CrownRenderer;
	public ParticleSystem shatterOrb;
	public List<GameObject> shattersObject;
	public bool isObtained, isArrivedAgain;
	bool shatterArrived, hasUp;
	public float orbSpeed = 1;
	float starterDistance, nowDistance;
	public bool preferLoopCount;
	[Range(1,2)]
	public int loopCountTarget;
	//Saving Variables
	Vector3 currentPosition;
	int currentLoopCount;
	float currentAudioTime;
	int checkpointSort;
	//Unnecesarry
	bool commitUnshatter;
	
	GameManager mgr;
	LineMovement mov;
	MeshRenderer rend;
	MeshRenderer crownRend;
	void Start(){
		mgr = FindObjectOfType<GameManager>();
		mgr.OnCheckpointReset += UnShatter;
		mov = FindObjectOfType<LineMovement>();
		rend = GetComponent<MeshRenderer>();
		crownRend = Crown.GetComponent<MeshRenderer>();
	}
    void Update()
    {
    	if(isObtained){
    		if(!isArrivedAgain){
	    		if(shattersObject.Count > 0){
	    			nowDistance = Vector3.Distance(shattersObject[0].transform.position, CrownRenderer.transform.position);
	    			if(nowDistance > starterDistance / 2 && !hasUp){
	    				Vector3 ae = new Vector3(0, 2, 0) + CrownRenderer.transform.position;
	    				shattersObject[0].transform.position = Vector3.Lerp(shattersObject[0].transform.position, ae, Time.deltaTime * orbSpeed);
	    			}else{
	    				hasUp = true;
	    				shattersObject[0].transform.position = Vector3.Lerp(shattersObject[0].transform.position, CrownRenderer.transform.position, Time.deltaTime * orbSpeed);
	    			}
	    			if(!shatterArrived){
		    			if(nowDistance < CrownRenderer.transform.localScale.x){
		    				foreach(GameObject ae in shattersObject){
					    		Destroy(ae);
					    	}
					    	shattersObject.Clear();
					    	shatterArrived = true;
		    			}
	    			}
	    		}
	    		if(shatterArrived){
	    			Color yo = CrownRenderer.color;
	    			CrownRenderer.color = Color.Lerp(CrownRenderer.color, new Color(yo.r, yo.g, yo.b, 1), Time.deltaTime * 5);
	    		}
    		}else{
    			if(shattersObject.Count > 0){
    				shattersObject[0].transform.position += new Vector3(0,Time.deltaTime*orbSpeed,0);
    				Color yo = CrownRenderer.color;
	    			CrownRenderer.color = Color.Lerp(CrownRenderer.color, new Color(yo.r, yo.g, yo.b, 0), Time.deltaTime * 5);
    			}
    		}
    	}else{
    		Crown.transform.Rotate(new Vector3(0,30,0) * Time.deltaTime);
    	}
    	if(commitUnshatter && mov.isStarted){
    		ReShatter();
    		mgr.AddCheckpoint(true);
    		commitUnshatter = false;
    	}
    }
    public void PieceSpawner(GameObject piece){
    	GameObject a = Instantiate(piece, Crown.transform.position, transform.rotation);
    	var renda = a.GetComponent<MeshRenderer>();
    	if(renda != null){
    		renda.material = rend.material;
    	}
    	shattersObject.Add(a);
    }
    void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
    		if(isObtained && !isArrivedAgain){
    			commitUnshatter = true;
    		}
    		if(!isObtained){
    			Shatter();
    			checkpointSort = mgr.CurrentCheckpoint + 1;
    			currentPosition = other.transform.position;
    			currentAudioTime = mov.manager.audioSource.time;
    			if(!preferLoopCount){
    				currentLoopCount = mov.loopCount;
    			}else{
    				currentLoopCount = loopCountTarget;
    			}
    			mgr.AddCheckpoint();
    		}
    	}
    }
    public void Shatter(){
    	PieceSpawner(shatterOrb.gameObject);
    	crownRend.enabled = false;
    	starterDistance = Vector3.Distance(Crown.transform.position, CrownRenderer.transform.position);
    	isObtained = true;
    }
    public void ReShatter(){
    	crownRend.enabled = false;
    	isObtained = true;
    	isArrivedAgain = true;
    	//customSpawn
    	GameObject a = Instantiate(shatterOrb.gameObject, CrownRenderer.transform.position, transform.rotation);
    	var rendy = a.GetComponent<MeshRenderer>();
    	if(rendy != null){
    		rendy.material = rend.material;
    	}
    	shattersObject.Add(a);
    }
    public void UnShatter(int sort){
    	if(checkpointSort == sort){
    		mov.loopCount = currentLoopCount - 1;
    		mov.manager.audioSource.time = currentAudioTime;
    		mov.transform.position = currentPosition;
    		isObtained = true;
    		foreach(GameObject ae in shattersObject){
    			Destroy(ae);
    		}
    		shattersObject.Clear();
    	}
    }
}
