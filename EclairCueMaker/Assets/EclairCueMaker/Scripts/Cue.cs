using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace wararyo.EclairCueMaker
{
    [System.Serializable]
    public class Cue
    {
        [SerializeField]
        public float time;

        public string gameObjectName;

        [SerializeField]
		public string cueEventID;

        //[SerializeField]
        //private System.Type paramType;
        [SerializeField]
		public string parameter;

		//parameterにScene上にないオブジェクトが指定された時に使用される
		//ちょっと頭悪い方法だけど…他に思いつかないので…
		public UnityEngine.Object paramObject;

		public Cue(){
		}

		public Cue(Cue copy){
			time = copy.time;
            gameObjectName = copy.gameObjectName;
            cueEventID = copy.cueEventID;
            //paramType = copy.paramType;
            parameter = copy.parameter;
		}


        //以下、ユーティリティ系

        /// <summary>
        /// 引数に指定したCueを実行します。
        /// </summary>
        public static void Invoke(Cue cue)
        {
            /*GameObject go = GameObject.Find(cue.gameObjectName);
            var cueEvents = go.GetComponents<CueEventBase>();
            foreach(CueEventBase e in cueEvents)
            {
                if (e.EventID.Equals(cue.cueEventID))
                {
                    e.Cue(ConvertParam(cue.parameter, e.ParamType));
                }
            }*/
            Invoke(GameObject.Find(cue.gameObjectName), cue.cueEventID, cue.parameter);
        }

        public static void Invoke(GameObject go,string cueEventID,string parameter)
        {
            var cueEvents = go.GetComponents<CueEventBase>();
            foreach (CueEventBase e in cueEvents)
            {
                if (e.EventID.Equals(cueEventID))
                {
                    e.Cue(ConvertParam(parameter, e.ParamType));
                }
            }
        }

        public static object ConvertParam(string st,Type type)
        {
            if (type.Equals(typeof(void)))
            {
                return null;
            }
            else if (type.Equals(typeof(string)))
            {
                return st;
            }
            else if (type.Equals(typeof(int)))
            {
                return int.Parse(st);
            }
            else if (type.Equals(typeof(float)))
            {
                return float.Parse(st);
            }
            else if (type.Equals(typeof(GameObject)))
            {
                return GameObject.Find(st);
            }
			else if(type.IsSubclassOf(typeof(UnityEngine.Object))){
				return AssetD;
			}
            return null;
        }

    }

}
