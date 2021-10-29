using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
	public KeyCode[] tapKeys = {
		KeyCode.Space,
		KeyCode.Mouse0,
		KeyCode.UpArrow
	};
	public KeyCode restartKey = KeyCode.R;
	public KeyCode pauseKey = KeyCode.Escape;
	public KeyCode quitKey = KeyCode.Escape;
}
