using UnityEngine;
using UnityEditor;

public class HowToMakeLevels : EditorWindow
{
    [MenuItem("Dancing Line/How to make levels?")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        var window = (HowToMakeLevels)EditorWindow.GetWindow(typeof(HowToMakeLevels));
        window.titleContent.text = "Guide";
        window.Show();
        var posX = window.position.x;
        var posY = window.position.y;
        window.position = new Rect(posX, posY, 316, 472);
    }

    void OnGUI()
    {
    	var style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 15;
        style.wordWrap = true;
        EditorGUILayout.Space();
    	EditorGUILayout.LabelField("Fanmade Creator Guide", style);
    	DrawUILine();
        style = new GUIStyle();
        style.wordWrap = true;
        style.margin = new RectOffset(8, 5, 0, 0);
        style.fontStyle = FontStyle.Italic;
    	EditorGUILayout.LabelField("This is the steps on how to make a new level : ", style);
    	GUILayout.Space(5);
        style.fontStyle = FontStyle.Normal;
    	EditorGUILayout.LabelField("1. Generate a new level using : \n \"Dancing Line -> Generate New Level...\"", style);
    	var satyle = EditorStyles.miniLabel;
    	satyle.wordWrap = true;
    	EditorGUILayout.LabelField("A new folder will be created on : \n \"Assets\\Game Core\\Levels\\\"", satyle);
    	//GUILayout.Space(10);
    	if(GUILayout.Button("Create New...")) SceneGenerator.CreateWizard();
    	GUILayout.Space(5);
    	EditorGUILayout.LabelField("2. Import a music to the project.", style);
    	GUILayout.Space(5);
    	EditorGUILayout.LabelField("3. Put the AudioClip on : \n BaseTemplate -> GameCore -> AudioClip", style);
    	if(GUILayout.Button("Find GameCore")) {
    		var manager = FindObjectOfType<GameManager>();
    		if(manager != null){
    			Selection.objects = new Object[] {manager.gameObject};
    		}
    	}
    	GUILayout.Space(5);
    	EditorGUILayout.LabelField("4. Set your line speed on : \n BaseTemplate -> GameCore -> Line Speed", style);
    	GUILayout.Space(5);
    	EditorGUILayout.LabelField("5. Sync the taps with the music in play mode, pause it and copy 'Parents' to get the trail.", style);
    	if(GUILayout.Button("Find 'Parents'")) {
    		var manager = FindObjectOfType<GameManager>();
    		if(manager != null){
    			var obj = manager.gameObject.transform.parent.Find("Parents");
    			if(obj != null)
    				Selection.objects = new Object[] {obj.gameObject};
    		}
    	}
    	GUILayout.Space(5);
    	EditorGUILayout.LabelField("6. Stop the play mode, and then click the scene in the inspector. Paste it there.", style);
    	GUILayout.Space(5);
    	EditorGUILayout.LabelField("7. Design the level! Try to improve your skills on designing by experiencing more :D", style);
    	GUILayout.Space(20);
    	EditorGUILayout.LabelField("Have fun \n - ReDark Technology", style);
    	if(GUILayout.Button("Donate me! (Don't request me money qwp)")) System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/redarktechnology");
    	if(GUILayout.Button("Join my Discord!")) System.Diagnostics.Process.Start("https://discord.gg/qxTSBjG4Vg");
    	//GUILayout.Label(this.position.width + ", " + position.height);
    }
    public static void DrawUILine(int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        Color color = new Color(.65f, .65f, .65f);
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        EditorGUI.DrawRect(r, color);
    }
}
