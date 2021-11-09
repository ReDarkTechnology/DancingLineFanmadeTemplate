using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedCustomModel
{
	public string modelName;
	public string modelAuthor;
	public SerializedGameObject rootObject;
	public SerializedGameObject[] rootObjects;
	
	public static SerializedCustomModel GetCustomModel (GameObject root)
	{
		var cls = new SerializedCustomModel();
		cls.modelName = root.name;
		cls.modelAuthor = Environment.UserName;
		var objs = root.GetComponentsInChildren <Transform> ();
		var rootObjs = new List<SerializedGameObject>();
		cls.rootObject = SerializedGameObject.SerializeGameObject(root);
		foreach (var obj in objs)
		{
			if(obj.gameObject != root){
				rootObjs.Add(SerializedGameObject.SerializeGameObject(obj.gameObject));
			}
		}
		cls.rootObjects = rootObjs.ToArray();
		return cls;
	}
	public ObjectIdentity Spawn ()
	{
		var spawned = new List<ObjectIdentity>();
		var root = rootObject.Spawn();
		root.Register();
		foreach (var obj in rootObjects)
		{
			var spawn = obj.Spawn();
			spawn.Register();
			spawned.Add (spawn);
			
		}
		foreach(var spawn in spawned){
			spawn.SetAsCache();
		}
		return root;
	}
}
