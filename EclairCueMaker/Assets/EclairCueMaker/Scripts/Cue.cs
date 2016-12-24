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
        private GameObject gameObject;

        [SerializeField]
        private string cueEventID;

        //[SerializeField]
        //private System.Type paramType;
        [SerializeField]
        private object parameter;



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
            //paramType = copy.paramType;
            parameter = copy.parameter;
		}
    }

}
