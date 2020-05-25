using UnityEditor;
using UnityEngine;

namespace Febucci.UI.Core
{
#if UNITY_EDITOR

    [CustomEditor(typeof(TAnimGlobalDataScriptable))]
    class TAnimGlobalDataScriptableDrawer : Editor
    {

        [MenuItem("Window/TextAnimator/Locate GlobalData")]
        public static void LocateGlobalData()
        {
            var foundData = Resources.Load(TAnimGlobalDataScriptable.resourcesPath);
            if (foundData != null)
            {
                Selection.activeObject = foundData;
            }
            else
            {
                Debug.LogWarning($"Text Animator: No Scriptable data found, please create one in path {TAnimGlobalDataScriptable.resourcesPath}");
            }
        }

        TextAnimatorDrawer.Show current;
        TAnimGlobalDataScriptable script;

        SerializedProperty behaviorPresets;
        SerializedProperty appearancesPresets;

        TextAnimatorDrawer.UserPresetDrawer[] behaviorDrawers = new TextAnimatorDrawer.UserPresetDrawer[0];
        TextAnimatorDrawer.UserPresetDrawer[] appearancesDrawers = new TextAnimatorDrawer.UserPresetDrawer[0];

        protected virtual void OnEnable()
        {
            behaviorPresets = serializedObject.FindProperty("globalBehaviorPresets");
            appearancesPresets = serializedObject.FindProperty("globalAppearancePresets");
            script = (TAnimGlobalDataScriptable)target;


            Undo.undoRedoPerformed += Redo;

        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= Redo;
        }

        void Redo()
        {
            serializedObject.UpdateIfRequiredOrScript(); //I have spent too much searching this method... :(
            Repaint();
            TryResettingTextAnimators();
        }

        bool fake = true;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField($"Drawing Global Effects");

            if (Application.isPlaying)
                EditorGUILayout.LabelField($"[!!] Remember: Saves are applied in play mode.");

            //Chooses what to show 
            TextAnimatorDrawer.WhatToShowButton(ref current);

            fake = true;
            switch (current)
            {
                case TextAnimatorDrawer.Show.Behaviors:

                    TextAnimatorDrawer.ShowPresets(ref behaviorDrawers, ref fake, ref behaviorPresets, false);

                    break;

                case TextAnimatorDrawer.Show.Appearances:

                    TextAnimatorDrawer.ShowPresets(ref appearancesDrawers, ref fake, ref appearancesPresets, true);

                    break;
            }

            if (serializedObject.hasModifiedProperties)
            {
                //Repaint();

                //Undo.RecordObject(serializedObject.targetObject, "Changed TextAnimator Global Data Scriptable");
                Undo.RecordObject(script, "Changed TextAnimator Global Data Scriptable");
                EditorUtility.SetDirty(script);

                //Undo.RegisterCompleteObjectUndo(script, "Changed TextAnimator Global Data Scriptable");
                serializedObject.ApplyModifiedProperties();

                Repaint();
                TryResettingTextAnimators();
            }

        }

        void TryResettingTextAnimators()
        {
            if (EditorApplication.isPlaying)
            {
                TAnim_EditorHelper.TriggerEvent();
            }
        }
    }

#endif


}