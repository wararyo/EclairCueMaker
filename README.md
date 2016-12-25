# <img src="https://raw.githubusercontent.com/wararyo/EclairCueMaker/master/Images/IconHiDPI.svg" width=48px /> EclairCueMaker

## 概要
EclairCueMakerは、アニメーション再生などの処理を時間差をつけて実行させることにより、UIアニメーションや、カットシーン作成などを行えるUnity用ツールです。タイムライン上で視覚的に編集することができます。

## 注意点
EclairCueMakerはあくまでアニメーション再生などの「合図」を出すだけであり、EclairCueMaker単体でアニメーションを行うことはできません。
## 用語の定義
### Cue
時間差をつけて実行される「合図」です。`Cue`が保持している情報は以下の通りです。
* 直前の`Cue`からの待機時間
* キューを出す対象となるGameObject
* そのGameObjectにアタッチされた`CueEvent`の中のどれを実行するか
* `CueEvent`ごとのパラメーター

### CueEvent
`Cue`が発行された際に起こる動作です。あらかじめ数種類が用意されており、いずれかを対象のゲームオブジェクトにアタッチして使用します。実体は`CueEventBase`を継承したクラスですので、必要に応じてあなたもCueEventを自作することができます。
### CueScene
複数の`Cue`をまとめたもので、独立したファイルとして保存することができます(拡張子は.asset)。特定のUnityシーンに関連付けられています。内部的にはCueの配列の他、関連付けられたシーン名などが含まれています。なお、.assetはUnityでクラスを外部ファイルに保存する際に一般的に使われる拡張子であり、.assetファイルがすべて`CueScene`のファイルとは限りません(インスペクターから区別することができます)
### CueSceneEditor
CueSceneを作成するエディターです。`CueEditor`と省略されることがあります。UnityのWindowメニュー>EclairCueEditorを選択することで開きます。
### CueScenePlayer
Unityシーン内の実際のゲームオブジェクトにアタッチして使うコンポーネントです。"Manager"といった名前の空のゲームオブジェクトを作成し、そこにアタッチする使い方を想定しています。特定の`CueScene`を指定すると、それを再生します。
## 任意のメソッドを実行できるようにしなかった理由
`Cue`には`CueEventBase`を継承したクラスに書かれた動作しか関連付けられず、`Cue`発行時に好きなメソッドを好き放題実行することはできません。理由は、「EclairCueMakerを知らない人がプロジェクトファイルを見た時に解読のヒントを与える」ためです。  
実はUnityに標準でEclairCueMakerのような機能があり、Animationがそれに該当します。UnityのAnimationでは、"Event"という概念で好きなタイミングで好きなメソッドを実行できます。しかしこの機能は広くは知られていません。AnimationのEvent機能を知らない人がプロジェクトファイルを見た場合、「なんでか知らないけど効果音が鳴る処理が突然実行される、どこから実行されてるのか分からない…」といった状況に陥ります。  
一方EclairCueMakerでは、アニメーション再生や効果音再生などの処理が走るゲームオブジェクトにはかならず`CueEvent`継承クラスがアタッチされているので、そこを糸口に`EclairCueMaker`の存在に気づき、解読を進めることができます。

## 導入方法

## 使い方

## CueEventを自作する(上級)

## CueScenePlayerをカスタマイズする(さらに上級)
