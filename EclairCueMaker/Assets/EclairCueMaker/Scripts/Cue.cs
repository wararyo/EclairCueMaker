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

		//Cueごとに割り当てられる固有のID
		//Unityのシリアライザの使用によりSerializedPropertyがEqualじゃなくなるからIDにより判定するよ
		public string UUID;

		public Cue(){ 
			UUID = GenerageUUID ();
		}

		public Cue (float time, string gameObjectName, string cueEventID, string parameter, UnityEngine.Object paramObject)
			: this ()
        {
            this.time = time;
            this.gameObjectName = gameObjectName;
            this.cueEventID = cueEventID;
            this.parameter = parameter;
            this.paramObject = paramObject;
        }

		public Cue(string UUID, float time, string gameObjectName, string cueEventID, string parameter, UnityEngine.Object paramObject)
			: this(time,gameObjectName,cueEventID,parameter,paramObject)
		{
			this.UUID = UUID;
		}

		public Cue(Cue copy) : this()
		{
			time = copy.time;
            gameObjectName = copy.gameObjectName;
            cueEventID = copy.cueEventID;
            //paramType = copy.paramType;
            parameter = copy.parameter;
            paramObject = copy.paramObject;
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
            Invoke(GameObject.Find(cue.gameObjectName), cue);
        }

        public static void Invoke(GameObject go,Cue cue)
        {
            var cueEvents = go.GetComponents<CueEventBase>();
            foreach (CueEventBase e in cueEvents)
            {
                if (e.EventID.Equals(cue.cueEventID))
                {
                    e.Cue(ConvertParam(cue,e.ParamType));
                }
            }
        }

        public static object ConvertParam(Cue cue,Type type)
        {
            string st = cue.parameter;
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
            else if (type.Equals(typeof(bool)))
            {
                return bool.Parse(st);
            }
            else if (type.Equals(typeof(GameObject)))
            {
                return GameObject.Find(st);
            }
			else if(type.IsSubclassOf(typeof(UnityEngine.Object))){
                return cue.paramObject;
			}
            return null;
        }

		public static string GenerageUUID(){
			/*Guid guid = Guid.NewGuid();
			StringBuilder sb = new StringBuilder();
			byte[] bytes = guid.ToByteArray();
			foreach (Byte b in bytes)
			{
				sb.AppendFormat("{0:x2}", b);
			}
			return sb.ToString();*/
			return System.Guid.NewGuid ().ToString ();
		}

    }

}
