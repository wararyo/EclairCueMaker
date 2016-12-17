﻿using UnityEngine;
using UnityEditor;
using System.Collections;

namespace wararyo.EclairCueMaker
{

    [CustomEditor(typeof(CueScene))]
    public class CueSceneInspector : Editor
    {

		[MenuItem("Assets/Create/EclairCueScene")]
		public static void CreateCueSceneInstance()
		{
			CueScene cs = CreateInstance<CueScene>();

			AssetDatabase.CreateAsset(cs, GetSelectedFolderPath("Assets/NewCueScene.asset"));
		}

		private static string GetSelectedFolderPath(string whenNone)
		{
			// ファイルが選択されている時.
			/*if (Selection.assetGUIDs != null && Selection.assetGUIDs.Length > 0)
	            {
	                return AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
	            }
	            else */
			return whenNone;
		}

		const string message = 
			"Animatorなどと同じように、シーン上のゲームオブジェクトにCueScenePlayerをアタッチしてこのCueSceneをセット、その後アタッチしたゲームオブジェクトを選択することで編集可能になります。";
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
			EditorGUILayout.LabelField("EclairCueMaker CueScene");
			EditorGUILayout.LabelField ("CueCount:", "8");
			EditorGUILayout.LabelField ("Duration:", "10.0s");
			EditorGUILayout.HelpBox (message, MessageType.Info);
        }
    }
}
