using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAtDistance : MonoBehaviour
{
    public Vector3 CubePosition, CubeRotation, CubeScale, CubeOffset = new Vector3 (0, -50, 0);
    public LeanTweenType easeType;
    public float distFromPlayer = 15, speed = 0.2f;
    public string tagTo = "Way";
    GameObject TheSpawnedObject;
    GameManager core;
    int CheckpointSort;
    LineMovement mov;
    public bool withZScale, thePositionRotationAndScaleSameWithThisObject = true,disableMeshWhenPlaying=true;
    bool animate,hasBeenResetted,hasAnimate;
    float zStuff;
    // Use this for initialization
    void Start () {
        core = FindObjectOfType<GameManager>();
        CheckpointSort = core.CheckpointGot;
        core.OnCheckpointReset += ResetObject;
        if (disableMeshWhenPlaying)
        {
            GetComponent<MeshRenderer>().enabled = false;
            MeshRenderer[] meshes = transform.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer ya in meshes){
            	ya.enabled = false;
            }
        }
        if (thePositionRotationAndScaleSameWithThisObject) {
        	TheSpawnedObject = Instantiate(this.gameObject, transform.parent);
        	Destroy(TheSpawnedObject.GetComponent<BoxCollider>());
        	Destroy(TheSpawnedObject.GetComponent<AppearAtDistance>());
        	if(GetComponent<Light>() != null){
        		Destroy(TheSpawnedObject.GetComponent<Light>());
        	}
        	TheSpawnedObject.GetComponent<MeshRenderer> ().enabled = true;
            TheSpawnedObject.transform.position = transform.position + CubeOffset;
            TheSpawnedObject.transform.eulerAngles = transform.eulerAngles;
            TheSpawnedObject.transform.localScale = transform.localScale;
            TheSpawnedObject.GetComponent<MeshRenderer> ().material = GetComponent<MeshRenderer> ().material;
            if(transform.parent != null){
            	TheSpawnedObject.transform.SetParent(this.transform.parent);
            }
            TheSpawnedObject.tag = this.gameObject.tag;
        } else {
            TheSpawnedObject = GameObject.CreatePrimitive (PrimitiveType.Cube);
            TheSpawnedObject.transform.position = CubePosition + CubeOffset;
            TheSpawnedObject.transform.eulerAngles = CubeRotation;
            TheSpawnedObject.transform.localScale = CubeScale;
            if(transform.parent != null){
            	TheSpawnedObject.transform.SetParent(this.transform.parent);
            }
            TheSpawnedObject.GetComponent<MeshRenderer> ().material = GetComponent<MeshRenderer> ().material;
            if (tagTo != "")
            {
                TheSpawnedObject.tag = tagTo;
            }
        }
        mov = FindObjectOfType<LineMovement> ();
    }

    // Update is called once per frame
    void Update () {
        float dist = Vector3.Distance (mov.gameObject.transform.position, transform.position);
        float minimumDistance = distFromPlayer;
        if(withZScale){
        	minimumDistance = distFromPlayer * transform.localScale.z / 0.5f;
        }
    	if (dist < minimumDistance)
    	{
    	    animate = true;
    	    if (!hasAnimate)
    	    {
    	    	TheSpawnedObject.LeanMoveLocal (transform.localPosition, speed).setEase(easeType);
    	        hasAnimate = true;
    	        CheckpointSort = core.CheckpointGot;
    	    }
    	}
        if (animate) {
            //TheSpawnedObject.transform.position = Vector3.Lerp (TheSpawnedObject.transform.position, transform.position, speed * Time.deltaTime);
        }
    }
    public void ResetObject(int ha)
    {
        if (CheckpointSort == ha)
        {
            TheSpawnedObject.transform.position = transform.position + CubeOffset;
            hasAnimate = false;
        }
    }
    void OnDrawGizmosSelected(){
    	Gizmos.color = Color.red;
    	if(!Application.isPlaying){
    		Gizmos.DrawLine(transform.position, transform.position + CubeOffset);
    	}else{
    		Gizmos.DrawLine(transform.position, TheSpawnedObject.transform.position);
    	}
    	if(withZScale){
    		zStuff = transform.localScale.z;
    	}
    	Gizmos.color = Color.yellow;
    	if(withZScale){
    		Gizmos.DrawWireSphere(transform.position, distFromPlayer * transform.localScale.z / 0.5f);
    	}else{
    		Gizmos.DrawWireSphere(transform.position, distFromPlayer);
    	}
    }
}
