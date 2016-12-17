using UnityEngine;
using System.Collections;

namespace wararyo.EclairCueMaker {

public class CueScene : ScriptableObject
{
		[SerializeField]
		string st = "hoge";

		[SerializeField]
		Cue[] cueList;
	}
}