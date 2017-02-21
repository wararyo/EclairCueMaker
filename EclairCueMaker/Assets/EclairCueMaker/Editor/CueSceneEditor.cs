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
        private int tab = 1;
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
		private Vector2 rawScrollPos;

		private Vector2 timelineScrollPos;

		const float timelineZoomFactorMin = 1;
		const float timelineZoomFactorMax = 60;
        private float timelineZoomFactor_ = 8.0f;//単位はsec
		private float timelineZoomFactor{
			get{
				return timelineZoomFactor_;
			}
			set{
				timelineZoomFactor_ = Mathf.Clamp (value, timelineZoomFactorMin, timelineZoomFactorMax);
			}
		}

        private float timelineTimeMax = 120;

        private List<string> selectedCueList;
        private Vector2 cueIconDragStartedMousePos;
        private bool dragEndFlag = false;

        private float timelineSnapSpan = 0.05f;//タイムラインエディタでドラッグした時の吸着

		const string MessageWhenUnEditable = "GameObjectにCueScenePlayerをアタッチし、CueSceneを指定すると編集できます。";

        [System.NonSerialized]
        GUIStyle lockButtonStyle;
        [System.NonSerialized]
        bool locked = false;

        [MenuItem("Window/EclairCueEditor")]
        static void Open()
        {
            var window = EditorWindow.GetWindow<CueSceneEditor>("CueEditor");
			string iconPath = AssetDatabase.GUIDToAssetPath ("40470457539b24899825bad08fdb5ed1");
			Debug.Log (iconPath);
			var icon = AssetDatabase.LoadAssetAtPath<Texture> (iconPath);

			window.titleContent = new GUIContent ("CueEditor", icon);

            window.minSize = new Vector2(512, 256);
        }

        void OnEnable()
        {
            //cueList = new ReorderableList(seria, Cue);
            Selection.selectionChanged = OnSelectionChanged;
            EclairGUILayout.CueIconDragStart += OnCueIconDragStart;
            EclairGUILayout.CueIconDragEnd += OnCueIconDragEnd;
			OnSelectionChanged ();
        }

        void ShowButton(Rect position)
        {
            if (lockButtonStyle == null)
                lockButtonStyle = "IN LockButton";
            locked = GUI.Toggle(position, locked, GUIContent.none, lockButtonStyle);
        }

        void OnSelectionChanged(){
            if (locked) return;
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
            selectedCueList = new List<string>();

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
                    //cueListSerialized.arraySize++;
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
				var horizontalScrollbarRect = new Rect (paneWidth - 1, position.height - 15, position.width - paneWidth + 1 - 30, 15);
				var zoomInButtonRect = new Rect (position.width - 30, position.height - 15, 15, 15);
				var zoomOutButtonRect = new Rect (position.width - 15, position.height - 15, 15, 15);
                timelineScrollPos.x = GUI.HorizontalScrollbar(horizontalScrollbarRect, timelineScrollPos.x, timelineZoomFactor, -0.2f, timelineTimeMax);
				if (GUI.Button (zoomInButtonRect, "+", "OL Plus"))
					timelineZoomFactor -= 1.0f;
				if (GUI.Button (zoomOutButtonRect, "-", "OL Minus"))
					timelineZoomFactor += 1.0f;

                float startTime = timelineScrollPos.x;
                float endTime = timelineScrollPos.x + timelineZoomFactor;

                using (var rulerScope = new EditorGUILayout.HorizontalScope("toolbar"))
                {
                    GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.Width(64));
                    toolbarSpace(paneWidth - 64 - 6);
                    //ここまで256px
					EclairGUILayout.Ruler(startTime, endTime);
                }

                //グリッド
                var timelineBackGroundRect = new Rect(paneWidth - 1, 36, position.width - paneWidth - 6 + 1, position.height - 36 - 15);
                EclairGUILayout.TimelineBackground(timelineBackGroundRect, startTime, endTime);
                //横スクロールでスクロール
				Event evt = Event.current;
				if (evt.type.Equals (EventType.ScrollWheel)) {
					if (evt.shift) {
						timelineZoomFactor += (evt.delta.x + evt.delta.y) / 4;
						Repaint ();
					} else {
						timelineScrollPos.x += (evt.delta.x + evt.delta.y) * timelineZoomFactor / 24;
						Repaint ();
					}
				}


				//Cueの削除
				if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Delete || Event.current.keyCode == KeyCode.Backspace)) {
					//Debug.Log ("aahahahahahhhahaa");
					Undo.RecordObject(cueScene,"Delete Cue");
					foreach (string ID in selectedCueList) {
						//Debug.Log (ID);
						var cue = cueScene.cueList.FindIndex(x => x.UUID == ID);
						Debug.Log (cue);
						cueScene.cueList.RemoveAt(cue);
					}
				}

				cueListSerialized.serializedObject.Update ();

                var absoluteCueList = CueListUtil.GenerateAbsoluteCueList(cueListSerialized);

                //CueIconのドラッグ
                if (EclairGUILayout.isCueIconDragging)
                {
                    float delta = (Event.current.mousePosition.x - cueIconDragStartedMousePos.x) * getTimePerPixel(timelineBackGroundRect.width,timelineZoomFactor);
                    foreach(string ID in selectedCueList)
                    {
                        var acue = absoluteCueList.Find(x => x.Value.FindPropertyRelative("UUID").stringValue == ID);
                        int index = absoluteCueList.IndexOf(acue);
                        absoluteCueList.Remove(acue);
						absoluteCueList.Insert(index, new KeyValuePair<float, SerializedProperty>((int)((acue.Key + delta + timelineSnapSpan/2)/timelineSnapSpan) * timelineSnapSpan, acue.Value));
                    }
                }

                //タイムライントラック描画
                List<string> trackList = new List<string>();
                foreach (var acue in absoluteCueList)
                {
                    bool isSelectionChanged = false;
					string gameObjectName = acue.Value.FindPropertyRelative ("gameObjectName").stringValue;
					if (!trackList.Exists(st => st == gameObjectName))
                    {
						trackList.Add(gameObjectName);
                        var selectedCueListTemp = EclairGUILayout.TimelineTrack(absoluteCueList, gameObjectName, paneWidth, (trackList.Count % 2) > 0, startTime, endTime, selectedCueList);
                        if (selectedCueListTemp.Equals( selectedCueList))
                        {
                            selectedCueList = selectedCueListTemp;
                            isSelectionChanged = true;
                        }
                    }
                    if (isSelectionChanged) Repaint();
                    else
                    {
                        if (Event.current.type == EventType.MouseUp)
                        {
                            //Debug.Log("weiwei");
                            Rect rect = new Rect(0, 0, paneWidth, position.height);
                            if (rect.Contains(Event.current.mousePosition))
                            {
                                selectedCueList.Clear();
                                Repaint();
                            }
                        }
                    }
                } 

				if (Event.current.type == EventType.MouseUp) {
					if (EclairGUILayout.isCueIconDragging)
					{
						EclairGUILayout.isCueIconDragging = false;
						EclairGUILayout.CueIconDragEnd();
					}
				}

                if (dragEndFlag)
                {
                    dragEndFlag = false;
					Undo.RecordObject(cueScene,"Edit CueScene");
                    cueScene.cueList = CueListUtil.GenerateCueListFromAbsolute(absoluteCueList);
                    //cueListSerialized = new SerializedObject(cueScene).FindProperty("cueList");
                    cueListSerialized.serializedObject.ApplyModifiedProperties();
                }
            }


            else//Rawタブ
            {
				cueListSerialized.serializedObject.Update ();
				// リスト・配列の変更可能なリストの表示
				using (var scrollView = new EditorGUILayout.ScrollViewScope(rawScrollPos)) {
					rawScrollPos = scrollView.scrollPosition;
					rawList.DoLayoutList ();
				}
				cueListSerialized.serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnCueIconDragStart()
        {
            wantsMouseMove = true;
            cueIconDragStartedMousePos = Event.current.mousePosition;
			if (Event.current.alt) {
				var beforeSelection = new List<string>(selectedCueList);
				selectedCueList.Clear ();
				foreach (string uuid in beforeSelection) {
					Cue cue = cueScene.cueList.Find (x => x.UUID == uuid);
					int index = cueScene.cueList.IndexOf (cue);
					Cue copy = new Cue (cue){time = 0};
					cueScene.cueList.Insert (index+1,copy);
					selectedCueList.Add (copy.UUID);
				}
			}
        }
        private void OnCueIconDragEnd()
        {
            wantsMouseMove = false;
            dragEndFlag = true;
        }

        /// <summary>
        /// 画面上の1ピクセルが何秒に相当するかを求めます。
        /// </summary>
        /// <returns></returns>
        private static float getTimePerPixel(float width, float zoomFactor)
        {
            return zoomFactor / width;
        }

        private void toolbarSpace(int width = 6)
        {
			//頭悪い
            using (new EditorGUI.DisabledScope(true)) GUILayout.Button("", EditorStyles.toolbarButton, GUILayout.Width(width));
        }
    }
}
