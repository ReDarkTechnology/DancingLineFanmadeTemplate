using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class MonoSerializeObject : MonoBehaviour
{
	[Header("Object Serializing")]
	public GameObject targetObject;
	public string serializedFilePath = "gameObject.json";
	public bool serialize;
	public bool asCustomModel;
	
	public bool spawn;
	
	[Header("Utility Resets")]
	public bool resetMd5Dictionary;
	public bool resetObjectIdentityDictionary;
	
	[Header("Utility Tool")]
	public ObjectIdentity searchResult;
	public string searchMd5;
	public bool search;
	
    void Update()
    {
    	if(search){
    		searchResult = ObjectIdentity.GetIdentity(searchMd5);
    		search = false;
    	}
    	if(serialize){
    		if(targetObject != null){
    			string a = null;
    			if(!asCustomModel){
    				var s = SerializedGameObject.SerializeGameObject(targetObject);
    				a = JsonUtility.ToJson(s);
    			}else{
    				var s = SerializedCustomModel.GetCustomModel(targetObject);
    				a = JsonUtility.ToJson(s);
    			}
    			File.WriteAllText(serializedFilePath, a);
    		}
    		serialize = false;
    	}
    	if(spawn)
    	{
    		if(File.Exists(serializedFilePath)){
    			var r = File.ReadAllText(serializedFilePath);
    			if(!asCustomModel){
    				var c = JsonUtility.FromJson<SerializedGameObject>(r);
    				c.Spawn();
    			}else{
    				var c = JsonUtility.FromJson<SerializedCustomModel>(r);
    				c.Spawn();
    			}
    		}
    		spawn = false;
    	}
    	if(resetMd5Dictionary){
    		Utility.knownMD5.Clear();
    		resetMd5Dictionary = false;
    	}
    	if(resetObjectIdentityDictionary){
    		ObjectIdentity.identities.Clear();
    		resetObjectIdentityDictionary = false;
    	}
    }
}
