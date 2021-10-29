using UnityEngine;
[CreateAssetMenu(fileName = "TailType", menuName = "Line Game/Tail Type", order = 0)]
public class TailType : ScriptableObject {
	public PrimitiveType primitiveType = PrimitiveType.Cube;
	public Vector3 defaultScale = Vector3.one;
	public Vector3 colliderSize = Vector3.one;
	public float spawnZOffset;
	public bool colliderIsTrigger = true;
	public bool addRigidbody;
	public Rigidbody constraintsAsObject;
	public float rigidbodyMass;
}
