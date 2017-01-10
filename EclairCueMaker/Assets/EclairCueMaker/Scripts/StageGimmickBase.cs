using UnityEngine;
using System.Collections;

namespace wararyo.EclairCueMaker
{

    public abstract class StageGimmickBase : MonoBehaviour
    {
        public GameObject target;

		[SerializeField, HideInInspector]
		private string cueEventID;

        public virtual void DispatchEvent()
        {
			if (target)
				target.GetComponent<CueEventBase> ().Cue<string> ("");
			else
				Debug.LogWarning ("Target of " + gameObject.name + "." + name + " is not assigned.");
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position + Vector3.up * 0.5f, "Event.png", true);
        }
    }
}
