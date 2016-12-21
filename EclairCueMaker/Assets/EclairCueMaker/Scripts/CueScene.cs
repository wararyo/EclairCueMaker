using UnityEngine;
using System.Collections.Generic;

namespace wararyo.EclairCueMaker {

	public class CueScene : ScriptableObject
	{

		[SerializeField]
		public List<Cue> cueList = new List<Cue>();

		public float Length{
			get{
				float length = 0;
				foreach (Cue c in cueList) {
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

		private int Version = 1;

		public void Awake(){
			
		}
	}
}