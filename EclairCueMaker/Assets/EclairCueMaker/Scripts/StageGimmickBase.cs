using UnityEngine;
using System.Collections;

namespace wararyo.EclairCueMaker
{

    public abstract class StageGimmickBase : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject target;

        public virtual void DispatchEvent()
        {
            target.GetComponent<CueEventBase>().Cue<string>("");
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position + Vector3.up * 0.5f, "Event.png", true);
        }
    }
}
