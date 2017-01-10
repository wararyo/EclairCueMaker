using UnityEngine;
using System.Collections;

namespace wararyo.EclairCueMaker
{

    public abstract class StageGimmickBase : MonoBehaviour
    {
        [HideInInspector]
        public GameObject target;

		[SerializeField, HideInInspector]
		private string cueEventID;

        [HideInInspector]
        public string parameter;

        public virtual void DispatchEvent()
        {
            if (target)
                Cue.Invoke(target,cueEventID,parameter);
			else
				Debug.LogWarning ("Target of " + gameObject.name + "." + name + " is not assigned.");
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position + Vector3.up * 0.5f, "Event.png", true);
        }
    }
}
