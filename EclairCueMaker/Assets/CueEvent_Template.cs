using UnityEngine;
using System.Collections;
using wararyo.EclairCueMaker;
using System;

public class CueEvent_Template : CueEventBase {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// CueSceneEditorのCue編集画面に置いてCueEventリストに表示される文字列です。
    /// </summary>
    public override string EventName
    {
        get
        {
            return "**Input name of this CueEvent here**";
        }
    }

    /// <summary>
    /// CueSceneEditorのCue編集画面でこのCueEventを指定したとき、ParamTypeで指定した型に応じて引数編集画面が表示されます。
    /// よってCueScenePlayerがCueメソッドを実行するとき、引数paramにはParamTypeで指定した型の変数が入ります。
    /// 現在サポートしているParamTypeは以下の通りです。
    /// 
    /// </summary>
    public override Type ParamType
    {
        get
        {
            return typeof(void);
        }
    }


    public override void Cue<T>(T param)
    {
        //ここに任意の処理を記述
    }
}
