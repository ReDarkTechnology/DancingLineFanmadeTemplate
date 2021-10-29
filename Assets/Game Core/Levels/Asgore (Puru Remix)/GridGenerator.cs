using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridGenerator : MonoBehaviour
{
	public bool CreateGrid;
	public Material material;
	public Vector2 offset = new Vector2 (5, 5);
	public Vector2 size = new Vector2 (0.1f, 0.1f);
	public Transform GridParents;
	
	public float gridRange = 1000;
	
	void Update(){
		if(CreateGrid){
			// Destroy Previous
			if(GridParents != null){
				var previous = GridParents.GetComponentsInChildren<Transform>(true);
				foreach(var a in previous){
					if(a != GridParents)
						DestroyImmediate(a.gameObject);
				}
			}
			// X
			for(float i = -gridRange; i < gridRange; i += offset.x){
				var cube = CreateCube();
				cube.transform.position = new Vector3 (i, 0, 0);
				cube.transform.localScale = new Vector3 (size.x, size.y, gridRange * 2);
			}
			// Y
			for(float i = -gridRange; i < gridRange; i += offset.y){
				var cube = CreateCube();
				cube.transform.position = new Vector3 (0, 0, i);
				cube.transform.localScale = new Vector3 (gridRange * 2, size.y, size.x);
			}
			CreateGrid = false;
		}
	}
	public GameObject CreateCube(){
		var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		if(material != null){
			cube.GetComponent<MeshRenderer>().material = material;
		}
		if(GridParents != null){
			cube.transform.SetParent(GridParents);
		}
		cube.name = "grid_instance";
		return cube;
	}
}
