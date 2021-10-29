using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPutter : MonoBehaviour
{
	public GameObject Instance;
	public Vector3 Offset = new Vector3(1,0,1);
	public KeyCode spawnKey = KeyCode.T;
	public bool isTrigger;
	public TriggerPutter targTrigPutter;
	LineMovement line;
	
	public Grouped defaultValue;
    // Start is called before the first frame update
    void Start()
    {
    	line = FindObjectOfType<LineMovement>();
    }

    // Update is called once per frame
    void Update()
    {
    	if(!isTrigger){
    		if(Input.GetKeyDown(spawnKey)){
    			GameObject h = Instantiate(Instance,transform.position + Offset,Instance.transform.rotation);
    			if(defaultValue != null){
    				h.GetComponent<TriggerIt>().grouped = defaultValue;
    			}
    		}
    	}
    }
    public void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
    		if(isTrigger){
    			targTrigPutter.Offset = Offset;
    		}
    	}
    }
}
