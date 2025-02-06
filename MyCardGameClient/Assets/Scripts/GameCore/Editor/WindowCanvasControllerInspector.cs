using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WindowCanvasController))]
public class WindowCanvasControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var ctrl = target as WindowCanvasController;

        ctrl.TryInitialize();
    }
}
