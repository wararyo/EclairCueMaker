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
		private int gameObjectID;

        //CueDrawerにて変更
        [SerializeField]
        public GameObject gameObject;

        [SerializeField]
		public string cueEventID;

        //[SerializeField]
        //private System.Type paramType;
        [SerializeField]
		public string parameter;

		//CueEventのTypeがObject型だった時のみ用いられる
		[SerializeField]
		public Object paramObject;



        public GameObject getGameObject()
        {
            return gameObject;
        }

        /*public GameObject getGameObject()
        {
            return 
        }*/

		public Cue(){
		}

		public Cue(Cue copy){
			time = copy.time;
			gameObjectID = copy.gameObjectID;
            gameObject = copy.gameObject;
            cueEventID = copy.cueEventID;
            //paramType = copy.paramType;
            parameter = copy.parameter;
            paramObject = copy.paramObject;
		}
    }

}
