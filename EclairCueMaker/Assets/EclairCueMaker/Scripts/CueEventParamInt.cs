using UnityEngine;
using System.Collections;

namespace wararyo.EclairCueMaker
{
	public class CueEventParamInt : CueEventParamBase
	{
		public int value;

		public CueEventParamInt(int paramvalue){
			value = paramvalue;
		}
	}
}