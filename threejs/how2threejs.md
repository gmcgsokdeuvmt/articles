Title:    three.jsで3DCGをお手軽に
Date:     2015-04-13 00:32
Category: 入門
Authors:  ran
Tags:     three.js


こんにちは、ランです。何か記事がないかということで、自分が COINS-Project に入ってから始めたことでも書こうと思います。HTML も JavaScript も詳しくないのに three.js ってのが段階飛ばし過ぎな感じがありますが…よろしくお願いします。

## なぜthree.js？

**3DCG を作るのはとても面倒だ**というイメージが私にはあります。いわゆる食わず嫌いです。何が面倒かって、**環境を整えて、マウスでポチポチして位置やらの調整**を要されるところだと思うんです。これだけでも、気軽に 3DCG やる…ってわけにはいかないと思うんですよね。

そこで、` three.js `というわけです。**利点**としては

  * ブラウザで 3DCG を動かせる
  * 必要な開発ソフトがない（最悪メモ帳だけでも問題ない）

というところです。メモ帳というか、単純に**` HTML `ファイルに` JavaScript `を書いてけばいいだけ**なので、それぞれが好きなテキストエディタ等を使えばいいと思います。私は` Visual Studio `か` Sublime Text `で書いてます。この辺は二の次だと思いますが、色々補間してくれたりする頭のいいエディタがあるといい感じだと思います。

ここで重要なのは、**何やらスクリプトを書くとHTMLファイルをブラウザで開くだけで3DCGを作ることができる**ところです。` three.js `を始めるには十分だと私は思いました。

まぁ、そもそものきっかけは ` cocuh `さんに「こんなものあるよ～」って提案をもらったところからなんですけれどもね。一人じゃなかなか思いつかないです。

## 前準備

最低限の準備が必要です。
  
  * ` three.js `をダウンロードして、編集する` HTML `ファイルと同じディレクトリに置く

実際には ` three.js `がローカルにあろうがインターネッツに転がってようがそのパスを指定できればそれでいいんですけれども、とりあえず1つのディレクトリにまとめましょう。

