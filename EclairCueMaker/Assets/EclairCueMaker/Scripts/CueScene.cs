using UnityEngine;
using System.Collections.Generic;

namespace wararyo.EclairCueMaker {

    [CreateAssetMenu(fileName ="NewCueScene.asset",menuName ="EclairCueScene",order = 370)]
	public class CueScene : ScriptableObject
	{
        [SerializeField]
        public string attachedSceneGUID = "";

		[SerializeField]
		public List<Cue> cueList = new List<Cue>();

		public float Length{
			get{
				float length = 0;
				foreach (Cue c in cueList) {
                    if(c.time > 0)
					    length += c.time;
				}
				return length;
			}
		}

		public int Count {
			get {
				return cueList.Count;
			}
		}

        [SerializeField]
		private int Version = 1;

		public void Awake(){
			
		}
	}
}