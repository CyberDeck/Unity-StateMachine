using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Secretlab.StateMachine.Editor {

    [CustomEditor(typeof(TimedStateMachineMonoBehaviour), true)]
    public class StateMachineEditor: UnityEditor.Editor {
        bool showStateInformation = false;

        public override bool RequiresConstantRepaint() {
            if (showStateInformation && EditorApplication.isPlaying) {
                return true;
            }
            return base.RequiresConstantRepaint();
        }

        public override void OnInspectorGUI() {
            // Show default inspector property editor
            DrawDefaultInspector();

            if (EditorApplication.isPlaying) {
                showStateInformation = EditorGUILayout.BeginFoldoutHeaderGroup(showStateInformation, "State Machine Status");
                if (showStateInformation) {
                    TimedStateMachineMonoBehaviour tsm = (TimedStateMachineMonoBehaviour)target;
                    EditorGUILayout.LabelField("State:", tsm.editorStateName);
                    EditorGUILayout.LabelField("Delay:", tsm.editorStateDelay.ToString());
                    EditorGUILayout.HelpBox("Displaying this information may impact your performance.", MessageType.Info);
                }
            }

        }
    }

}
