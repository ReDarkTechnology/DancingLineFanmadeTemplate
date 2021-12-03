using System;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;

public static class Utility
{
	public static string Vector3String (Vector3 v)
	{
		return "(" + v.x + "," + v.y + "," + v.z + ")";
	}
	public static Vector3 StringVector3 (string s, Vector3 defaultVector = default(Vector3))
	{
		Vector3 result = defaultVector;
		try {
			var s1 = s.Remove (0,1);
			s1 = s1.Remove(s1.Length - 1, 1);
			var s2 = s1.Split(',');
			result = new Vector3 (float.Parse(s2[0]), float.Parse(s2[1]), float.Parse(s2[2]));
		} catch {
			result = defaultVector;
		}
		return result;
	}
	static readonly char[] randomMD5 = "abcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();
    public static List<string> knownMD5 = new List<string>();
    public static string GetMD5()
    {
        var rand = new Random();
        string build = null;
        for (int i = 0; i < 20; i++)
        {
            build += randomMD5[rand.Next(0, randomMD5.Length - 1)];
        }
        if (knownMD5.Contains(build))
        {
            build = GetMD5();
        }
        knownMD5.Add(build);
        return build;
    }
}
