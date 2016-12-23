using UnityEngine;
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
				var gameObjectIDProperty = property.FindPropertyRelative ("gameObjectID");
				var gameObjectProperty = property.FindPropertyRelative ("gameObject");
                var cueEventNameProperty = property.FindPropertyRelative("cueEventName");
                var cueEventParamProperty = property.FindPropertyRelative("parameter");

                //GUIを配置
                EditorGUI.LabelField (timeLabelRect, "Duration");
				timeFieldProperty.floatValue = EditorGUI.FloatField (timeFieldRect, timeFieldProperty.floatValue);

                GameObject go = (GameObject)EditorGUI.ObjectField(gameObjectRect, EditorUtility.InstanceIDToObject(gameObjectIDProperty.intValue), typeof(GameObject),true);
                try
                {
                    if (gameObjectIDProperty.intValue != go.GetInstanceID())
                    {
                        gameObjectIDProperty.intValue = go.GetInstanceID();
                        gameObjectProperty.objectReferenceValue = go;
                    }
                    cueEventList = go.GetComponents<CueEventBase>();
                    EditorGUI.Popup(cueEventPopupRect, 0, getCueEventsStrings(cueEventList));
                    CueEventParamGUI(cueEventParamRect,"",typeof(string));
                }
                catch
                {
                    gameObjectIDProperty.intValue = -1;
                    gameObjectProperty.objectReferenceValue = null;
                    cueEventList = null;
                }


			}
		}

        private object CueEventParamGUI(Rect rect,object param,System.Type type)
        {
            if (type.Equals(typeof(string)))
            {
                return EditorGUI.TextField(rect, (string)param);
            }
            else if (type.IsSubclassOf(typeof(Object)))
            {
                return EditorGUI.ObjectField(rect, (Object)param, type);//TODO:このままじゃ多分シーン上のオブジェクトを指定できない
            }
            else
            {
                return null;
            }
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
        public static int getIndexFromStrings(string[] sts) { return 0; }
	}
}