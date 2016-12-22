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

        [SerializeField]
        public UnityEvent cueEvent;

		[SerializeField]
		public int gameObjectID;

        [SerializeField]
        private GameObject gameObject;

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
		}
    }

}
