using UnityEngine;
[CreateAssetMenu(fileName = "SkinType", menuName = "Line Game/Skin Type", order = 0)]
public class SkinType : ScriptableObject {
	public Mesh skinMesh;
	public GameObject additionalDecoration;
	public bool disableDefaultTail;
	[System.Serializable]
	public class AppearEveryFrame{
		public bool enable;
		public GameObject instance;
		public float spawnDelay;
	}
	public AppearEveryFrame appearEveryFrame;
	[System.Serializable]
	public class AppearEveryTap{
		public bool enable;
		public bool scaleAfterSpawning;
		public GameObject instance;
	}
	public AppearEveryTap appearEveryTap;
	[System.Serializable]
	public class FollowingObject{
		public bool enable;
		public GameObject theObject;
	}
	public FollowingObject followingObject;
}
