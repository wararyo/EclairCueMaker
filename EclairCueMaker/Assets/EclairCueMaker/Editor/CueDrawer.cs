using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace wararyo.EclairCueMaker
{
	[CustomPropertyDrawer(typeof(Cue))]
	public class CueDrawer : PropertyDrawer
	{
        //GameObject go;
        private string[] cueEventList = null;

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

				//各プロパティーの SerializedProperty を求める
				var timeFieldProperty = property.FindPropertyRelative ("time");
				var gameObjectIDProperty = property.FindPropertyRelative ("gameObjectID");
				var gameObjectProperty = property.FindPropertyRelative ("gameObject");
                var cueEventNameProperty = property.FindPropertyRelative("cueEventName");
                var cueEventParamTypeProperty = property.FindPropertyRelative("paramType");
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
                }
                catch
                {
                    gameObjectIDProperty.intValue = -1;
                    gameObjectProperty.objectReferenceValue = null;
                }
			}
		}

        public static string[] getCueEvents(GameObject gameObject)
        {
            List<string> list = new List<string>();
            foreach (CueEventBase c in gameObject.GetComponents<CueEventBase>()) ;

            return list.ToArray();
        }
	}
}