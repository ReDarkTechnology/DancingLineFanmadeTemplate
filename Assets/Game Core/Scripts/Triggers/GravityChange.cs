using System;
using UnityEngine;

public class GravityChange : MonoBehaviour
{
	/// <summary>
	/// If this is used as trigger, then yes.
	/// </summary>
	public bool isTrigger = true;
	
	public bool setOnStartup;
	
	public Vector3 targetWorldRotation;
	public float gravityValue = 9.81f;
	void Start(){
		ChangeWorldRotation(targetWorldRotation);
	}
	private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			ChangeWorldRotation(targetWorldRotation);
		}
	}
	private void OnCollisionEnter (Collision other) {
		if (other.gameObject.tag == "Player") {
			ChangeWorldRotation(targetWorldRotation);
		}
	}
	public static void ChangeWorldRotation(Vector3 to){
		//SimulateGravity
		GameObject SimulationCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		SimulationCube.transform.position = new Vector3(0, 100, 0);
        SimulationCube.transform.eulerAngles = to;
        Destroy(SimulationCube.GetComponent<BoxCollider>());
        Destroy(SimulationCube.GetComponent<MeshRenderer>());
        Destroy(SimulationCube.GetComponent<MeshFilter>());
        bool useDefault = false;
        GravityChange changeValuesCentre = new GravityChange();
        if(UnityEngine.Object.FindObjectOfType<GravityChange>() != null){
        	changeValuesCentre = UnityEngine.Object.FindObjectOfType<GravityChange>();
        }else{
        	useDefault = true;
        }
		LineMovement mov = UnityEngine.Object.FindObjectOfType<LineMovement>();
		Vector3 targetGravity = Vector3.zero;
		Vector3 targ1 = new Vector3(SimulationCube.transform.eulerAngles.x, mov.turnBlock2.y, SimulationCube.transform.eulerAngles.z);
		Vector3 targ2 = new Vector3(SimulationCube.transform.eulerAngles.x, mov.turnBlock1.y, SimulationCube.transform.eulerAngles.z);
		if(useDefault){
			targetGravity = (-SimulationCube.transform.up * 9.81f);
		}else{
			targetGravity = (-SimulationCube.transform.up * changeValuesCentre.gravityValue);
		}
		mov.turnBlock1 = targ1;
		mov.turnBlock2 = targ2;
		if(mov.loopCount % 2 != 0){
			mov.TurnPlayer(targ2);
		}else{
			mov.TurnPlayer(targ1);
		}
		Physics.gravity = targetGravity;
		Debug.Log("Simulated! Gravity :" + targetGravity + " - Player rotation : " + new Vector3(SimulationCube.transform.eulerAngles.x, mov.transform.eulerAngles.y, SimulationCube.transform.eulerAngles.z));
	}
    public static Vector3 Singularite(Vector3 from)
    {
        var rot = Quaternion.Euler(from);

        var forward = Vector3.forward;  // fairly common

        var result = rot * forward;
        return result;
    }
}