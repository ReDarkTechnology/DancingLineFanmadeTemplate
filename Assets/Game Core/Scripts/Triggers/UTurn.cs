using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTurn : MonoBehaviour
{
	public LineMovement target;
	public Vector3 turnBlock1 = new Vector3(0, 90, 0), turnBlock2, finishBlock = new Vector3(0, 45, 0);
    //Checkpoint
    [HideInInspector] public GameManager core;
    [HideInInspector] public static int CheckpointSort;
    [HideInInspector] public static Vector3 turn1, turn2;
    void Start(){
        core = FindObjectOfType<GameManager>();
        core.OnCheckpointReset += ResetTurn;
    }
    public void ResetTurn(int to){
        if(CheckpointSort == to){
            target.turnBlock1 = turn1;
            target.turnBlock2 = turn2;
        }
    }
    public void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
            //CheckpointPurposes
            if(core.CheckpointGot > CheckpointSort){
                CheckpointSort = core.CheckpointGot;
                turn1 = target.turnBlock1;
                turn2 = target.turnBlock2;
            }
            //Actual Code
			target.turnBlock1 = turnBlock1;
            target.turnBlock2 = turnBlock2;
    	}
    }
}
