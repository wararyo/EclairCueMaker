using UnityEngine;
using System.Collections;
using wararyo.EclairCueMaker;
using System;

public class CueEvent_PlayState : CueEventBase {

	void Start () {
	
	}

    /// <summary>
    /// CueSceneEditorのCue編集画面においてCueEventリストに表示される文字列です。
    /// </summary>
    public override string EventName
    {
        get
        {
            return "PlayAnimatorState";
        }
    }

    //public string stateName = "Anim";

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
            return "656f78s8f9d0n97d";
        }
    }

    /// <summary>
    /// CueSceneEditorのCue編集画面でこのCueEventを指定したとき、ParamTypeで指定した型に応じて引数編集GUIが表示されます。
    /// CueScenePlayerがCueメソッドを実行するとき、引数paramにはParamTypeで指定した型の変数が入ります。
    /// 現在サポートしているParamTypeは以下の通りです。
    /// string
    /// int
    /// float
    /// GameObject
    /// </summary>
    public override Type ParamType
    {
        get
        {
            return typeof(string);
        }
    }


    public override void Cue(object param)
    {
		GetComponent<Animator>().Play((string)param);
    }
}