[three.js](http://threejs.org/)

このサイトの` download `という項から` zip `ファイルをダウンロードし、解凍します。解凍フォルダの` build `下に` three.js `が入ってます。解凍フォルダそのものには、` three.js `で使える色々な` js `ファイル・テクスチャ等が入ってます。

また、想定しているブラウザは` Google Chrome `です。変なことはやらないので` FireFox `や` Internet Explorer `ならおそらく動くと思います。

## three.jsで描画する

` three.js `で 3DCG を描画するのに必要なものは` camera `, ` scene `, ` renderer `です。それぞれの役割は

| 名称 | 役割 |
|:-----------|------------:|
| `camera` | 3D空間でのカメラの種類・位置・向きを決める |
| `scene` | オブジェクトを追加して3D空間を形成する |
| `renderer` | ` camera `と` scene `の状態を基に2D描画を行う |

となります。これらを組み合わせてdiv要素に描画を加える等してブラウザでの描画を実現します。

### 雛形の用意

個人的には、` init() `, ` animate() `, ` render() `を関数として定義した方が分かりやすいかなと思い使用しています。それぞれの関数の役割は以下の通りです。

| 名称 | 役割 |
|:-----------|------------:|
| ` init() ` | ` camera `, ` scene `, ` renderer `等の初期化を行う |
| ` animate() ` | シーンにあるオブジェクトの状態を更新して` render() `を呼び出す |
| ` render() ` | ` camera `, ` scene `の状態を基に2D描画を行う |

これらを雛形の上から各関数に分けて実装していきます。

` HTML `ファイルのソースコードを雛形としてそのまま以下に貼ります。ファイルの名前はなんでも大丈夫だと思います。
新規作成してこれをそのまま貼り付けましょう。

```
<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8" />
    <title>threejsTest</title>

</head>
<body>
    <div id="container">Generating world...</div>
    <script src="./three.js"></script>
    <script>
        // レンダラが描画したものを表示するコンテナ
        var container;

        // three.js ではカメラ・シーン・レンダラを使用する
        var camera, scene, renderer;

        // init で初期化して animate でループする
        init();
        animate();

        function init() {
        	// camera, scene, renderer等を初期化する
        }

        function animate() {
        	// camera, scene を制御する
        	// また、render()を実行して描画をする
        }

        function render() {
        	// レンダラによる描画をする
        }
    </script>
</body>
</html>
```

` init() `で初期化して` animate() `でループするのが大きな流れです。ただ、` three.js `を使用するために

```
<script src="./three.js"></script>
```

このコードをソースコードに含める必要があります。**` three.js `ファイルのパスを正しく指定しましょう**。

また、` init() `の実行が終わるまではロード中の文字表示が欲しいので、` <div> `要素として

```
<div id="container">Generating world...</div>
```

このコードをソースコードに含めておきます。` init() `による初期化が終わったら、` container `の中身を` renderer `による描画に差し替えます。

### init()を書く

まずは、カメラを設定します。
` init() `内に以下のコードを書きましょう

```
			// 遠近法を使ったカメラを設定
			// コンストラクタに渡す引数は左から、画角、アスペクト比、カメラがオブジェクトを映す距離の最小値、最大値
            camera = new THREE.PerspectiveCamera(60, window.innerWidth / window.innerHeight, 100, 2000);

            // カメラの位置を設定する
            camera.position.set(0, 500, 0);

            // カメラの向きを設定する
            camera.lookAt(new THREE.Vector3(0, 0, 0));
```

` three.js `のカメラにはいくつか種類がありますが、私たちの目に馴染み深いのは ` THREE.PerspectiveCamera `だと思います。これは、**遠近法を用いて**シーン内のオブジェクトを**2Dに映す方法**をとるカメラです。

また、このカメラをインスタンス化する際に渡す引数に、**画角・アスペクト比・カメラがオブジェクトを映す距離の範囲** があります。**画角**は大きくすれば**画面端の歪みが強くなります**し、**アスペクト比**を間違えれば**オブジェクトは縦や横に引き伸ばされます**。**最後2つの引数**については、わざわざ映す必要のない距離にあるオブジェクトを描画しないことによって**描画計算の負担を軽減する**効果があります。


カメラの種類を選んだら、その次は位置と向きを設定します。

` position.set ` によってカメラの位置を設定できます。引数は(x, y, z)それぞれの座標をとります。
また、` lookAt `によってカメラの向きをある座標に向けることができます。引数は` THREE.Vector3 `です。` position `も同じ` THREE.Vector3 `です。

今回の例ではカメラをy軸の正の位置に置き、カメラが` (0, 0, 0) `の座標を向くことでxz平面を垂直に映すような設定になっています。

次に、シーンを用意します。
` init() `内に以下のコードを追加します。

```
			// オブジェクトを管理するシーン
            scene = new THREE.Scene();

            // オブジェクト初期化
            // グリッドのインスタンス化
            var grid = new THREE.GridHelper(1000, 50);

            // グリッドの軸線とそれ以外の線の色を指定する
            grid.setColors(0xffffff, 0xaacc00);

            // グリッドオブジェクトをシーンに追加する
            scene.add(grid);
```

シーンを使うためにまず初期化が必要なので` THREE.Scene `を` new `します。シーンの設定は特にありません。**このシーンに描画するオブジェクトを追加する**ことでシーンを作ることができます。
では、シーンにグリッドを追加しましょう。` THREE.GridHelper ` を` new `することで、グリッドを生成できます。引数には**グリッド全体の１辺の長さとグリッドを何分割するか**を必要とします。

グリッド自体は、` position.set `、` setColors `、` rotateX `、` rotateY `、` rotateZ `を通じて**位置・色・向き**を設定できます。

最後に、レンダラの設定です。

以下のコードを` init() `に追加してください。

```
            // 描画更新を管理するレンダラの初期化
            // WebGLを使用する
            renderer = new THREE.WebGLRenderer();

            // 背景とする色・比率・サイズの初期化
            renderer.setClearColor(0x000000);
            renderer.setPixelRatio(window.devicePixelRatio);
            renderer.setSize(window.innerWidth, window.innerHeight);

            // div の container を拾ってくる
            container = document.getElementById('container');

            // container の要素を初期化
            container.innerHTML = "";

            // container で renderer の描画を映す
            container.appendChild(renderer.domElement);
```

まず使用するレンダラを指定して` new `します。今回は、` THREE.WebGLRenderer `を使用します。

その次に、` setClearColor `で**何も描画しないところの色**を設定、` setPixelRatio `で**画面のピクセル比**を設定、`setSize`で**描画する画面の大きさ**を設定します。

あとは、` container `を操作して` renderer `のDOM要素を` container `の中身とするだけです。

これで` init() `は終了です。

### animate()、render()を設定する

あとは描画をするだけです。` animate() `、` render() `内に以下のようにコードを追加してください。

```
        function animate() {

            // 再描画の準備が整い次第実行される
            requestAnimationFrame(animate);

            // 描画
            render();

        }

        function render() {

            // シーン・カメラ情報を基にして2Dに描画
            renderer.render(scene, camera);

        }
```

この時点で、` HTML `ファイルを保存してブラウザで開いてみるとグリッドが表示されているはずです。
これは、毎フレーム` requestAnimationFrame(animate) `が再帰的に実行されて、` render() `によって画面が更新されているからです。` render() `の中はシーンとカメラを指定して、` renderer.render(scene, camera) `を実行するだけです。レンダリング時に何か処理が必要な場合はここに処理を書くべきなんですが、私にはその例が思いつきません。申し訳ないです。

グリッドが表示できたのはいいですが、これが3Dなのかどうかが全くわかりません。少しカメラを傾けてみましょう。

` init() `内でカメラの位置を修正します。

```diff
            // カメラの位置を設定する
-           camera.position.set(0, 500, 0);
+           camera.position.set(500, 500, 0)
```

これでグリッドに遠近感がついたと思います。

次は少しカメラを動かしてみましょう。

` animate() `内を書き換えてカメラをy軸周りに回転させるようにしてみます。

```diff
            // 再描画の準備が整い次第実行される
            requestAnimationFrame(animate);

            // 描画
            render();

+            // count を degree から radian に変換する
+            var theta = count * Math.PI / 180.0;

+            // camera をy軸周りに回転させる
+            camera.position.set(500 * Math.cos(theta), 500, 500 * Math.sin(theta))

+            // camera の視点は固定する
+            camera.lookAt(new THREE.Vector3(0, 0, 0));

+            // 1フレームずつカウントする
+            ++count;
```

無断で` count `を使ってますが、` init() `が呼ばれる前あたりで

```
var count = 0;
```

と宣言してください。
こうすることで、` animate() `が呼ばれるたびにその回数を` count `が数えてくれます。

これを保存してブラウザで表示すれば、**3Dアニメーションの完成です！**

## まとめ

「で、何？」と言われたらもうオシマイですが、**「変なソフト入れなくても、プログラムで、3DCGできます！」**
ということだけ伝わったらいいのかなと思います。

自分はJavaSctiptを触ったことがなく、まだ詳しいことはわかってないので、**「この辺がおかしい」「こうあるべきだ」**みたいな疑問点を感じたのであれば、[three.jsのドキュメント](http://threejs.org/docs/)を覗いて見るか、` three.js `ファイルの中身を探検するなどして、**私ならこう書きます**とクレームをつけて頂けると幸いです。

**「何ができるのかわからん」**という場合には[three.jsのホームページ](http://threejs.org/)には魅力的なサンプルがあるので、実際に触ってみて` three.js `でできることを体感してもいいと思います。

今回の記事では、グリッドを表示して回転させることしかしませんでしたが、**` three.js `でアニメーションをする術**は手に入れたわけです。

` three.js `では**物体を描画する**ことも、もちろんできます。` Geometry `を使い**物体の形状を作り**、` Material `によって**物体の表面を作り**、それを` Light `によって**物体を照らす**ことで物体を描画することができます。

どうやって物体を作っていくか、次回記事を書く機会があれば簡単に説明したいと思います。

最後まで、この記事を見て下さってありがとうございました。

## 完成コード

今回の記事で扱ったソースコードの完成形をここに載せます。「なぜか動かない」っていうときはこちらから参照してみてください。

```
<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8" />
    <title>threejsTest</title>

</head>
<body>
    <div id="container">Generating world...</div>
    <script src="./three.js"></script>
    <script>
        // レンダラが描画したものを表示するコンテナ
        var container;

        // three.js のカメラ・シーン・レンダラを使用する
        var camera, scene, renderer;

        var count = 0;

        // init して animate でループする
        init();
        animate();

        function init() {

            // カメラを設定
            camera = new THREE.PerspectiveCamera(60, window.innerWidth / window.innerHeight, 1, 20000);
            camera.position.set(500, 500, 0);
            camera.lookAt(new THREE.Vector3(0, 0, 0));

            // オブジェクトを管理するシーン
            scene = new THREE.Scene();

            // オブジェクト初期化
            // グリッド表示
            var grid = new THREE.GridHelper(1000, 50);
            grid.setColors(0xffffff, 0xaacc00);
            scene.add(grid);
            
            // 描画更新を管理するレンダラの初期化
            // WebGLを使用する
            renderer = new THREE.WebGLRenderer();

            // 描画する色・比率・サイズの初期化
            renderer.setClearColor(0x000000);
            renderer.setPixelRatio(window.devicePixelRatio);
            renderer.setSize(window.innerWidth, window.innerHeight);

            // div の container を拾ってくる
            container = document.getElementById('container');

            // container の要素を初期化
            container.innerHTML = "";

            // container で renderer の描画を映す
            container.appendChild(renderer.domElement);

        }

        function animate() {

            // 再描画の準備が整い次第実行される
            requestAnimationFrame(animate);

            // 描画
            render();

            // count を degree から radian に変換する
            var theta = count * Math.PI / 180.0;

            // camera をy軸周りに回転させる
            camera.position.set(500 * Math.cos(theta), 500, 500 * Math.sin(theta))

            // camera の視点は固定する
            camera.lookAt(new THREE.Vector3(0, 0, 0));

            // 1フレームずつカウントする
            ++count;
           
        }

        function render() {

            // シーン・カメラ情報を基にして2Dに描画
            renderer.render(scene, camera);

        }
    </script>
</body>
</html>

```