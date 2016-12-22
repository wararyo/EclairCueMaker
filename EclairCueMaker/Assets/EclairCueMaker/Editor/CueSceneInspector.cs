using UnityEngine;
using UnityEditor;
using System.Collections;

namespace wararyo.EclairCueMaker
{

    [CustomEditor(typeof(CueScene))]
    public class CueSceneInspector : Editor
    {
        CueScene cueScene = null;

		//[MenuItem("Assets/Create/EclairCueScene")]//CueScene.csにてCreateAssetMenuAttributeを使う方法に変更
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

        void OnEnable()
        {
            cueScene = (CueScene)target;
        }

		const string message = 
			"Animatorなどと同じように、シーン上のゲームオブジェクトにCueScenePlayerをアタッチしてこのCueSceneをセット、その後アタッチしたゲームオブジェクトを選択することで編集可能になります。";
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
			EditorGUILayout.LabelField("EclairCueMaker CueScene");
            string sceneGUID = cueScene.attachedSceneGUID;
            EditorGUILayout.LabelField("Attached in " + (sceneGUID == "" ? "Nothing" : System.IO.Path.GetFileNameWithoutExtension( AssetDatabase.GUIDToAssetPath(sceneGUID))));
			EditorGUILayout.LabelField ("CueCount:", cueScene.Count + "");
			EditorGUILayout.LabelField ("Duration:", cueScene.Length + "s");
			EditorGUILayout.HelpBox (message, MessageType.Info);
        }
    }
}
