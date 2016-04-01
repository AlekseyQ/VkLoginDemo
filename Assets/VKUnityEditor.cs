using UnityEngine;
using System.Collections;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;

public class VKUnityReadOnlyAttribute : PropertyAttribute { }
public class VKUnityInspectorHackAttribute : PropertyAttribute { }

public sealed class FingerprintsSingleton
{
    public delegate void FingerprintsHandler(string fingerprints);

    private static readonly FingerprintsSingleton instance = new FingerprintsSingleton();

    private FingerprintsSingleton() { }

    public static FingerprintsSingleton Instance
    {
        get
        {
            return instance;
        }
    }

    private System.Diagnostics.Process process;
    private StringBuilder outputLineBuilder;
    private string fingerprints = string.Empty;
    private FingerprintsHandler myHandler = delegate { };

    void OutputHandler(object sendingProcess, System.Diagnostics.DataReceivedEventArgs outLine)
    {
        if (outLine.Data != null)
        {
            outputLineBuilder.AppendLine(outLine.Data);
        }
        else
        {
            outputLineBuilder.AppendLine();
        }

        var newReg = new System.Text.RegularExpressions.Regex("Certificate\\sfingerprints:[\\s\\w:]*SHA1:\\s*([\\w:]*)");
        System.Text.RegularExpressions.MatchCollection matches = newReg.Matches(outputLineBuilder.ToString());
        if (matches.Count > 0)
        {
            System.Text.RegularExpressions.Match mat = matches[0];
            fingerprints = mat.Groups[1].Value;
        }
    }

    private void OnProcessExited(object sender, System.EventArgs e)
    {
        process = null;
        myHandler(string.IsNullOrEmpty(fingerprints) ? "*" : fingerprints);
    }

    public void RunCommand(FingerprintsHandler myHandler)
    {
        if(process != null)
        {
            return;
        }
        this.myHandler = myHandler;
        outputLineBuilder = new StringBuilder();

        var fileInfo = new System.IO.FileInfo(PlayerSettings.Android.keystoreName);

        process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "keytool";
        process.StartInfo.Arguments = "-exportcert -alias " + PlayerSettings.Android.keyaliasName + (!string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass) ? (" -storepass " + PlayerSettings.Android.keyaliasPass) : "") + " -keystore ./" + fileInfo.Name + " -list -v";
        process.StartInfo.WorkingDirectory = fileInfo.Directory.FullName;

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;

        process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(OutputHandler);
        process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(OutputHandler);

        process.EnableRaisingEvents = true;
        process.Exited += new System.EventHandler(OnProcessExited);

        // start process and handlers
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        //process.WaitForExit();
    }

}

[CustomEditor(typeof(VKUnity))]
public class VKUnityEditor : Editor
{
    private static int value = 0;
    private string fingerprints = string.Empty;

    private GUIContent appIdLabel = new GUIContent("Application ID", "Application ID");
    private GUIContent fingerprintsLabel = new GUIContent("Signing Certificate", "Signing certificate fingerprint for Android");
    private GUIContent packageNameLabel = new GUIContent("Package Name", "Package name for Android");
    private GUIContent mainActivityLabel = new GUIContent("Main Activity", "Main activity for Android");
    private GUIContent keystoreLocationLabel = new GUIContent("Keystore Location", "Keystore location for your app");
    private GUIContent permissionsLabel = new GUIContent("Permissions", "Scope");

    private bool showVKPermissionsSettings = false;

    void MyFingerprintsHandler(string fingerprints)
    {
        this.fingerprints = fingerprints;
        var myScript = target as VKUnity;
        myScript.__ = value++.ToString();
    }

    void OnEnable()
    {
        var fileInfo = new System.IO.FileInfo(PlayerSettings.Android.keystoreName);
        if(fileInfo.Exists)
        {
            fingerprints = "Evaluate...";
            FingerprintsSingleton.Instance.RunCommand(MyFingerprintsHandler);
        }
    }

    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        var myScript = target as VKUnity;

        EditorGUILayout.LabelField("VK Application Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(appIdLabel, GUILayout.MaxWidth(180), GUILayout.MinWidth(100));
        myScript.appId = EditorGUILayout.IntField(myScript.appId, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("VK SDK Settings", EditorStyles.boldLabel);
        SelectableLabelField(fingerprintsLabel, fingerprints);
        SelectableLabelField(packageNameLabel, PlayerSettings.bundleIdentifier);
        SelectableLabelField(mainActivityLabel, "com.unity3d.player.UnityPlayerActivity");

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Fingerprint Receiving via Keytool", EditorStyles.boldLabel);
        SelectableLabelField(keystoreLocationLabel, PlayerSettings.Android.keystoreName);
        EditorGUILayout.HelpBox("keytool -exportcert " + (!string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName) ? (" -alias " + PlayerSettings.Android.keyaliasName) : "") + " -keystore path-to-debug-or-production-keystore -list -v", MessageType.Info);

        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
        myFoldoutStyle.fontStyle = FontStyle.Bold;

        this.showVKPermissionsSettings = EditorGUILayout.Foldout(this.showVKPermissionsSettings, permissionsLabel, myFoldoutStyle);
        if (this.showVKPermissionsSettings)
        {
            foreach (VKScope scope in VKScope.GetValues(typeof(VKScope)))
            {
                bool b = myScript.scope[(int)scope];
                myScript.scope[(int)scope] = EditorGUILayout.Toggle(System.Enum.GetName(typeof(VKScope), scope), b);
            }
        }
        EditorGUILayout.Space();
    }

    private void SelectableLabelField(GUIContent label, string value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.MaxWidth(180), GUILayout.MinWidth(100), GUILayout.Height(16));
        EditorGUILayout.SelectableLabel(value, GUILayout.ExpandWidth(true), GUILayout.Height(16));
        EditorGUILayout.EndHorizontal();
    }

}

[CustomPropertyDrawer(typeof(VKUnityReadOnlyAttribute))]
 public class VKUnityReadOnlyDrawer : PropertyDrawer
 {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}

[CustomPropertyDrawer(typeof(VKUnityInspectorHackAttribute))]
public class VKUnityInspectorHackDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // nothing
    }
}

#endif