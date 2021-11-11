using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

[Serializable]
public class SerializedGameObject {

    // Info
    public bool selfActive;
	public string name;
	public string tag;
	public string md5;
	public string parentMD5;
	public int layer;
	
	// Transform
	public Vector3 localPosition;
	public Vector3 localEulerAngle;
	public Vector3 localScale;

    // Childs
    public SerializedGameObject[] childs;

    // Components
    public List<SerializedComponent> components = new List<SerializedComponent>();
	
	public static SerializedGameObject SerializeGameObject(GameObject obj){
		var cls = new SerializedGameObject();
		cls.name = obj.name;
		cls.tag = obj.tag;
		cls.layer = obj.layer;

        cls.selfActive = obj.activeSelf;

		cls.localPosition = obj.transform.localPosition;
		cls.localEulerAngle = obj.transform.localEulerAngles;
		cls.localScale = obj.transform.localScale;
		
		var identity = obj.GetComponent <ObjectIdentity>();
		if (identity == null)
		{
			identity = obj.AddComponent <ObjectIdentity>();
			identity.md5 = Utility.GetMD5();
		}
		
		cls.md5 = identity.md5;
		
		if(obj.transform.parent != null)
		{
			var identity1 = obj.transform.parent.GetComponent <ObjectIdentity>();
			if (identity1 == null)
			{
				identity1 = obj.transform.parent.gameObject.AddComponent <ObjectIdentity>();
				identity1.md5 = Utility.GetMD5();
			}
			cls.parentMD5 = identity1.md5;
		}
		
		var filter = obj.GetComponent<MeshFilter>();
		if(filter != null){
			cls.components.Add (MeshFilterClass.GetMeshClassFromRenderer(filter).Serialize());
		}
		var collider = obj.GetComponent<Collider>();
		if(collider != null){
			cls.components.Add (ColliderClass.GetColliderClassFromObject(obj).Serialize());
		}
		var renderer = obj.GetComponent<MeshRenderer>();
		if(renderer != null){
			cls.components.Add (MeshRendClass.GetMeshClassFromRenderer(renderer).Serialize());
		}

        var objs = obj.GetComponentsInChildren<Transform>();
        var rootObjs = new List<SerializedGameObject>();
        foreach (var ob in objs)
        {
            if (ob.gameObject != obj)
            {
                rootObjs.Add(SerializeGameObject(ob.gameObject));
            }
        }
        cls.childs = rootObjs.ToArray();
        return cls;
	}
	
	public ObjectIdentity Spawn (Material diffuse = null)
	{
		var obj = new GameObject();
		var identity = obj.AddComponent <ObjectIdentity>();
		obj.name = name;
		obj.tag = tag;
		obj.layer = layer;
        obj.SetActive(selfActive);
		identity.md5 = md5;
		identity.parentMD5 = parentMD5;
		identity.cachedPosition = localPosition;
		identity.cachedEuler = localEulerAngle;
		identity.cachedScale = localScale;
		identity.SetAsCache();
        identity.Register();

        foreach (var comp in components)
		{
            switch (comp.name) {
				case "ColliderClass":
					var collider = JsonUtility.FromJson<ColliderClass>(comp.data);
					collider.ApplyTo(obj);
					break;
				case "MeshRendClass":
					var renderer = JsonUtility.FromJson<MeshRendClass>(comp.data);
					renderer.ApplyTo(obj);
					break;
				case "MeshFilterClass":
					var filter = JsonUtility.FromJson<MeshFilterClass>(comp.data);
					filter.ApplyTo(obj);
					break;
				default:
					Debug.LogError("Component is not recognizable");
					break;
			}
            /*var component = GetComponent(comp);
            if(component != null)
            {
                component = JsonUtility.FromJson(comp.data, component.GetType());
                component.ApplyTo (obj);
            }*/
        }

        var spawned = new List<ObjectIdentity>();
        foreach (var child in childs)
        {
            var spawn = child.Spawn();
            spawn.Register();
            spawned.Add(spawn);

        }
        foreach (var spawn in spawned)
        {
            spawn.SetAsCache();
        }
        return identity;
	}

    public SerializableComponent GetComponent (SerializedComponent component)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(SerializedComponent)));
        foreach (Type mytype in types)
        {
            if (mytype.ToString() == component.name)
            {
                return (SerializableComponent)mytype;
            }
        }
        return null;
    }
	
}

#region ComponentClasses
[Serializable]
public class ColliderClass : SerializableComponent {
	public enum ColliderType{
		BoxCol,
		MeshCol,
		SphereCol
	}
	public ColliderType colType;
	public bool enabled;
	[Header("CommonCollider")]
	public Vector3 offset;
	[Header("BoxCollider")]
	public Vector3 size;
	[Header("SphereCollider")]
	public float radius;
	[Header("MeshCollider")]
	public bool convex;
	
