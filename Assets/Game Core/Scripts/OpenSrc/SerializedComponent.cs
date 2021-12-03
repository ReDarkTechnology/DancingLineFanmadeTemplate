using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedComponent
{
	public string name;
	public string data;
}

public interface SerializableComponent
{
	string GetName();
	void ApplyTo(GameObject obj);
	SerializedComponent Serialize();
}
