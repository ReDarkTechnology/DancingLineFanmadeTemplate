using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;

public class SceneGenerator : ScriptableWizard
{
	public string levelName;
	public string musicCredit;
	public bool addGround = true;
	
	[Header("Game Variables")]
	public AudioClip levelMusic;
	public float lineSpeed = 12;
	public Color LineColor = Color.white;
	
	[Header("Camera Variables")]
	public Vector3 pivotOffset = new Vector3(5, 0, 5);
	public float targetX = 45f;
	public float targetY = 45f;
	public float targetZ;
	public float TargetDistance = 20f;
	
    public static Camera cam;
    public static Camera lastUsedCam;

    //Generated plane meshes are saved and loaded from Plane Meshes folder (you can change it to whatever you want)
    public static string assetSaveLocation = "Assets/Game Core/Scenes";
	
    public static SceneGenerator self;
    [MenuItem("Dancing Line/Generate New Level...")]
    public static void CreateWizard()
    {
        //Check if the asset save location folder exists
        //If the folder doesn't exists, create it
        if (!Directory.Exists(assetSaveLocation))
        {
            Directory.CreateDirectory(assetSaveLocation);
        }

        //Open Wizard
        var wiz = DisplayWizard("Generate New Level", typeof(SceneGenerator));
    }
    // disable AccessToStaticMemberViaDerivedType
    private void OnWizardCreate()
    {
    	if(string.IsNullOrEmpty(levelName)){
    		levelName = "Hello World";
    	}
    	
    	// Creating a new scene
    	
    	Scene s = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    	
    	// Destroy the default camera since we have our own Camera
    	
    	DestroyImmediate(Camera.main.gameObject);
    	
    	// Setting the scene view
    	
    	Vector3 positione = SceneView.lastActiveSceneView.pivot;
        positione.z -= 10.0f;
        SceneView.lastActiveSceneView.pivot = new Vector3(-5 , 7, -5);
        SceneView.lastActiveSceneView.rotation = Quaternion.Euler(new Vector3(45, 45, 0));
        SceneView.lastActiveSceneView.Repaint();
        
    	// Spawning base template
    	
    	const string gamePrefabLocation = "Assets/Game Core/Prefabs/BaseTemplate.prefab";
    	var gamePrefab = (GameObject) PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(gamePrefabLocation));
    	gamePrefab.name = "Base Template";
    	GameManager manager = gamePrefab.GetComponentInChildren<GameManager>();
    	LineMovement mov = gamePrefab.GetComponentInChildren<LineMovement>();
    	manager.lineSpeed = lineSpeed;
        if (!Directory.Exists("Assets/Game Core/Levels/" + levelName)) Directory.CreateDirectory("Assets/Game Core/Levels/" + levelName);
        var path = "Assets/Game Core/Levels/" + levelName + "/" + levelName + ".asset";
        var property = ScriptableObject.CreateInstance<LevelProperty>();
        property.LevelName = levelName;
        if(string.IsNullOrWhiteSpace(musicCredit)){
        	if (levelMusic != null)
        	{
	            property.LevelMusic = levelMusic.name;
        	}
        }else{
        	property.LevelMusic = musicCredit;
        }
        manager.property = property;
    	if(levelMusic != null){
    		manager.audioClip = levelMusic;
    	}
        var renderer = mov.GetComponent<MeshRenderer>();
        var mat = new Material(renderer.sharedMaterial);
        mat.color = LineColor;
        AssetDatabase.CreateAsset(property, "Assets/Game Core/Levels/" + manager.property.LevelName + "/" + manager.property.LevelName + ".asset");
        AssetDatabase.CreateAsset(mat, "Assets/Game Core/Levels/" + manager.property.LevelName + "/LineMaterial.mat");
        renderer.sharedMaterial = mat;
        
    	// Spawning camera
    	
    	const string cameraPrefabLocation = "Assets/Game Core/BetterCamera/CameraPivot.prefab";
    	var cameraPrefab = (GameObject) PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(cameraPrefabLocation));
    	cameraPrefab.name = "Camera Pivot";
    	BetterCamera cam = cameraPrefab.GetComponent<BetterCamera>();
    	cam.Line = mov.transform;
    	cam.targetX = targetX;
    	cam.targetY = targetY;
    	cam.targetZ = targetZ;
    	cam.TargetDistance = TargetDistance;
    	cam.pivotOffset = pivotOffset;
    	
    	// If add ground checked, spawn ground
    	
    	if(addGround){
    		const string groundLocation = "Assets/Game Core/Prefabs/Wide Ground.prefab";
    		var groundPrefab = (GameObject) PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(groundLocation));
    		groundPrefab.name = "Base Ground";
    	}
    	
    	// Save the scene and make the default selection as the manager.
    	
    	Selection.activeObject = manager.gameObject;
        EditorSceneManager.SaveScene(s, "Assets/Game Core/Levels/" + manager.property.LevelName + "/" + manager.property.LevelName + ".unity");
    }
}