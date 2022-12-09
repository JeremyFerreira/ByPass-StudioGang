using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(EventSO))]
public class EventSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EventSO eventSO = (EventSO)this.target;
        var style = new GUIStyle(GUI.skin.button);
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Launch Event",style, GUILayout.Height(80)))
        {
            eventSO.OnLaunchEvent?.Invoke();
        }
    }
}