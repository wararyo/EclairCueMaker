using UnityEngine;
using System.Collections;

namespace wararyo.EclairCueMaker
{

    public abstract class CueEventBase : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public abstract void Queue();

        public abstract void Queue<T>(T t)
        {

        }
    }
}
