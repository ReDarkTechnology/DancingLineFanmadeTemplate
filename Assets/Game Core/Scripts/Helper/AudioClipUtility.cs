using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public static class AudioClipUtility
{
	public static AudioRequest GetAudioClip(string path){
		AudioRequest req = new AudioRequest();
		req.path = path;
		return req;
	}
	public static void ProcessRequest(AudioRequest req){
		GameObject obj = new GameObject();
		obj.name = "ParseRequest";
		req.temporaryObject = obj;
		var mono = obj.AddComponent<DefaultMono>();
		mono.StartCoroutine(GetClipEnumerator(req));
	}
	public static IEnumerator GetClipEnumerator(AudioRequest request){
		string audioPath = request.path;
		if(File.Exists(audioPath)){
			#if UNITY_2017_1_OR_NEWER
			using (var download = new UnityEngine.Networking.UnityWebRequest(audioPath)){
				yield return download.SendWebRequest();
				var theClip = ((UnityEngine.Networking.DownloadHandlerAudioClip) download.downloadHandler).audioClip;
				if (theClip != null) {
					theClip.name = Path.GetFileNameWithoutExtension (audioPath);
					request.result = theClip;
					request.OnRequestCompleted.Invoke(request.result);
				} else {
					Debug.LogError ("The audio file can't be loaded or isn't supported.");
				}
			}
			#else
			using (var download = new WWW (audioPath)) {
				yield return download;
				var theClip = download.GetAudioClip (false, true);
				if (theClip != null) {
					theClip.name = Path.GetFileNameWithoutExtension (audioPath);
					audioClip = theClip;
					isAudioReady = true;
				} else {
					Debug.LogError ("The audio file can't be loaded or isn't supported.");
				}
			}
			#endif
		}else{
			Debug.LogError("The file does not exist");
		}
		if(request.temporaryObject != null){
			UnityEngine.Object.DestroyImmediate(request.temporaryObject);
		}
	}
}
[Serializable]
public class AudioRequest
{
	public string path;
	public AudioClip result;
	public Action<AudioClip> OnRequestCompleted;
	public GameObject temporaryObject;
	public void Request(){
		AudioClipUtility.ProcessRequest(this);
	}
}
