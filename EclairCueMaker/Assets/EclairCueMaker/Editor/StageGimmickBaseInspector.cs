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

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(stageGimmickBase, "Change Cue Target");
                stageGimmickBase.target = cueTarget;
                cueEventList = cueTarget.GetComponents<CueEventBase>();
            }
            EditorGUI.BeginChangeCheck();


            string cueEventID = "";
			string param = "";
            Object paramObject = null;

			if (cueEventList != null) {
				if (cueEventList.Length != 0) {
					cueEventID = cueEventList [EditorGUILayout.Popup (CueDrawer.getIndexFromID (cueEventList, stageGimmickBase.cueEventID), CueDrawer.getCueEventsStrings (cueEventList))].EventID;
					EditorGUILayout.LabelField ("","");
                    CueEventBase cueEvent = cueEventList[CueDrawer.getIndexFromID(cueEventList, stageGimmickBase.cueEventID)];

                    param = CueDrawer.CueEventParamGUI (GUILayoutUtility.GetLastRect(), stageGimmickBase.parameter, cueEvent.ParamType);
                    if (cueEvent.ParamType.IsSubclassOf(typeof(Object))){
                        paramObject = (Object)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(param), typeof(Object));
                    }
				}
			}

            if (EditorGUI.EndChangeCheck())
            {

                //変更前に Undo に登録
                Undo.RecordObject(stageGimmickBase, "Change Cue");

				stageGimmickBase.cueEventID = cueEventID;
				stageGimmickBase.parameter = param;
                stageGimmickBase.paramObject = paramObject;
				//Debug.Log ("cueEventID"+cueEventID);
            }

			EditorGUILayout.Space ();

            base.OnInspectorGUI();
        }

        void OnSceneGUI()
        {
            if (stageGimmickBase.target.GetComponent<CueScenePlayer>()) return;
            Vector3 start = stageGimmickBase.GetComponent<Transform>().position;
            Vector3 end = stageGimmickBase.target.GetComponent<Transform>().position;
            Handles.color = new Color(1, 0.8f, 0, 1f);
            Handles.DrawDottedLine(start, end, 4);
            Handles.color = new Color(1, 0.8f, 0, 0.5f);
            Handles.SphereCap(0, end, Quaternion.LookRotation(end - start), 1);
        }
    }
}
