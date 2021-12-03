using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedWorld
{
    public string worldName;
    public string[] worldAuthors;
    public SerializedGameObject[] worldObjects;
}