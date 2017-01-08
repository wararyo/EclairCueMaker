﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace wararyo.EclairCueMaker
{
	[CustomPropertyDrawer(typeof(Cue))]
	public class CueDrawer : PropertyDrawer
	{
        //GameObject go;
        private CueEventBase[] cueEventList = null;

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			//base.OnGUI (position, property, label);
			using (new EditorGUI.PropertyScope (position, label, property)) {

				//各パーツの領域を指定
				var timeLabelRect = new Rect (position) {
					width = 56,
					height = 16,
					x = position.x + 2,
					y = position.y + 2,
				};
				var timeFieldRect = new Rect (position) {
					width = 56,
					height = 16,
					x = position.x + 2,
					y = position.y + 19,
				};

				var gameObjectRect = new Rect (position) {
					width = 156,
					height = 16,
					x = position.x + 64,
					y = position.y + 2
				};

                var cueEventPopupRect = new Rect(position)
                {
                    width = 160,
                    height = 16,
                    x = position.x + 224,
                    y = position.y + 2
                };

                var cueEventParamRect = new Rect(position)
                {
                    width = 320,
                    height = 16,
                    x = position.x + 64,
                    y = position.y + 19
                };

                //各プロパティーの SerializedProperty を求める
                var timeFieldProperty = property.FindPropertyRelative ("time");
				var gameObjectNameProperty = property.FindPropertyRelative ("gameObjectName");
                var cueEventIDProperty = property.FindPropertyRelative("cueEventID");
                var cueEventParamProperty = property.FindPropertyRelative("parameter");

                //GUIを配置
                EditorGUI.LabelField (timeLabelRect, "Duration");
				timeFieldProperty.floatValue = EditorGUI.FloatField (timeFieldRect, timeFieldProperty.floatValue);

                GameObject go = (GameObject)EditorGUI.ObjectField(gameObjectRect, GameObject.Find(gameObjectNameProperty.stringValue), typeof(GameObject),true);
                try
                {
                    if (gameObjectNameProperty.stringValue != GetHierarchyPath(go.transform))
                    {
                        gameObjectNameProperty.stringValue = GetHierarchyPath(go.transform);
                    }
                    cueEventList = go.GetComponents<CueEventBase>();
                }
                catch
                {
                    gameObjectNameProperty.stringValue = "";
                    cueEventList = null;
                }
				if (cueEventList != null) {
					if (cueEventList.Length != 0) {
						cueEventIDProperty.stringValue = cueEventList [EditorGUI.Popup (cueEventPopupRect, getIndexFromID (cueEventList, cueEventIDProperty.stringValue), getCueEventsStrings (cueEventList))].EventID;
						string param = CueEventParamGUI (cueEventParamRect, cueEventParamProperty.stringValue, cueEventList[getIndexFromID (cueEventList, cueEventIDProperty.stringValue)].ParamType);
						/*if(cueEventList[getIndexFromID (cueEventList, cueEventIDProperty.stringValue)].ParamType.IsSubclassOf(typeof(Object))){
							if (param != cueEventParamProperty.stringValue) {
								cueEventParamObjectProperty.objectReferenceValue = EditorUtility.InstanceIDToObject (int.Parse (param));
							}
						}*/
						cueEventParamProperty.stringValue = param;
					}
				}
			}
		}

		private string CueEventParamGUI(Rect rect,string param,System.Type type)
        {
			if (type.Equals (typeof(void))) {
				return "";
			} else if (type.Equals (typeof(string))) {
				try {
					return EditorGUI.TextField (rect, param);
				} catch {
					return EditorGUI.TextField (rect, "");
				}
			} else if (type.Equals (typeof(int))) {
				try {
					return EditorGUI.IntField (rect, int.Parse (param)).ToString ();
				} catch {
					return EditorGUI.IntField (rect, 0).ToString ();
				}
			} else if (type.Equals (typeof(float))) {
				try { return EditorGUI.FloatField(rect, float.Parse(param)).ToString();}
				catch{
					return EditorGUI.FloatField (rect, 0).ToString();
				}
			}
            else if (type.Equals(typeof(GameObject)))
            {
				try{
					GameObject o = (GameObject)EditorGUI.ObjectField(rect, EditorUtility.InstanceIDToObject(int.Parse(param)), typeof(GameObject),true);
                    string path = AssetDatabase.GetAssetOrScenePath(o);
                    if(path == "")
                    {
                        return o.name;
                    }
                    else
                    {
                        return AssetDatabase.AssetPathToGUID(path);
                    }
                }
				catch{
					EditorGUI.ObjectField(rect, null, typeof(GameObject),true);
					return "";
				}
            }
			return "";
        }

        public static string[] getCueEventsStrings(CueEventBase[] events)
        {
            if (events == null) return null;

            string[] st = new string[events.Length];
            for(int i = 0;i < st.Length; i++)
            {
                st[i] = events[i].EventName;
            }
            return st;
        }
		public static int getIndexFromID(CueEventBase[] events,string ID) {
			if (events == null) return 0;

			for (int i = 0; i < events.Length; i++) {
				if (events [i].EventID == ID) {
					return i;
				}
			}
			return 0;
		}

        public string GetHierarchyPath(Transform self)
        {
            string path = self.gameObject.name;
            Transform parent = self.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }
    }
}