# python で O/R マッパー

python わかんないし、データベースわかんないし、とりあえず検索して勉強しよう。

私の python のバージョンはこんな感じ

```
Python 2.7.9
```

参考にさせてもらったサイトはこちら。

* [O/R マッパー経由でデータベースを使う](https://skitazaki.github.io/python-school-ja/csv/csv-6.html)

## import 文って何

### これ

```python
import csv
import datetime

from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy import Column, Integer, Float, String, Date

from pyschool.cmdline import parse_args
```

**全くわからん**

というわけで検索「python import文」

参考サイト

* [Pythonのimport文について](http://d.hatena.ne.jp/kakurasan/20090306/p1)

>import文では、指定した名前のPythonスクリプトやモジュール/パッケージを調べて、存在しない場合には例外(ImportError)を出し、存在する場合にはそのコードを実行する(中の変数や関数が読み込まれて利用可能となる)。

なるほど。コードを実行するらしい。どこにあるのかは知らんけど。(pythonフォルダのLibにあるらしい)

> 基本

```python
import sys
print (sys.argv)
```

> モジュール名を飛ばしたいとき

```python
from [モジュール名] import [使用したい変数や関数]

from xml.parsers.expat import ParserCreate
p = ParserCreate ()
``` 

### とりあえず

これ以上はピンと来ない。とりあえず ```import``` で指定したやつはキーワードとして使用できる。

```cs
using System
```

みたいな感じ(C\#)だろうか。

関数とかも```()```なしで使われているので、実際に使うときは 

```python
import hoge
hoge()
```

みたいな感じ。


**import文を確認してfrom下で動作確認**


### ちなみに

>* importではワイルドカードとかも使える
* import ~ as とかも書いてあるとよいかも

と話を受けたのでちょっと調べもの。

「python import ワイルドカード」検索

参考サイト

* [モジュール(2)](https://python.g.hatena.ne.jp/muscovyduck/?of=40)

```python
>>> from fibo import *
>>> fib(500)
1 1 2 3 5 8 13 21 34 55 89 144 233 377
>>>
```

> ワイルドカード(*)は、基本的には全ての名前をimportするけど、アンダースコア(_)で始まる名前のものはimportしないよ。

という話。

でも、これを使うと「何の変数を使う」とか「何の関数を使う」とかが良く分からない。

ワイルドカード使わない方がコードが読みやすいかなあ…と思う限りです。

「python import as」検索

参考サイト

* [Pythonのモジュールインポートのしくみ](http://python.matrix.jp/pages/tips/import.html)

>「as」は別名を名づけするためのもので、以下のような使われ方です。

```
import A.B as C
from D.E.F import G.H.I as J
```

>こうすると、複雑なパッケージに含まれるオブジェクトにシンプルな名前でアクセスできます。

>別名をつけるのは混乱を招きやすいので 互換モジュールを指定する時などを除き やたらと使うのはやめたほうがいいと思います。

`as` で別名を付けることができるらしい。適度に使えば読みやすくできそう。

## スネークキャメルの変数…？

```python
DEFAULT_SQLITE_FILE = ':memory:'
```

「ねぇ、これ何？」

「いや、Pythonにそんな書き方あんの？定数か？pep見ろよ」

「pep python」で検索～

...数十分後...

**わからない**

pep8? ああ、なるほど。

「pep8」検索

* [pep8](https://github.com/mumumu/pep8-ja/blob/master/index.rst)

>定数

>定数は通常モジュールレベルで定義します。全ての定数は大文字で書き、単語をアンダースコアで区切ります。例として MAX_OVERFLOW や TOTAL があります。

なるほど、あるらしい。定数でいいか。

## 連想配列

```
FIELDS = (
    {'id': 'day', 'type': 'datetime', 'format': '%Y-%m-%d'},
    {'id': 'price_begin', 'type': 'float'},
    {'id': 'price_max', 'type': 'float'},
    {'id': 'price_min', 'type': 'float'},
    {'id': 'price_end', 'type': 'float'}
)
```

連想配列。以上。

```
hoge = {}
```

で空の連想配列	(dict)

```
hoge = []
```

で空の配列(list)

…だった気がする。

list と dict は違うので注意されたし。

連想配列は 

```
{ key: value, key: value }
```

という形式で。

「python 配列 連想配列」検索

参考サイト

* [文字列・配列・連想配列](http://d.hatena.ne.jp/uyamae/20071023/1193264069)

## 知らない関数 sessionmaker()

```python
Session = sessionmaker()
Base = declarative_base()
```

```import``` にあった。以上。実装は必要なときに、 python の Lib 下にあるモジュールを見る。

```python
from sqlalchemy.orm import sessionmaker
from sqlalchemy.ext.declarative import declarative_base
```

この場合は sqlalchemy フォルダにある orm を見れば sessionmaker があって、sqlalchemyの ext にある declarative という部分を見れば、 declarative_base の内容がわかるらしい。

## クラス StockPrice

```python
class StockPrice(Base):

    __tablename__ = 'stock_price'

    id = Column(Integer, primary_key=True)
    day = Column(Date, nullable=False, unique=True)
    price_begin = Column(Float, nullable=False)
    price_max = Column(Float, nullable=False)
    price_min = Column(Float, nullable=False)
    price_end = Column(Float, nullable=False)

    def __repr__(self):
        return "<StockPrice('{}')>".format(self.day)

    def diff(self):
        return self.price_end - self.price_begin
```

if文とかfor文とかのスコープは、**コロン**と**インデント**だよね。それはわかる。

「class python」検索

参考サイト

* [Python入門 - クラス](http://www.tohoho-web.com/python/class.html)

クラス(super()) という書き方らしい。

### 親クラス Base

`Base` っていう親クラスがある…ということか。カンマで多重継承もできるとか。

「base クラス python」検索

…数十分後…

予約語かなんかかと思ったけど全く出てこない。わからん。

()内が引数かなんかになるのだろうか。その線でもうちょいクラスを調べる。

…数分後…

**見つからない**

そもそも動的に継承元変えられるとか意味わからん。

```python
Base = declarative_base()
```

(三行前にありました。。。)

### \__変数名__ is 何

なんかシステムで使ってる変数らしい。

モジュールとかクラスとかの内部に書き書きするタイプのやつ。

### このクラス、テーブルクラスらしい

```python
    id = Column(Integer, primary_key=True)
    day = Column(Date, nullable=False, unique=True)
    price_begin = Column(Float, nullable=False)
    price_max = Column(Float, nullable=False)
    price_min = Column(Float, nullable=False)
    price_end = Column(Float, nullable=False)
```

色々変数に代入してるけど、それに使われてるのは `import` したやつ。

```python
from sqlalchemy import Column, Integer, Float, String, Date
```

`Column` の内容はわからないけど、`id` とかもろもろデータベースっぽい。このクラスはとあるテーブルの定義らしい。

### \__repr__ is 何

「__ 変数 python」検索

こんな感じに調べてたら、

* [Pythonの変数名のつけ方](http://www.lifewithpython.com/2013/02/rules-for-naming-identifiers-in-python.html)

* [Lexical analysis](https://docs.python.org/2/reference/lexical_analysis.html#reserved-classes-of-identifiers)

* [Data model](https://docs.python.org/2/reference/datamodel.html#specialnames)

参考サイトが色々出てきた。

**Data model** の方で「\__repr__」で検索。

私の脳には string を返すくらいにしかわからない。まあ、`toString()`という体で行きたい。

```python
    def __repr__(self):
        return "<StockPrice('{}')>".format(self.day)
```

文字列フォーマットを返してるっぽい。

#### このフォーマット何？

というわけで2つ検索してみました。

「python string format」検索

参考サイト

* [[python]文字列フォーマットまとめ(format関数, %dなど)](http://dackdive.hateblo.jp/entry/2014/11/15/001318)

>基本形

>format() 関数を使った場合、上記の一番最初の文は次のような書き方になります。

```python
	>>> print 'Hello, {}!'.format('World')
	Hello, World!
```

多分この辺、```{}``` に ```format()``` の引数が入る。


「python ダブルクォーテーション シングルクォーテーション」検索

参考サイト

* [エスケープシーケンス](http://www.python-izm.com/contents/basis/escape_sequence.shtml)

```python
# -*- coding: utf-8 -*-
if __name__ == "__main__":
    print '1234¥'567890'
    print "1234'567890"
```

	--実行結果--
	1234'567890
	1234'567890

という感じらしい。一番外のクォーテーションを見る。

```python
return "<StockPrice('{}')>".format(self.day)
```

これは、多分

```python
return "<StockPrice('"+self.day+')>".format(self.day)
```
みたいな感じですかね。

```python
    def diff(self):
        return self.price_end - self.price_begin
```

差を返す ```diff()``` を定義する。

見る限り株の終値から始値をひいた感じ。

何て名前だったか。

## 飛ばすぜー

```python
def process(args):
```

引数付きの関数定義

```python
    """Parse daily Tokyo stock prices, and calculate up/down.
    After that, import them into SQLite database.
    """
```

コメント


「python ダブルクォーテーション 3つ」検索

参考サイト

* [http://d.hatena.ne.jp/SumiTomohiko/20070606/1181161100](http://d.hatena.ne.jp/SumiTomohiko/20070606/1181161100)

>文字列リテラルには、ダブルクォーテーションひとつ (") でくくるものと、ダブルクォーテーション3つ (""") でくくるものの2種類があります。ダブルクォーテーション3つでくくると、複数行に渡る文字列を記述することができます。

```python
	>>> s = "foo"
	>>> print s
	foo
	>>> s = """foo
	... bar
	... baz"""
	>>> print s
	foo
	bar
	baz
```

まあ、こんな使い方もあるということで。

## SQLAlchemy の起動

### dslとは

```python
    dsl = 'sqlite:///' + (args.output or DEFAULT_SQLITE_FILE)
    engine = create_engine(dsl, echo=True)
    Base.metadata.create_all(engine)
    Session.configure(bind=engine)
    session = Session()
```

「dsl sql」検索

参考サイト

* [DSLについて考える](http://kozy4324.github.io/blog/2012/09/18/thinking-about-dsl/)

なるほど、狭い範囲で特化した言語…と。

**飛ばそう**

```dsl``` にパス入れて、```engine``` で ```sqlalchemy``` を立ち上げる感じですかね。

```engine``` に関して ```Base```, ```Session``` を作って置くという感じで…（適当）

この辺 ```import``` してるものを把握しないとわからんです。python に慣れるという意味で、**どこをみればいいか**だけわかればいいと思います。

### with as

```python
    with open(args.filename[0]) as fp:
        ...
```

with as って何だ…。

C\#だったらアップキャストしたりとかあったはず…

```fp``` の定義がないあたりはこのスコープで ```fp``` っていう変数を使ってるはず。

まあ、 ```fp``` の中身は多分 ```open(args.filename[0])```

「with as python」検索

参考サイト

* [Pythonのwithステートメントのまとめ](http://shin.hateblo.jp/entry/2013/03/23/211750)

>まず、一番身近なファイル操作の例を載せておきます。

```python
with open("...") as f:
    print(f.read())
```

>これは、以下と同等です。

```python
f = open("...")
print(f.read())
f.close()
```

ファイル操作が楽になるやつっぽい。なるほどね。

### reader から値を格納する
というわけで、```json``` 形式なファイルが ```t``` に入ってるはず。その仮定で行こう。

```python
        reader = csv.reader(fp)  # Instantiate CSV reader with file pointer.
        for t in reader:
            # Convert input values to declared name and type.
            dt = {}
            for i, f in enumerate(FIELDS):
                if f['type'] == 'integer':
                    dt[f['id']] = int(t[i])
                elif f['type'] == 'float':
                    dt[f['id']] = float(t[i])
                elif f['type'] == 'datetime':
                    dt[f['id']] = datetime.datetime.strptime(t[i], f['format'])
                else:
                    dt[f['id']] = t[i]
```

```python
for i, f in enumerate(FIELDS):
```

```i, f``` つまり ```index, value``` を取り出す。 ```value``` は連想配列ですね（FIELDSの構造から察して）

これを ```type``` ごとに格納していく。 ```type``` ごとに ```id``` が違うので、条件分岐を行う必要があった。

最終的には、`id` の数だけ `key` を持つ `dt` というデータ用の連想配列が完成するはず。もちろん `value` は `id` に対応した数値、フォーマットとなる。

ちなみに `enumrate()` について

「enumerate python」検索

参考サイト

* [forループで便利な zip, enumerate関数](http://python.civic-apps.com/zip-enumerate/)

>enumerate関数 インデックスとともにループ
>ループする際にインデックスつきで要素を得ることができる。

```python
>>> list1 = ['a', 'b', 'c']
>>> for (i, x) in enumerate(list1):
...   print i,x
... 
0 a
1 b
2 c
```

インデックスを振ってくれるのが `enumrate()`

## 可変長引数

```python
            # Instantiate SQLAlchemy data model object.
            p = StockPrice(**dt)
```

データを `StockPrice` としてラップしてる感じですかね。気になるのは `**dt`

「python ** 引数」検索

参考サイト

* [[Python] 可変長引数あれこれ](http://blog.taikomatsu.com/2009/03/13/python-%E5%8F%AF%E5%A4%89%E9%95%B7%E5%BC%95%E6%95%B0%E3%81%82%E3%82%8C%E3%81%93%E3%82%8C/)

`key` 無し `key` 付きを扱えるわけ…

`**dt` は dict型の可変長引数ということで。

## diff を取って文字列出力

```python
            # Show the same things with previous scripts.
            diff = p.diff()
            if diff > 0:
                message = 'up'
            elif diff < 0:
                message = 'down'
            else:
                message = 'same'
            # Write out day, up/down/same, and diff.
            print('{}\t{:5}\t{}'.format(p.day, message, round(diff, 2)))
            session.add(p)
    # Don't forget to commit the changes you add.
    session.commit()
```

まあ、あとはわかりそう。場合分けで文字出力を変えるだけですね。 `diff` は株価変動の値です。

`add` したら `commit` がお決まりらしいので、このへんは、よしなに。

やってることはcsv to sqlalchemy?

## main

```python
def main():
    args = parse_args()
    process(args)


def test():
    pass

if __name__ == '__main__':
    main()
```

実際に実行するところについて…。

`pass` `test` __name__ == __main__ については後で調べましょう。

「if __name__ == '__main__':」検索

参考サイト

* [if __name__ == '__main__': について](http://d.hatena.ne.jp/s-n-k/20080512/1210611374)

この辺抜粋

```python
#! /usr/bin/env python
# -*- coding: utf-8 -*-

print 'import でも実行される  __name__ = %s' % __name__

if __name__ == '__main__':
    print 'スクリプトの時だけ  __name__ = %s' % __name__
```

>んで、普通に実行するとこんな感じ

```sh
 $ python test.py
import でも実行される  __name__ = __main__
スクリプトの時だけ  __name__ = __main__
```

>今度は Python インタープリタで import してみるとこんな感じ

```sh
 $ python
 >>> import test
 import でも実行される  __name__ = test
 >>>
```

__name__が変わるというお話。なるほどね。

「python pass文」検索

参考サイト

* [Python には何もしない文 pass がある](http://d.hatena.ne.jp/r_ikeda/20110517/pass)

>Python には pass 文があり、空の関数定義だけ作るときや、例外処理で何もしないときに pass 文を使える。

明示的に何もしない感じですか。

**Pythonの勉強終わり！！**

んで…

# このプログラム is 何

## 株価csv とってこような

日経株価平均の日足をとってきた

実行なう

実行できない。

pyschoolのモジュールがない…? pase_argsがインポートできない…?

ああ、適当に検索して出てきたこの講座は1コ完結じゃないのか。自作モジュールらしい。

* [クラスを定義する](https://skitazaki.github.io/python-school-ja/csv/csv-5.html)

>パッケージング

>スクリプトをたくさん書いていくと、似たような実装が様々な場所に散り散りになってしまいます。 全てをフラットに配置すると依存関係の把握が難しくなってしまいますので、パッケージとしてまとめます。 ここでは、引数処理の関数をパッケージとして共有しておきましょう。 プロジェクト内で共通となるデータ構造やクラス定義もまとめておくと良いでしょう。

```
pyschool
        __init__.py
        cmdline.py
```

1つ講義を遡ってみたらこんな感じにパッケージをつくればいいとのこと。

python の Lib 下に上のようにパッケージフォルダ pyschool を置いておく。

`__init__.py` はお決まり。適当にコメントだけ残しとく。

`cmdline.py` は以下の通り。ほぼほぼ参考サイトのままです。

```python
import argparse
import logging

def parse_args():
    """Parse arguments and set up logging verbosity.

    :rtype: parsed arguments as Namespace object.
    """
    parser = argparse.ArgumentParser()
    parser.add_argument("-f", "--file", dest="filename",
                        help="setting file", metavar="FILE")
    parser.add_argument("-o", "--output", dest="output",
                        help="output file", metavar="FILE")
    parser.add_argument("-n", "--dryrun", dest="dryrun",
                        help="dry run", default=False, action="store_true")
    parser.add_argument("-v", "--verbose", dest="verbose", default=False,
                        action="store_true", help="verbose mode")
    parser.add_argument("-q", "--quiet", dest="quiet", default=False,
                        action="store_true", help="quiet mode")
    # Add this line from boilerplate.
    parser.add_argument("filename", nargs=1, help="CSV file path")

    args = parser.parse_args()

    if args.verbose:
        logging.basicConfig(level=logging.DEBUG)
    elif not args.quiet:
        logging.basicConfig(level=logging.INFO)

    return args
```

こんな設定をしたのち、いざ実行。

通った！

`csv-6.sqlite` ができた。

## 中身を確認してみよう

「python sqlite」検索

参考サイト

* [Pythonのsqlite3ライブラリでデータベースを操作しよう](http://msrx9.bitbucket.org/blog/html/2013/07/04/db_study.html)

直接クエリ文を発行する感じ。

これを参考にデータベースにアクセスしてみる。

>データベースへの接続とカーソル

```python
# データベースへ接続
>>> conn = sqlite3.connect('./sqlite.db')

# カーソルの作成
>>> cur = conn.cursor()
```

>まずは、 ./sqlite.db データベースへ接続をします。 データベースが存在しない場合は新規作成をしてから接続、存在している場合にはそのまま接続をします。 次に、カーソルの作成をすればデータベースを操作する準備は完了です。


>初めてカーソルという単語を見た時に一体何のためのものなのかが理解できませんでした。 全然違っているのかもしれませんが、調べてみた感じでは以下の様な意味で良いのかなと…

>* コネクションオブジェクト  
データベース自体を制御するためのオブジェクト

>* カーソルオブジェクト  
データベース内のデータを制御するためのオブジェクト（データベースへ接続していないと作成できない）

このへんの処理をして `csv-6.sqlite` にコネクションを作って、制御用にカーソルを作ってあげると、クエリ文でいじくることができる。

あとは `cur.execute("""クエリ文""")` という風にすれば、データベースを直接いじれるわけですね。

### 中身の表示

> SELECT文を用いてデータの抽出、Pythonを使って結果の出力

> 実際にPython上でテーブルの内容を表示してみます。

```python
>>>
# テーブルの内容を表示
>>> cur.execute("""SELECT name, price FROM shop;""")
>>> for name, price in cur.fetchall():
...     print u"%sは、%s円です。" % (name, price)
...
りんごは、99円です。
```

サンプル通りにやれば、おおよそ大丈夫だ。今回作ったテーブルの定義を再掲します。

例と比較すると、`shop` が `stock_price`(table_name)で、`name` や `price` が `id` `day` `price_begin`...にあたる。

```python
class StockPrice(Base):

    __tablename__ = 'stock_price'

    id = Column(Integer, primary_key=True)
    day = Column(Date, nullable=False, unique=True)
    price_begin = Column(Float, nullable=False)
    price_max = Column(Float, nullable=False)
    price_min = Column(Float, nullable=False)
    price_end = Column(Float, nullable=False)

    def __repr__(self):
        return "<StockPrice('{}')>".format(self.day)

    def diff(self):
        return self.price_end - self.price_begin
```


表示してある程度遊んだら、

>COMMITを忘れずに

```python
>>> conn.commit()
```

>今日は店じまいです。ちゃんと今日の作業の結果を保存してあげましょう。 データベースへの変更を保存するには COMMIT をする必要があります。

>COMMITを実行するには最初に作成したコネクトオブジェクトである conn の commit() メソッドを実行すればよいです。

>後片付け

```python
>>> conn.close()
```

>接続したのならちゃんと切断しましょう。 切断が完了したらデータベースの作業は終了となります。

この辺の処理を忘れずに…とのこと。

**データの格納終わり！！**