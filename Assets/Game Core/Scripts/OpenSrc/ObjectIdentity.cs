using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIdentity : MonoBehaviour
{
	public string md5;
	
	public string parentMD5;
	
	public Vector3 cachedPosition;
	public Vector3 cachedEuler;
	public Vector3 cachedScale;

    public bool isCustomModel;

    public static Dictionary<string, ObjectIdentity> customIdentities = new Dictionary<string, ObjectIdentity>();

	public static Dictionary<string, ObjectIdentity> identities = new Dictionary<string, ObjectIdentity>();
	
	public void SetAsCache ()
	{
		if(!string.IsNullOrWhiteSpace(parentMD5)) {
			var parent = GetIdentity(parentMD5);
			if(parent != null){
				transform.SetParent (parent.transform);
			}
		}
		
		transform.localPosition = cachedPosition;
		transform.localEulerAngles = cachedEuler;
		transform.localScale = cachedScale;
	}
	public static ObjectIdentity GetIdentity (string md5)
	{
		if(identities.ContainsKey(md5)) return identities[md5];
		return null;
	}
	public void Register ()
	{
		if(!isRegistered){
			if(!identities.ContainsKey(md5))
				identities.Add(md5, this);
			isRegistered = true;
		}
	}
	public bool isRegistered;
	void Start(){
		Register();
	}
	
	void OnDestroy ()
	{
		if(identities.ContainsKey(md5))
			identities.Remove(md5);
	}
	
	public static implicit operator GameObject(ObjectIdentity v) {
		return v.gameObject;
	}
}