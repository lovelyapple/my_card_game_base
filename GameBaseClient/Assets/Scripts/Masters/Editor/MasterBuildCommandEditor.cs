using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using R3;
public class MasterBuildCommandEditor : EditorWindow
{
    private static MasterBuildCommandEditor _instance;
    [MenuItem(("Command/MasterBuilderEditor"))]
    public static void Open()
    {
        if (_instance == null)
        {
            _instance = EditorWindow.GetWindow(typeof(MasterBuildCommandEditor)) as MasterBuildCommandEditor;
        }
    }
    private static string _currentInput = "";
    private static string _currentCmd = "";
    private static string _cmdConsoleText = "";
    private Vector2 _scrollPosition;
    private const string GenerateMasterMemoryCode = "dotnet-mmgen" +
                                                    " -inputDirectory " + SystemPath.MasterMemoryClassDefinePath + 
                                                    " -outputDirectory " + SystemPath.MasterMemoryGeneratePath +
                                                    " -usingnamespace \"Starmessage.Master\"" +
                                                    " -addImmutableConstructor";
    private const string GenerateMessagePackCode = "mpc" +
                                                   " -input " + SystemPath.MasterMemoryClassDefinePath + 
                                                   " -output " + SystemPath.MasterMemoryGeneratePath;
    void OnGUI()
    {
        using (new GUILayout.HorizontalScope())
        {
            _currentInput = EditorGUILayout.TextField(_currentInput);

            if (GUILayout.Button("Execute", GUILayout.Width(100)))
            {
                ExecuteCommandAsync(_currentInput).Forget();
            }
        }
        using (var h = new EditorGUILayout.VerticalScope())
        {
            // _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
            using (var scrollView  = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUILayout.Height(600)))
            {
                _scrollPosition = scrollView .scrollPosition;
                EditorGUILayout.TextArea(_cmdConsoleText, GUILayout.ExpandHeight(true));
            }
            // EditorGUILayout.EndScrollView();
        }
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("GenerateMasterMemory", GUILayout.Width(200)))
            {
                GenerateMasterMemoryAsync().Forget();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Clear"))
            {
                _cmdConsoleText = "";
                GUI.FocusControl(null);
            }
        }
    }
    private async UniTask GenerateMasterMemoryAsync()
    {
        await ExecuteCommandAsync(GenerateMasterMemoryCode);
        await ExecuteCommandAsync(GenerateMessagePackCode);
    }
    private async UniTask ExecuteCommandAsync(string cmd)
    {
        if (!string.IsNullOrEmpty(_currentCmd) || string.IsNullOrEmpty(cmd))
        {
            return;
        }

        _currentCmd = cmd;
        _cmdConsoleText += _currentCmd + "\n";
        
        var outPut = await BashCommandInputter.DoBashCommand(cmd);
        _cmdConsoleText += outPut + "\n";
        _currentCmd = "";
    }
}

