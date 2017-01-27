# <img src="Images/IconHiDPI.png" width=48px /> EclairCueMaker

## 概要
EclairCueMakerは、アニメーション再生などの処理を時間差をつけて実行させることにより、UIアニメーションや、カットシーン作成などを行えるUnity用ツールです。タイムライン上で視覚的に編集することができます。

この文書は、途中までは非プログラマーにも概ね理解していただける内容となっています。

## 注意点
EclairCueMakerはあくまでアニメーション再生などの「合図」を出すだけであり、EclairCueMaker単体でアニメーションを行うことはできません。
## 用語の定義
### Cue
時間差をつけて実行される「合図」です。`Cue`が保持している情報は以下の通りです。
* 直前の`Cue`からの待機時間
* キューを出す対象となるゲームオブジェクト
* そのゲームオブジェクトにアタッチされた`CueEvent`の中のどれを実行するか
* `CueEvent`ごとのパラメーター

### CueEvent
`Cue`が発行された際に起こる動作です。あらかじめ数種類が用意されており、いずれかを対象のゲームオブジェクトにアタッチして使用します。実体は`CueEventBase`を継承したクラスですので、必要に応じて自作することができます。
### CueScene
複数の`Cue`をまとめたもので、独立したファイルとして保存することができます(拡張子は.asset)。特定のUnityシーンに関連付けられています。内部的にはCueの配列の他、関連付けられたシーン名などが含まれています。なお、.assetはUnityでクラスを外部ファイルに保存する際に一般的に使われる拡張子であり、.assetファイルがすべて`CueScene`のファイルとは限りません

### CueSceneEditor
`CueScene`を作成するエディターです。`CueEditor`と省略されることがあります。UnityのWindowメニュー>EclairCueEditorを選択することで開きます。

<img src="Images/EclairCueEditor.png" width="256px" />

### CueScenePlayer
Unityシーン内の実際のゲームオブジェクトにアタッチして使うコンポーネントです。"Manager"といった名前の空のゲームオブジェクトを作成し、そこにアタッチする使い方を想定しています。特定の`CueScene`を指定すると、それを再生します。
## 任意のメソッドを実行できるようにしなかった理由
`Cue`には`CueEventBase`を継承したクラスに書かれた動作しか関連付けられず、`Cue`発行時に好きなメソッドを好き放題実行することはできません。理由は、「EclairCueMakerを知らない人がプロジェクトファイルを見た時に解読のヒントを与える」ためです。  
実はUnityに標準でEclairCueMakerのような機能があり、Animationがそれに該当します。UnityのAnimationでは、"Event"という概念で好きなタイミングで好きなメソッドを実行できます。しかしこの機能は広くは知られていません。AnimationのEvent機能を知らない人がプロジェクトファイルを見た場合、「なんでか知らないけど効果音が鳴る処理が突然実行される、どこから実行されてるのか分からない…」といった状況に陥ります。  
一方EclairCueMakerでは、アニメーション再生や効果音再生などの処理が走るゲームオブジェクトにはかならず`CueEvent`継承クラスがアタッチされているので、そこを糸口にEclairCueMakerの存在に気づき、解読を進めることができます。

## 導入方法
1. EclairCueMakerをダウンロードまたはgit cloneします。
2. Assets/EclairCueMakerフォルダを、導入したいUnityプロジェクトのAssetsフォルダ以下にコピーします。
3. Assets/Gizmosフォルダを、導入したいUnityプロジェクトのAssetsフォルダにコピーします。

## 主な流れ
### 時間差で処理を実行する
#### 1.CueSceneを作成
Createメニュー>EclairCueSceneを選択し、新たなCueSceneファイルを作成します。

<img src="Images/EclairCueScene.png" width="256px" />
#### 2.CueScenePlayerをアタッチ
処理を時間差で実行したいUnityシーンを開き、好きなゲームオブジェクトに`CueScenePlayer`をアタッチします。
`Manager`といった名前の空のゲームオブジェクトを作成し、そこにアタッチする方法をおすすめしています。
さらに`CueScenePlayer`コンポーネント内の"Cue Scene"パラメーターに、先ほど作成した`CueScene`をアタッチします。

<img src="Images/CueScenePlayer.png" width="320px" />  
(Cue Event_Play Cue Sceneについては後ほど説明します。)

Play On Awakeをオンにした場合は、Unityシーンを開いたと同時に`CueScene`の再生を開始します。

#### 3.処理を実行する対象にCueEventをアタッチ
実行したい処理に応じた`CueEvent`を、**実行する対象のゲームオブジェクト**にアタッチします
(CueScenePlayerがアタッチされたゲームオブジェクトではない場合が多いです)
例えば様々な8個の立方体Cube1,Cube2,...,Cube8があり、それらに順番にアニメーションを実行させたい場合、
Cube1からCube8すべてに`CueEvent_PlayAnimState`をアタッチします。

