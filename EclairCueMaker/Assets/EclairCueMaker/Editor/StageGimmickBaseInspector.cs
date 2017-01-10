using UnityEngine;
using System.Collections;
using UnityEditor;

namespace wararyo.EclairCueMaker
{
    [CustomEditor(typeof(StageGimmickBase),true)]//このtrueが大事っぽい！！！！こんなニッチな要求を想定してるなんて…Unity最高
    public class StageGimmickBaseInspector : Editor
    {
        SerializedProperty targetProperty, cueEventIDProperty, parameterProerty;

        void OnEnable()
        {
            targetProperty = serializedObject.FindProperty("target");

        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Cue");

            base.OnInspectorGUI();
        }
    }
}
