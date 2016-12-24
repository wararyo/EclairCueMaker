using UnityEngine;
using System.Collections;

namespace wararyo.EclairCueMaker
{
	public class CueEventParamString : CueEventParamBase
	{
		public string value;

		public CueEventParamString(string paramvalue){
			value = paramvalue;
		}
	}
}