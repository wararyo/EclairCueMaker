using UnityEngine;
using System.Collections;
using wararyo.EclairCueMaker;
using System;

public class EventWay : StageGimmickBase {

	private Animator animator;

	[TooltipAttribute("一回イベントが起きたらwayEnabledがfalseになります。")]
	public bool singleUse = true;

	[TooltipAttribute("プレイヤーが通過した時にイベントが発行されるかどうか")]
	public bool wayEnabled = true;

	//switchStateは感圧スイッチの状態。0が基本、1が押した状態、2が押していない状態

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider collider){
		if (wayEnabled) {
			if (collider.gameObject.tag == "Player") {
				DispatchEvent ();
				if (singleUse)
					wayEnabled = false;
			}
		}
	}

    public override void DispatchEvent()
    {
        base.DispatchEvent();
    }

	public override void OnDrawGizmos ()
	{
		Gizmos.color = new Color(1, 0.8f, 0, 0.2f);
		Gizmos.DrawCube (transform.position, transform.lossyScale);
		Gizmos.DrawWireCube (transform.position, transform.lossyScale);
		base.OnDrawGizmos ();
	}
}