using System.IO;
using UnityEngine;
using FullSerializer;

[ExecuteInEditMode]
public class MonoSerializeObject : MonoBehaviour
{
	[Header("Object Serializing")]
	public GameObject targetObject;
	public string serializedFilePath = "gameObject.json";
	public bool serialize;
	
	public bool spawn;
	
	[Header("Utility Resets")]
	public bool resetMd5Dictionary;
	public bool resetObjectIdentityDictionary;
	
	[Header("Utility Tool")]
	public ObjectIdentity searchResult;
	public string searchMd5;
	public bool search;

    // Serializer (because JsonUtility sucks)
    // But Full Serializer is slow so please, if you have a suggestion to which serializer to use, tell me.
    private static readonly fsSerializer _serializer = new fsSerializer();

    void Update()
    {
    	if(search){
    		searchResult = ObjectIdentity.GetIdentity(searchMd5);
    		search = false;
    	}
        if (serialize) {
    		if(targetObject != null){
    			string a = null;
    			var s = SerializedGameObject.SerializeGameObject(targetObject);
                fsData data;
                _serializer.TrySerialize(typeof(SerializedGameObject), s, out data).AssertSuccessWithoutWarnings();

                // emit the data via JSON
                a = fsJsonPrinter.CompressedJson(data);
                File.WriteAllText(serializedFilePath, a);
    		}
    		serialize = false;
    	}
    	if(spawn)
    	{
    		if(File.Exists(serializedFilePath)){
    			var r = File.ReadAllText(serializedFilePath);
                fsData data = fsJsonParser.Parse(r);

                // deserialize the data
                object deserialized = null;
                _serializer.TryDeserialize(data, typeof(SerializedGameObject), ref deserialized).AssertSuccessWithoutWarnings();
                var c = (SerializedGameObject)deserialized;
    			c.Spawn();
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
