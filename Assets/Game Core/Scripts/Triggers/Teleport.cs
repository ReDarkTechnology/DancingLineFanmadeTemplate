using UnityEngine;

public class Teleport : MonoBehaviour
{

	public Vector3 To_Teleport_position = Vector3.zero;
    public GameObject WantToTeleportObject;

	public void OnTriggerEnter (Collider other)
	{
        if (other.tag == "Player")
        {
            WantToTeleportObject.transform.position = new Vector3(To_Teleport_position.x, To_Teleport_position.y, To_Teleport_position.z);
        }
	}
}
