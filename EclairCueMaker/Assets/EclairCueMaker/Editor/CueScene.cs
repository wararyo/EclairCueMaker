using UnityEngine;
using UnityEditor;
using System.Collections;

namespace wararyo.EclairCueMaker
{

    public class CueScene : ScriptableObject
    {

        [SerializeField]
        string st = "hoge";

        [SerializeField]
        Cue[] cueList;

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
    }

    [CustomEditor(typeof(CueScene))]
    public class CueSceneInspector : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("セイクで", "優勝");
        }
    }
}
