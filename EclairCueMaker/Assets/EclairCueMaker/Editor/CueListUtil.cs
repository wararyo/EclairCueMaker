using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace wararyo.EclairCueMaker
{

    public static class CueListUtil
    {
		public static List<KeyValuePair<float, SerializedProperty>> GenerateAbsoluteCueList(SerializedProperty cueListSerialized)
        {
			var absoluteCueList = new List<KeyValuePair<float, SerializedProperty>>();
            float sum = 0;
			for(int i=0;i < cueListSerialized.arraySize;i++)
            {
				var cueSerialized = cueListSerialized.GetArrayElementAtIndex (i);
				sum += cueSerialized.FindPropertyRelative("time").floatValue;
				absoluteCueList.Add(new KeyValuePair<float,SerializedProperty>(sum, cueSerialized));
            }
            return absoluteCueList;
        }

        public static List<Cue> GenerateCueListFromAbsolute(List<KeyValuePair<float,SerializedProperty>> absoluteCueList)
        {
            absoluteCueList.Sort((a, b) => CompareFloat(a.Key,b.Key));
            List<Cue> list = new List<Cue>();
			for(int i=0;i < absoluteCueList.Count;i++)
            {
				var cue = absoluteCueList [i].Value;
				float time = absoluteCueList [i].Key - ((i == 0) ? (0) : (absoluteCueList [i - 1].Key));
				list.Add(new Cue(cue.FindPropertyRelative("UUID").stringValue,
					time,
					cue.FindPropertyRelative("gameObjectName").stringValue,
					cue.FindPropertyRelative("cueEventID").stringValue,
					cue.FindPropertyRelative("parameter").stringValue,
					cue.FindPropertyRelative("paramObject").objectReferenceValue));
            }
            return list;
        }

        public static int CompareFloat(float a, float b)
        {
            if (a > b)
            {
                return 1;
            }
            else if (a < b)
            {
                return -1;
            }
            else//a == b
            {
                return 1;//0にするとドラッグするたびに順番が入れ替わる怪現象が起こる
            }
        }
    }
}
