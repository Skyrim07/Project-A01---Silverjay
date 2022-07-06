using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SKCell
{
    [CustomEditor(typeof(SKTranslator))]
    public class SKTranslatorEditor : Editor
    {
        public SKTranslatorEditorState state;
        SKTranslator translator;
        private Transform previewContainer;
        private void OnDisable()
        {
           // ExitEdit();
        }
        public override void OnInspectorGUI()
        {
            translator = (SKTranslator)target;
            GUILayout.Label("Waypoint Editor");
            GUI.contentColor = new Color(0.9f, 0.8f, 0.7f);
            if (state == SKTranslatorEditorState.Idle && GUILayout.Button("<Edit Waypoint>"))
            {
                EnterEdit();
            }

            if (state == SKTranslatorEditorState.Editing && GUILayout.Button("<End Edit>"))
            {
                ExitEdit();
            }

            if (state == SKTranslatorEditorState.Editing && GUILayout.Button("<Add Waypoint>"))
            {
                SKTranslatorWaypoint wp = new SKTranslatorWaypoint()
                {
                    position = translator.GetLastWayPoint() == null ? (translator.transform.position+Vector3.right) : (translator.GetLastWayPoint().position+Vector3.right)
                };
                translator.AddWaypoint(wp);
            }
            GUI.contentColor = Color.white;
            base.OnInspectorGUI();
            UpdateWaypointPreview();
        }

        public void EnterEdit()
        {
            state = SKTranslatorEditorState.Editing;
            previewContainer = (new GameObject("Waypoints")).transform;
            previewContainer.SetParent(translator.transform);
            previewContainer.localPosition = Vector3.zero;
        }

        private void UpdateWaypointPreview()
        {
            if (state != SKTranslatorEditorState.Editing)
                return;

            previewContainer.ClearChildrenImmediate();
            for (int i = 0; i < translator.waypoints.Count; i++)
            {
                SKTranslatorWaypoint wp = translator.waypoints[i];
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.name = ("Waypoint_" + i);
                go.transform.SetParent(previewContainer);

                go.transform.position = wp.position;
            }
        }

        public void ExitEdit()
        {
            state = SKTranslatorEditorState.Idle;
            if(previewContainer != null)
            DestroyImmediate(previewContainer.gameObject);
        }
    }
    public enum SKTranslatorEditorState
    {
        Idle,
        Editing
    }
}
