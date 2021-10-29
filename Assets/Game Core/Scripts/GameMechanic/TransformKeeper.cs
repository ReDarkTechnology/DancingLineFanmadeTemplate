using UnityEngine;

public class TransformKeeper : MonoBehaviour
{
	GameManager core;
	public Vector3 pos;
	public Vector3 rot;
	public Vector3 scale;
    void Start()
    {
    	core = FindObjectOfType<GameManager>();
    	core.OnCheckpointObtained += SaveData;
    	core.OnCheckpointReset += ResetTransform;
    }
    public void SaveData(int temp){
    	pos = transform.position;
    	rot = transform.eulerAngles;
    	scale = transform.localScale;
    }
    public void ResetTransform(int temp){
    	transform.position = pos;
    	transform.eulerAngles = rot;
    	transform.localScale = scale;
    }
}