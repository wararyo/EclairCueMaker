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
		public GameObject gameObject;

		public Cue(){

		}

		public Cue(Cue copy){
			time = copy.time;
			gameObject = copy.gameObject;
		}
    }

}
