using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using wararyo.EclairCueMaker;

[RequireComponent(typeof(Animator))]
public class CueEvent_InAndOut : CueEventBase {

	private bool isStaged = false;

	// Use this for initialization
	void Start () {
		if(GetComponent<Renderer>())GetComponent<Renderer>().enabled = false;
		ChildIsEnabled = false;

	}

	private bool ChildIsEnabled {
		set{
			foreach (Renderer g in gameObject.GetComponentsInChildren<Renderer>()) {
				g.enabled = value;
			}
		}
	}

	/// <summary>
	/// CueSceneEditorのCue編集画面においてCueEventリストに表示される文字列です。
	/// </summary>
	public override string EventName
	{
		get
		{
			return "3D In and Out";
		}
	}

	/// <summary>
	/// CueEvent同士を区別するときに用いる識別子です。
	/// 他と被らない一意的な文字列である必要があります。
	/// 適当にキーボードをガチャガチャしてください。
	/// このCueEventが何らかのCueから参照されているときにこの値を変更した場合、動作は保証されません。
	/// </summary>
	public override string EventID
	{
		get
		{
			return "9384jfdns38ton92ewhf";
		}
	}

	/// <summary>
	/// CueSceneEditorのCue編集画面でこのCueEventを指定したとき、ParamTypeで指定した型に応じて引数編集GUIが表示されます。
	/// CueScenePlayerがCueメソッドを実行するとき、引数paramにはParamTypeで指定した型の変数が入ります。
	/// 現在サポートしているParamTypeは以下の通りです。
	/// string
	/// int
	/// float
	/// GameObjectなどのUnityEngine.Object継承型
	/// </summary>
	public override Type ParamType
	{
		get
		{
			return typeof(void);
		}
	}


	public override void Cue(object param)
	{
		//Debug.Log ("aho");
		if (isStaged) {//アウトのアニメーション
			GetComponent<Animator>().Play("Out");
		} else {//インのアニメーション
			if(GetComponent<Renderer>())GetComponent<Renderer>().enabled = true;
			ChildIsEnabled = true;
			isStaged = true;
			GetComponent<Animator>().Play("In");
		}
	}
}