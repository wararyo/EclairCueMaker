using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace wararyo.EclairCueMaker
{
	/// <summary>
	/// CueSceneを編集するウィンドウです。
	/// </summary>
    public class CueSceneEditor : EditorWindow
    {
        private int tab = 0;
        private int paneWidth = 192;//timelineの左側の幅

		private string sceneName = "";
		private CueScene cueScene_;
		private CueScene cueScene{
			get{
				return cueScene_;
			}
			set{
				cueScene_ = value;
				OnCueSceneChanged ();
			}
		}

		private bool isEditable = false;

        private ReorderableList rawList;

		const string MessageWhenUnEditable = "GameObjectにCueScenePlayerをアタッチし、CueSceneを指定すると編集できます。";

        [MenuItem("Window/EclairCueEditor")]
        static void Open()
        {
            EditorWindow.GetWindow<CueSceneEditor>("CueEditor");
        }

        void OnEnable()
        {
            //cueList = new ReorderableList(seria, Cue);
			Selection.selectionChanged = OnSelectionChanged;
			OnSelectionChanged ();

			rawList = new ReorderableList (cueScene.cueList,typeof(List<Cue>));

			rawList.drawHeaderCallback = (rect) => {
				EditorGUI.LabelField(rect, "CueList");
			};

			rawList.drawElementCallback = (rect, index, isActive, isFocused) => {
				Cue cue = cueScene.cueList[index];
				EditorGUI.BeginChangeCheck ();

				var time = EditorGUILayout.FloatField ("Time", cue.time);
				var gameObject = EditorGUILayout.ObjectField(cue.gameObject,typeof(GameObject),true);

				if (EditorGUI.EndChangeCheck ()) {

					//変更前に Undo に登録
					//Undo.RecordObject (cue, "Change Cue time");

					//cue.time = time;

				}
			};
        }

		void OnSelectionChanged(){
			isEditable = false;
			foreach(GameObject go in Selection.gameObjects)
			{
				if (go.activeInHierarchy && go.GetComponent<CueScenePlayer>()) {
					cueScene = go.GetComponent<CueScenePlayer> ().cueScene;
					if (cueScene) {
						isEditable = true;
						sceneName = go.name + "#" + cueScene.name;
						Repaint ();
						return;
					}
				}
			}
			if (sceneName != "") {//選択が解除された時
				sceneName = "";
				Repaint ();
			}
		}

		void OnCueSceneChanged(){

		}

        void OnGUI()
        {
            using (var horizontalScope = new EditorGUILayout.HorizontalScope("toolbar"))
            {
                tab = GUILayout.Toolbar(tab, new string[] { "Timeline", "Raw" }, EditorStyles.toolbarButton, GUILayout.Width(180));
                //GUILayout.Space(20);
                toolbarSpace();//こうしないとRawボタンの右側の線が表示されない
                using (new EditorGUI.DisabledScope(true)) GUILayout.Button("Play", EditorStyles.toolbarButton,GUILayout.Width(64));
                EditorGUILayout.Space();
				GUIStyle rightLabelStyle = new GUIStyle(GUI.skin.label);
				rightLabelStyle.alignment = TextAnchor.MiddleRight;
				rightLabelStyle.normal.textColor = new Color (.5f, .5f, .5f, 1);
				EditorGUILayout.LabelField (sceneName,rightLabelStyle);
            }

			if (!isEditable) {
				GUIStyle centerLabelStyle = new GUIStyle(GUI.skin.label);
				centerLabelStyle.alignment = TextAnchor.MiddleCenter;
				//centerLabelStyle.normal.textColor = new Color (0, 0, 0,1 );
				EditorGUILayout.LabelField (MessageWhenUnEditable,centerLabelStyle,GUILayout.Height(128));
				return;
			}


            if (tab == 0)//Timelineタブ
            {
                using (var rulerScope = new EditorGUILayout.HorizontalScope("toolbar"))
                {
                    GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.Width(64));
                    toolbarSpace(paneWidth - 64 - 6);
                    //ここまで256px
                    EclairGUILayout.Ruler(3.95f, 5.55f);
                }
            }


            else//Rawタブ
            {
				// リスト・配列の変更可能なリストの表示
				rawList.DoLayoutList();
            }
        }

        private void toolbarSpace(int width = 6)
        {
			//頭悪い
            using (new EditorGUI.DisabledScope(true)) GUILayout.Button("", EditorStyles.toolbarButton, GUILayout.Width(width));
        }
    }
}
