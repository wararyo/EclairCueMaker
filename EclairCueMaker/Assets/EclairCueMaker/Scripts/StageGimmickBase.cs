using UnityEngine;
using System.Collections;

namespace wararyo.EclairCueMaker
{

    public abstract class StageGimmickBase : MonoBehaviour
    {
        [HideInInspector]
        public GameObject target;

		[HideInInspector]
		public string cueEventID;

        [HideInInspector]
        public string parameter;

        [HideInInspector]
        public Object paramObject;

        public virtual void DispatchEvent()
        {
            if (target)
                Cue.Invoke(target,new Cue(0,"",cueEventID,parameter,paramObject));
			else
				Debug.LogWarning ("Target of " + gameObject.name + "." + name + " is not assigned.");
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position + Vector3.up * 0.5f, "StageGimmick.png", true);
        }
    }
}
