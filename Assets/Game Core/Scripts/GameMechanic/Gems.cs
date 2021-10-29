using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gems : MonoBehaviour
{
	[HideInInspector]public GameManager manager;
	public GameObject shatterPiece;
	public ParticleSystem shatterOrb;
	bool isObtainable = true;
	public List<GameObject> shattersObject;
	MeshRenderer rend;
	public Vector3 rotationDirection = new Vector3(0, 5, 0);
    // Start is called before the first frame update
    void Start()
    {
    	rend = GetComponent<MeshRenderer>();
        if (manager == null) {
			if (FindObjectOfType<GameManager> () != null) {
				manager = FindObjectOfType<GameManager> ();
			} else {
				Debug.LogError ("There's no GameManager found in the scene.");
			}
		}
    	manager.OnCheckpointReset += UnShatter;
    }
    void Update(){
    	if(isObtainable){
    		transform.Rotate(rotationDirection * Time.deltaTime);
    	}
    }
    void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
    		if(isObtainable){
    			Shatter();
    		}
    	}
    }
    public void Shatter(){
    	PieceSpawner(shatterPiece, true);
    	PieceSpawner(shatterPiece, true);
    	PieceSpawner(shatterPiece, true);
    	PieceSpawner(shatterOrb.gameObject);
    	rend.enabled = false;
    	manager.AddGemsCount(1);
    	isObtainable = false;
    }
    public void UnShatter(int sort){
    	rend.enabled = true;
    	isObtainable = true;
    	foreach(GameObject ae in shattersObject){
    		Destroy(ae);
    	}
    	shattersObject.Clear();
    }
    public void PieceSpawner(GameObject piece, bool addRandomForce = false){
    	GameObject a = Instantiate(piece, transform.position, transform.rotation);
    	var render  = a.GetComponent<MeshRenderer>();
    	if(render != null){
    		render.material = GetComponent<MeshRenderer>().material;
    	}
    	var rigid = a.GetComponent<Rigidbody>();
    	if(rigid != null){
    		rigid.velocity = randomUnitSphereSpecified(-10, 20, -10, 30, -10, 30);
    	}
    	shattersObject.Add(a);
    }
    public Vector3 randomUnitSphereSpecified(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax){
    	float x = UnityEngine.Random.Range(xMin, xMax);
    	float y = UnityEngine.Random.Range(yMin, yMax);
    	float z = UnityEngine.Random.Range(zMin, zMax);
    	return new Vector3(x, y, z);
    }
}
