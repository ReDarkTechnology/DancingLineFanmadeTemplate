using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIdentity : MonoBehaviour
{
	public string md5;
	
	public Vector3 cachedPosition;
	public Vector3 cachedEuler;
	public Vector3 cachedScale;
	
	public void SetAsCache ()
	{
		transform.localPosition = cachedPosition;
		transform.localEulerAngles = cachedEuler;
		transform.localScale = cachedScale;
	}
	
	public static implicit operator GameObject(ObjectIdentity v) {
		return v.gameObject;
	}
}