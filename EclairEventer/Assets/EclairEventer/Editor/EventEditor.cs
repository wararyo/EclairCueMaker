using UnityEngine;
using UnityEditor;
using System.Collections;

namespace wararyo.EclairEventer
{
    public class EventEditor : EditorWindow
    {
        private int tab = 0;

        [MenuItem("Window/EclairEventEditor")]
        static void Open()
        {
            EditorWindow.GetWindow<EventEditor>("EventEditor");
        }

        void OnGUI()
        {
            using (var horizontalScope = new EditorGUILayout.HorizontalScope("toolbar"))
            {
                tab = GUILayout.Toolbar(tab, new string[] { "Timeline", "Raw" }, EditorStyles.toolbarButton, GUILayout.Width(180));
                //GUILayout.Space(20);
                using (new EditorGUI.DisabledScope(true)) GUILayout.Button("", EditorStyles.toolbarButton);
            }
            if (tab == 0)//Timelineタブ
            {
                // 試しにラベルを表示
                EditorGUILayout.LabelField("ようこそ！　Unityエディタ拡張の沼へ！");
            }
            else//Rawタブ
            {
                EditorGUILayout.LabelField("こちらはRawタブとなっております。");
            }
        }
    }
}
