using UnityEngine;
using UnityEngine.Events;
using System.Collections;
namespace wararyo.EclairCueMaker
{
    [System.Serializable]
    public class Cue
    {
        [SerializeField]
        public float time;

        //CueDrawerにて変更
        [SerializeField]
        public int gameObjectName;

        [SerializeField]
		public string cueEventID;

        //[SerializeField]
        //private System.Type paramType;
        [SerializeField]
		public string parameter;

		public Cue(){
		}

		public Cue(Cue copy){
			time = copy.time;
            gameObjectName = copy.gameObjectName;
            cueEventID = copy.cueEventID;
            //paramType = copy.paramType;
            parameter = copy.parameter;
		}
    }

}
