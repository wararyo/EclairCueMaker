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
				if(value) OnCueSceneChanged ();
			}
		}

		private SerializedProperty cueListSerialized;

		private bool isEditable = false;

        private ReorderableList rawList;

		const string MessageWhenUnEditable = "GameObjectにCueScenePlayerをアタッチし、CueSceneを指定すると編集できます。";

        [MenuItem("Window/EclairCueEditor")]
        static void Open()
        {
            var window = EditorWindow.GetWindow<CueSceneEditor>("CueEditor");
			string iconPath = AssetDatabase.GUIDToAssetPath ("40470457539b24899825bad08fdb5ed1");
			Debug.Log (iconPath);
			var icon = AssetDatabase.LoadAssetAtPath<Texture> (iconPath);

			window.titleContent = new GUIContent ("CueEditor", icon);
        }

        void OnEnable()
        {
            //cueList = new ReorderableList(seria, Cue);
			Selection.selectionChanged = OnSelectionChanged;
			OnSelectionChanged ();
        }

		void OnSelectionChanged(){
			isEditable = false;
			foreach(GameObject go in Selection.gameObjects)
			{
				if (go.activeInHierarchy && go.GetComponent<CueScenePlayer>()) {
					cueScene = go.GetComponent<CueScenePlayer> ().cueScene;//OnCueSceneChangedが実行されるね
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
            cueListSerialized = new SerializedObject(cueScene).FindProperty("cueList");

            rawList = new ReorderableList(cueListSerialized.serializedObject, cueListSerialized);
            rawList.elementHeight = 38;

            rawList.drawHeaderCallback = (rect) => {
                EditorGUI.LabelField(rect, "CueList");
            };

            rawList.drawElementCallback = (rect, index, isActive, isFocused) => {
                var cueSerialized = cueListSerialized.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, cueSerialized,true);
            };

            rawList.onAddCallback = (list) => {
                if (list.count == 0 || list.index < 0)
                {
                    cueScene.cueList.Add(new Cue());
                    cueListSerialized.arraySize++;
                }
                else {
                    cueScene.cueList.Insert(list.index + 1, new Cue(cueScene.cueList[list.index]));
                    cueListSerialized.arraySize++;
                    list.index++;
                }
            };
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
				cueListSerialized.serializedObject.Update ();
				// リスト・配列の変更可能なリストの表示
				rawList.DoLayoutList();
				cueListSerialized.serializedObject.ApplyModifiedProperties();
            }
        }

        private void toolbarSpace(int width = 6)
        {
			//頭悪い
            using (new EditorGUI.DisabledScope(true)) GUILayout.Button("", EditorStyles.toolbarButton, GUILayout.Width(width));
        }
    }
}
