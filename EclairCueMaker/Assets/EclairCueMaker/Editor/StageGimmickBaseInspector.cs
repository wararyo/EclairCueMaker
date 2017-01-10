using UnityEngine;
using System.Collections;
using UnityEditor;

namespace wararyo.EclairCueMaker
{
    [CustomEditor(typeof(StageGimmickBase),true)]//このtrueが大事っぽい！！！！こんなニッチな要求を想定してるなんて…Unity最高
    public class StageGimmickBaseInspector : Editor
    {


        StageGimmickBase stageGimmickBase;

        void OnEnable()
        {
            stageGimmickBase = (StageGimmickBase)target;

        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Cue");

            EditorGUI.BeginChangeCheck();

            var cueTarget = (GameObject)EditorGUILayout.ObjectField("Target", stageGimmickBase.target,typeof(GameObject),true);

            if (EditorGUI.EndChangeCheck())
            {

                //変更前に Undo に登録
                Undo.RecordObject(stageGimmickBase, "Change Cue");

                stageGimmickBase.target = cueTarget;

            }

            base.OnInspectorGUI();
        }
    }
}