	public static ColliderClass GetColliderClassFromObject(GameObject obj){
		ColliderClass cls = new ColliderClass();
		if(obj.GetComponent<BoxCollider>() != null){
			BoxCollider col = obj.GetComponent<BoxCollider>();
			cls.colType = ColliderType.BoxCol;
			cls.enabled = col.enabled;
			cls.offset = col.center;
			cls.size = col.size;
		}else{
			if(obj.GetComponent<MeshCollider>() != null){
				MeshCollider col = obj.GetComponent<MeshCollider>();
				cls.colType = ColliderType.MeshCol;
				cls.enabled = col.enabled;
				cls.convex = col.convex;
			}else{
				if(obj.GetComponent<SphereCollider>() != null){
					SphereCollider col = obj.GetComponent<SphereCollider>();
					cls.colType = ColliderType.SphereCol;
					cls.enabled = col.enabled;
					cls.offset = col.center;
					cls.radius = col.radius;
				}
			}
		}
		return cls;
	}
	public SerializedComponent Serialize ()
	{
		var cls = new SerializedComponent();
		cls.name = "ColliderClass";
		cls.data = JsonUtility.ToJson(this);
		return cls;
	}
	public string GetName(){
		return "ColliderClass";
	}
	public void ApplyTo (GameObject gameObject)
	{
		if(colType == ColliderClass.ColliderType.BoxCol){
			BoxCollider col = gameObject.GetComponent<BoxCollider>();
			if(col == null) col = gameObject.AddComponent<BoxCollider>();
			col.enabled = enabled;
			col.center = offset;
			col.size = size;
			return;
		}
		if(colType == ColliderClass.ColliderType.SphereCol){
			SphereCollider col = gameObject.GetComponent<SphereCollider>();
			if(col == null) col = gameObject.AddComponent<SphereCollider>();
			col.enabled = enabled;
			col.center = offset;
			col.radius = radius;
			return;
		}
		if(colType == ColliderClass.ColliderType.MeshCol){
			MeshCollider col = gameObject.GetComponent<MeshCollider>();
			if(col == null) col = gameObject.AddComponent<MeshCollider>();
			col.enabled = enabled;
			col.convex = convex;
			return;
		}
	}
}

[Serializable]
public class MeshRendClass : SerializableComponent
{
	public bool isEnabled;
	public Color[] materials;
	public static MeshRendClass GetMeshClassFromRenderer(MeshRenderer rend){
		var cls = new MeshRendClass(){
			isEnabled = rend.enabled
		};
		var colors = new List<Color>();
		var mats = rend.sharedMaterials;
		foreach(var a in mats){
			colors.Add(a.color);
		}
		cls.materials = colors.ToArray();
		return cls;
	}
	public SerializedComponent Serialize ()
	{
		var cls = new SerializedComponent();
		cls.name = "MeshRendClass";
		cls.data = JsonUtility.ToJson(this);
		return cls;
	}
	public string GetName(){
		return "MeshRendClass";
	}
	public void ApplyTo (GameObject gameObject)
    {
    	var rend = gameObject.AddComponent <MeshRenderer>();
    	rend.enabled = isEnabled;
    	var mats = new List<Material>();
    	GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
    	primitive.SetActive(false);
    	var diffuse = primitive.GetComponent<MeshRenderer>().sharedMaterial;
    	foreach (var col in materials)
    	{
    		var mat = new Material(diffuse);
    		mat.color = col;
    		mats.Add(mat);
    	}
    	rend.materials = mats.ToArray();
		UnityEngine.Object.DestroyImmediate(primitive);
    }
    public void ApplyTo (GameObject gameObject, Material diffuse)
    {
    	var rend = gameObject.AddComponent <MeshRenderer>();
    	rend.enabled = isEnabled;
    	var mats = new List<Material>();
    	foreach (var col in materials)
    	{
    		var mat = new Material(diffuse);
    		mat.color = col;
    		mats.Add(mat);
    	}
    	rend.materials = mats.ToArray();
    }
}

[Serializable]
public class MeshFilterClass : SerializableComponent
{
    public Mesh mesh;
    public static MeshFilterClass GetMeshClassFromRenderer(MeshFilter rend)
    {
        MeshFilterClass cls = new MeshFilterClass()
        {
            mesh = rend.sharedMesh
        };
        return cls;
    }
	public SerializedComponent Serialize ()
	{
		var cls = new SerializedComponent();
		cls.name = "MeshFilterClass";
		cls.data = JsonUtility.ToJson(this);
		return cls;
	}
	public string GetName(){
		return "MeshFilterClass";
	}
    public void ApplyTo (GameObject gameObject)
    {
    	var filter = gameObject.AddComponent <MeshFilter>();
    	filter.sharedMesh = mesh;
    }
}


[Serializable]
public class RigidbodyClass
{
	public float drag;
	public float angularDrag;
	public float mass;
	public bool useGravity;
	public float maxDepenetrationVelocity;
	public bool isKinematic;
	public bool freezeRotation;
	public RigidbodyConstraints constraints;
	public CollisionDetectionMode collisionDetectionMode;
}
[Serializable]
public class SerializedMember
{
	public SerializedMember (string name = null, string variable = null){
		this.name = name;
		this.variable = variable;
	}
	public string name;
	public string variable;
}
#endregion