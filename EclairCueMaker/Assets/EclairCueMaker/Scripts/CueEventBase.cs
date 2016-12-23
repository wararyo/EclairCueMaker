using UnityEngine;
using System.Collections;

namespace wararyo.EclairCueMaker
{

    public abstract class CueEventBase : MonoBehaviour
    {
        public abstract string EventName
        {
            get;
        }

        public abstract System.Type ParamType
        {
            get;
        }

        public abstract void Cue<T>(T param);
    }
}
