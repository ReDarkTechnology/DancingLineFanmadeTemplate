using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedPositionAnima : MonoBehaviour
{
	public Vector3 CubePosition;
	public float CubeXMin = 10, CubeXMax = 30;
	public float CubeYMin = 0, CubeYMax = 30;
	public float CubeZMin = 10, CubeZMax = 30;
    public float distFromPlayer = 3, speed = 5;
    public Vector3 CubeOffset;
    public string tagTo = "Obstacle";
    GameObject TheSpawnedObject;
    GameManager core;
    int CheckpointSort;
    LineMovement mov;
    public bool withZScale, thePositionRotationAndScaleSameWithThisObject = true,disableMeshWhenPlaying=true;
    bool animate, hasAnimate;
    // Use this for initialization
    void Start () {
        mov = FindObjectOfType<LineMovement> ();
        core = FindObjectOfType<GameManager>();
        CheckpointSort = core.CheckpointGot;
    	core.OnCheckpointReset += (int obj) => {
        	if(obj == CheckpointSort){
            	ResetObject();
        	}
    	};
		CubeOffset = new Vector3(Random.Range(CubeXMin,CubeXMax),Random.Range(CubeYMin,CubeYMax),Random.Range(CubeZMin,CubeZMax));
        if (disableMeshWhenPlaying)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        if (thePositionRotationAndScaleSameWithThisObject) {
        	TheSpawnedObject = Instantiate(this.gameObject);
        	Destroy(TheSpawnedObject.GetComponent<BoxCollider>());
        	Destroy(TheSpawnedObject.GetComponent<RandomizedPositionAnima>());
        	if(GetComponent<Light>() != null){
        		Destroy(TheSpawnedObject.GetComponent<Light>());
        	}
        	TheSpawnedObject.GetComponent<MeshRenderer> ().enabled = true;
            TheSpawnedObject.transform.position = transform.position + CubeOffset;
            TheSpawnedObject.transform.eulerAngles = transform.eulerAngles;
            TheSpawnedObject.GetComponent<MeshRenderer> ().material = GetComponent<MeshRenderer> ().material;
            TheSpawnedObject.tag = this.gameObject.tag;
        } else {
            TheSpawnedObject = GameObject.CreatePrimitive (PrimitiveType.Cube);
            TheSpawnedObject.transform.position = CubePosition + CubeOffset;
            TheSpawnedObject.GetComponent<MeshRenderer> ().material = GetComponent<MeshRenderer> ().material;
            if (tagTo != "")
            {
                TheSpawnedObject.tag = tagTo;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        float dist = Vector3.Distance (mov.gameObject.transform.position, transform.position);
        if (dist < distFromPlayer)
        {
            animate = true;
            if (!hasAnimate)
            {
                hasAnimate = true;
                CheckpointSort = core.CheckpointGot;
            }
        }
        if (animate) {
            TheSpawnedObject.transform.position = Vector3.Lerp (TheSpawnedObject.transform.position, transform.position, speed * Time.deltaTime);
        }
    }
    float zStuff;
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
    		Gizmos.DrawWireSphere(transform.position, distFromPlayer *transform.localScale.z / 0.5f);
    	}else{
    		Gizmos.DrawWireSphere(transform.position, distFromPlayer);
    	}
    }
    public void ResetObject()
    {
        if (thePositionRotationAndScaleSameWithThisObject)
            TheSpawnedObject.transform.position = transform.position + CubeOffset;
        else
            TheSpawnedObject.transform.position = CubePosition + CubeOffset;
        animate = false;
    }
    
}
