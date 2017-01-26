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
    }
}
