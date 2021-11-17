using UnityEngine;

public class ChangeObjMat : MonoBehaviour{
    public Material Mat;
    public GameObject[] Gameobjects;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < Gameobjects.Length; i++) 
            {
                Gameobjects[i].GetComponent<MeshRenderer>().material = Mat;
            }
        }
    }
}
