using UnityEngine;
using System.Collections;
using UnityEditor;

namespace wararyo.EclairCueMaker
{
    [CustomEditor(typeof(StageGimmickBase),true)]//このtrueが大事っぽい！！！！こんなニッチな要求を想定してるなんて…Unity最高
    public class StageGimmickBaseInspector : Editor
    {
		CueEventBase[] cueEventList;

        StageGimmickBase stageGimmickBase;

        void OnEnable()
        {
            stageGimmickBase = (StageGimmickBase)target;
			if(stageGimmickBase.target) cueEventList = stageGimmickBase.target.GetComponents<CueEventBase>();

        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Cue");

            EditorGUI.BeginChangeCheck();

            var cueTarget = (GameObject)EditorGUILayout.ObjectField("Target", stageGimmickBase.target,typeof(GameObject),true);

			string cueEventID = "";
			string param = "";

			if (cueEventList != null) {
				if (cueEventList.Length != 0) {
					cueEventID = cueEventList [EditorGUILayout.Popup (CueDrawer.getIndexFromID (cueEventList, stageGimmickBase.cueEventID), CueDrawer.getCueEventsStrings (cueEventList))].EventID;
					EditorGUILayout.LabelField ("","");
					param = CueDrawer.CueEventParamGUI (GUILayoutUtility.GetLastRect(), stageGimmickBase.parameter, cueEventList[CueDrawer.getIndexFromID (cueEventList, stageGimmickBase.cueEventID)].ParamType);
				}
			}

            if (EditorGUI.EndChangeCheck())
            {

                //変更前に Undo に登録
                Undo.RecordObject(stageGimmickBase, "Change Cue");

                stageGimmickBase.target = cueTarget;
				stageGimmickBase.cueEventID = cueEventID;
				stageGimmickBase.parameter = param;

				cueEventList = cueTarget.GetComponents<CueEventBase>();
				//Debug.Log ("cueEventID"+cueEventID);
            }

			EditorGUILayout.Space ();

            base.OnInspectorGUI();
        }
    }
}
