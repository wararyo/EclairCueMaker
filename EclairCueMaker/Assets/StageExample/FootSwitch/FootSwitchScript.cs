using UnityEngine;
using System.Collections;
using wararyo.EclairCueMaker;
using System;

public class FootSwitchScript : StageGimmickBase {

	private Animator animator;

    private const string AnimParam_IsSteppedOn = "IsSteppedOn";

    private bool isEventDisabled = false;

	//switchStateは感圧スイッチの状態。0が基本、1が押した状態、2が押していない状態

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Player") {
            animator.SetBool(AnimParam_IsSteppedOn, true);
            if (!isEventDisabled)
            {
                DispatchEvent();
                StartCoroutine(AfterSteppedOn());
            }
        }
	}
	private void OnTriggerExit(Collider collider){
        if (collider.gameObject.tag == "Player")
        {
            animator.SetBool(AnimParam_IsSteppedOn, false);
        }
    }

    //チャタリング現象のようなものを防ぐためのもの
    //一回押されたらしばらくはイベントが発行されない
    IEnumerator AfterSteppedOn()
    {
        isEventDisabled = true;
        yield return new WaitForSeconds(0.5f);
        isEventDisabled = false;
    }

    public override void DispatchEvent()
    {
        base.DispatchEvent();
    }
}