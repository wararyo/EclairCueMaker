using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace wararyo.EclairCueMaker
{

    public class EclairGUILayout
    {

        #region Ruler
        public static void Ruler(float startTime, float endTime, params GUILayoutOption[] options)
        {
            using (new EditorGUI.DisabledScope(true)) GUILayout.Button("", EditorStyles.toolbarButton, options);
            //EditorGUILayout.LabelField("", "", EditorStyles.toolbar, options);
            var rect = GUILayoutUtility.GetLastRect();//既存のGUILayoutを実行しRectだけ盗む
            EclairGUILayout.Ruler(rect, startTime, endTime);
        }

        public static void Ruler(float startTime, float endTime)
        {
            using (new EditorGUI.DisabledScope(true)) GUILayout.Button("", EditorStyles.toolbarButton);
            var rect = GUILayoutUtility.GetLastRect();//既存のGUILayoutを実行しRectだけ盗む
            EclairGUILayout.Ruler(rect, startTime, endTime);
        }

        public static void Ruler(Rect rect, float startTime, float endTime)
        {
            if (rect.height <= 1.0f) return;

            var id = GUIUtility.GetControlID(FocusType.Keyboard, rect);

            Texture2D tex = new Texture2D((int)rect.width, (int)rect.height);
            tex.alphaIsTransparency = true;
            tex.filterMode = FilterMode.Point;

            Color[] transparent = new Color[tex.width * tex.height];
            for (int i = 0; i < transparent.Length; i++)
            {
                transparent[i] = new Color(0, 0, 0, 0);
            }
            tex.SetPixels(0, 0, tex.width, tex.height, transparent);

            //目盛りと数字の描画開始
            for (float i = ((int)(startTime * 10)) / (float)10; i < endTime; i += 0.1f)//目盛り単位0.1秒
            {
                if (i <= -0.1) continue;
                i = (int)(i * 10 + 0.5f) / (float)10;
                if (i < startTime) continue;
                int x = (int)Mathf.Lerp(0, rect.width, (i - startTime) / (endTime - startTime));
                int height = 4;
                if (((int)(i * 10 + 0.5f)) % 10 == 0)
                {
                    height = 12;
                    GUI.Label(new Rect(rect.x + x + 2, rect.y - 2, 32, 16), i + "");
                }
                else if ((int)(i * 10 + 0.5f) % 5 == 0) height = 6;
                tex = drawScale(tex, x, height, new Color(0, 0, 0, 1));

            }
            tex.Apply();
            GUI.DrawTexture(rect, tex);
        }

        /// <summary>
        /// Texture2Dに目盛りを書きます。
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="x"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        static Texture2D drawScale(Texture2D tex, int x, int height, Color color)
        {
            for (int i = 0; i < height; i++) {
                tex.SetPixel(x, i, color);
            }
            return tex;
        }

        #endregion

        #region TimelineBackground

        public static void TimelineBackground(Rect rect, float startTime, float endTime)
        {
            if (rect.height == 1.0f) return;

            var id = GUIUtility.GetControlID(FocusType.Keyboard, rect);

            Texture2D tex = new Texture2D((int)rect.width, (int)1);
            tex.alphaIsTransparency = true;
            tex.filterMode = FilterMode.Point;

            Color[] transparent = new Color[tex.width * tex.height];
            for (int i = 0; i < transparent.Length; i++)
            {
                transparent[i] = new Color(0, 0, 0, 0);
            }
            tex.SetPixels(0, 0, tex.width, tex.height, transparent);

            tex.SetPixel(0, 0, new Color(0, 0, 0, 0.3f));

            //目盛りと数字の描画開始
            for (float i = ((int)(startTime * 10)) / (float)10; i < endTime; i += 0.1f)//目盛り単位0.1秒
            {
                if (i <= -0.1) continue;
                i = (int)(i * 10 + 0.5f) / (float)10;
                if (i < startTime) continue;
                int x = (int)Mathf.Lerp(0, rect.width, (i - startTime) / (endTime - startTime));
                float opacity = 0.1f;
                if (((int)(i * 10 + 0.5f)) % 10 == 0)
                {
                    opacity = 0.2f;
                }
                else if ((int)(i * 10 + 0.5f) % 5 == 0) opacity = 0.2f;
                tex = drawGrid(tex, x, new Color(0, 0, 0, opacity));

            }
            tex.Apply();
            GUI.DrawTexture(rect, tex);
        }

        /// <summary>
        /// Texture2Dに縦線を書きます。
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="x"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        static Texture2D drawGrid(Texture2D tex, int x, Color color)
        {
            for (int i = 0; i < tex.height; i++)
            {
                tex.SetPixel(x, i, color);
            }
            return tex;
        }

        #endregion

        #region TimelineTrack

		public static List<KeyValuePair<float, SerializedProperty>> TimelineTrack(List<KeyValuePair<float, SerializedProperty>> absoluteCueList,string gameObjectPath,int paneWidth,bool isOddRaw,float startTime,float endTime)
        {
            if (absoluteCueList == null) return null;
            EditorGUILayout.LabelField("");
            var rect = GUILayoutUtility.GetLastRect();//既存のGUILayoutを実行しRectだけ盗む
            rect = new Rect(rect.x - 1, rect.y - 1, rect.width + 2, rect.height + 2);
            var paneRect = new Rect(rect) {x = rect.x + 4, width = paneWidth - 4 };
            if(isOddRaw) EditorGUI.DrawRect(rect, new Color(1, 1, 1, .2f));
            EditorGUI.LabelField(paneRect,gameObjectPath);

            foreach(var acue in absoluteCueList)
            {
				if (acue.Value.FindPropertyRelative("gameObjectName").stringValue.Equals(gameObjectPath))//gameObjectName == gameObjectPathとするより微妙に高速かもしれない
                {
                    float x = (int)Mathf.LerpUnclamped(paneWidth, rect.width, (acue.Key - startTime) / (endTime - startTime)) - 9;
					CueIcon(new Rect(x, rect.y + 1, 18, 18), acue.Value);
                }
            }
            return absoluteCueList;
        }

        #endregion

        #region CueIcon

		public static void CueIcon(Rect rect, SerializedProperty cueSerialized){
			string iconPath = AssetDatabase.GUIDToAssetPath("fac45307b96430b4e87c05173f3d3986");
			string iconSelectedPath = AssetDatabase.GUIDToAssetPath("a69cd2e95d5e387429baa8a7821b593c");
			GUI.DrawTexture(rect, AssetDatabase.LoadAssetAtPath<Texture>(iconPath));
			if (Event.current.type == EventType.MouseDown) {
				if(rect.Contains(Event.current.mousePosition))
					PopupWindow.Show (rect, new CuePopupWindow(cueSerialized));
			}
        }

        #endregion

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

	public class CuePopupWindow : PopupWindowContent
	{
		SerializedProperty cueSerialized;
		public CuePopupWindow(SerializedProperty cue){
			cueSerialized = cue;
		}

		public override void OnGUI (Rect rect)
		{
			//EditorGUILayout.LabelField (cue.gameObjectName + ":" + cue.parameter);
			EditorGUI.PropertyField(rect, cueSerialized ,true);
		}

		public override void OnOpen ()
		{
			//Debug.Log ("表示するときに呼び出される");
		}

		public override void OnClose ()
		{
			//Debug.Log ("閉じるときに呼び出される");
		}

		public override Vector2 GetWindowSize ()
		{
			//Popup のサイズ
			return new Vector2 (386, 38);
		}
	}
}