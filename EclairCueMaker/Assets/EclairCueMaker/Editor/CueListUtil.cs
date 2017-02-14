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
            foreach(var acue in absoluteCueList)
            {
                list.Add(new Cue());
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
                return 0;
            }
        }
    }
}
