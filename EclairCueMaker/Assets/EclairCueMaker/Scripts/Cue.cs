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
        private int gameObjectName;

        [SerializeField]
        private string cueEventID;

        //[SerializeField]
        //private System.Type paramType;
        [SerializeField]
        private string parameter;

        /*public GameObject getGameObject()
        {
            return 
        }*/

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
