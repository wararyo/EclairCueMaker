using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace wararyo.EclairCueMaker
{
	[CustomPropertyDrawer(typeof(Cue))]
	public class CueDrawer : PropertyDrawer
	{
        //GameObject go;

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
				var gameObjectProperty = property.FindPropertyRelative ("gameObjectID");
				//gameObjectProperty.

				//GUIを配置
				EditorGUI.LabelField (timeLabelRect, "Duration");
				timeFieldProperty.floatValue = EditorGUI.FloatField (timeFieldRect, timeFieldProperty.floatValue);
                GameObject go = (GameObject)EditorGUI.ObjectField(gameObjectRect, EditorUtility.InstanceIDToObject(gameObjectProperty.intValue), typeof(GameObject));
                 if(go) gameObjectProperty.intValue = go.GetInstanceID();
                //gameObjectProperty.objectReferenceValue = EditorGUI.ObjectField (gameObjectRect, gameObjectProperty.objectReferenceValue,typeof(GameObject),true);
                //gameObjectProperty.
                //go = (GameObject)EditorGUI.ObjectField(gameObjectRect, , typeof(GameObject));
				//EditorGUI.ObjectField(gameObjectRect, gameObjectProperty,typeof(GameObject),label);
			}
		}
	}
}