#### 4.CueSceneを作成
Windowメニュー>EclairCueEditorを選択し`CueSceneEditor`を開きます。

<img src="Images/EclairCueEditor.png" width="256px" />

[CueSceneEditorでCueSceneを編集する](#cuesceneeditorでcuesceneを編集する)に基づき、CueSceneを編集します。

<img src="Images/CueSceneExample.png" width="640px" />  
編集されたCueSceneの例

Play On Awakeがオンである場合は、この時点で、
Unityの再生ボタンを押すとCueSceneの再生が開始します。
Play On Awakeがオフである場合はさらに、
[ステージギミックにより時間差で処理を実行する](#ステージギミックにより時間差で処理を実行する)などの方法により、CueSceneを再生する命令を出す必要があります。

### ステージギミックにより処理を実行する
#### 1.ステージギミックPrefabをプログラマーに作ってもらう
作ってもらってください。
ちなみにここでは、ステージギミックPrefabを、
「`StageGimmickBase`継承クラスがアタッチされたPrefab」と定義しておきます。
#### 2.ステージギミックPrefabを配置する
#### 3.ステージギミックとCueEventを紐付けする
<img src="Images/StageGimmickExample.png" width="320px" />  
BridgeゲームオブジェクトのCueEvent_PlayAnimatorStateを指定した例

`CueSceneEditor`で`CueEvent`を指定するときと同様に`CueEvent`の指定を行います。

以上で完了です。
プログラマーが設計した方法でステージギミックを作用させると、指定した`CueEvent`が実行されます。

### ステージギミックにより時間差で処理を実行する
上記2つの混合です。
ステージギミックによりCueSceneを再生させるという動作を実現するため、
CueSceneを再生させるためのCueEvent、"CueEvent\_PlayCueScene"を用意しました。
`CueEvent`には「その`CueEvent`がアタッチされているゲームオブジェクト以外のゲームオブジェクトには原則として影響を与えない」という規則が設けられていますが、CueEvent\_PlayCueSceneはその例外となります。

#### 1.ManagerにCueEvent\_PlayCueSceneをアタッチする
<img src="Images/CueScenePlayer.png" width="320px" />

`CueScenePlayer`がアタッチされているゲームオブジェクトに、CueEvent\_PlayCueSceneをアタッチします。

#### 2.ステージギミックのCueEventにCueEvent\_PlayCueSceneを指定する
<img src="Images/CueEventCueScenePlay.png" width="320px" />

以上で、ステージギミックを作用させると、指定した`CueScene`が再生されます。

## CueSceneEditorでCueSceneを編集する
EclairCueMakerの要となる、CueSceneEditorの使い方です。
編集したいCueSceneをCueEventPlayerにセットし、CueEventPlayerがアタッチされているゲームオブジェクトを選択すると、そのCueSceneが編集可能となります。

`CueSceneEditor`には、CueSceneを表示する2つの方法があります。  
一つがTimeline表示  
<img src="Images/CueSceneEditorTimeline.png" width="640px" />

もう一つがRaw表示です。  
<img src="Images/CueSceneExample.png" width="640px" />

CueSceneの編集に関しては、現在、Raw表示でのみ対応しています。

### Raw表示での編集
CueSceneに含まれているCueが上から下へ時系列で並んでいます。
右下の + ボタンを押すと、新しいCueを作成します。

<img src="Images/CueProperty.png" width="480px" />  
Cue

"Duration"は、一つ前に実行されたCueからの待機時間を示しています。
Durationより左の領域でCueEventを指定します。
左上に位置しているのが、対象となるゲームオブジェクトです。
このゲームオブジェクトは、CueScenePlayerがアタッチされているゲームオブジェクトと同じシーンに含まれている必要があり、かつ、CueEventコンポーネントをアタッチしている必要があります。

その右は、対象にCueEventコンポーネントが複数アタッチされていた場合に、どのCueEventを実行するかを指定するプルダウンメニューです。
指定した対象が、CueEventコンポーネントを一つもアタッチしていない場合は表示されません。

<img src="Images/CuePropertyWithNoCueEvent.png" width="480px" />  

その下で引数、すなわちパラメーターを指定します。
指定したCueEventが引数を要求するものである場合、引数の種類に応じたGUI表示されます。
何も表示されない場合は、そのCueEventが引数を要求していないことを意味します。

<img src="Images/CuePropertyWithBoolParameter.png" width="480px" />  
Bool型の引数を要求するCueEvent ChangeAnimatorEnabledの例


## CueEventを自作する(プログラマー向け)

## ステージギミックを自作する(プログラマー向け)

## CueScenePlayerをカスタマイズする(上級)
