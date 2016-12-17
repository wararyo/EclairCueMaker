using UnityEngine;
using UnityEditor;
using System.Collections;

namespace wararyo.EclairEventer
{

    public class EclairGUILayout
    {

        #region Ruler
        public static void Ruler (float startTime,float endTime,params GUILayoutOption[] options)
        {
            using (new EditorGUI.DisabledScope(true)) GUILayout.Button("", EditorStyles.toolbarButton,options);
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

        public static void Ruler (Rect rect,float startTime, float endTime)
        {
            if (rect.height == 1.0f) return;

            var id = GUIUtility.GetControlID(FocusType.Keyboard, rect);

            Texture2D tex = new Texture2D((int)rect.width, (int)rect.height);
            tex.alphaIsTransparency = true;
            //tex.filterMode = FilterMode.Point;

            Color[] transparent = new Color[tex.width * tex.height];
            for (int i = 0;i < transparent.Length;i++)
            {
                transparent[i] = new Color(0, 0, 0, 0);
            }
            tex.SetPixels(0, 0, tex.width, tex.height,transparent);

            //目盛りと数字の描画開始
            for(float i=((int)(startTime*10))/(float)10;i < endTime; i += 0.1f)//目盛り単位0.1秒
            {
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
        static Texture2D drawScale(Texture2D tex,int x,int height,Color color)
        {
            for (int i = 0; i < height; i++) {
                tex.SetPixel(x, i, color);
            }
            return tex;
        }

        #endregion

        #region TimelineTrack

        public static void TimelineTrack(Track t)
        {
            
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
}
