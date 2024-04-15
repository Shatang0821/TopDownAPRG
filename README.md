# コード命名規則

このプロジェクトは以下のコード命名規則に従います。

## クラス名と構造体名

- **PascalCase**を使用します。
- 例：`PlayerController`、`GameManager`、`EnemyAI`。

## メソッド名

- **PascalCase**を使用します。
- メソッドはその機能を明確に示すべきです。
- 例：`CalculateDamage`、`MovePlayer`、`ShowMenu`。

## 変数名

- プライベート変数は**camelCase**を使用し、前にアンダースコア`_`を付けます。
- パブリック変数は**PascalCase**を使用します。
- 例：プライベート`_health`、パブリック`PlayerSpeed`。

## ローカル変数とパラメータ

- **camelCase**を使用します。
- 例：`initialHealth`、`moveSpeed`。

## 定数と静的読み取り専用フィールド

- **PascalCase**または**ALL_CAPS**を使用します。
- 例：`const int MaxHealth = 100;`、`static readonly int DefaultSpeed = 5;`。

## プロパティ

- **PascalCase**を使用します。
- 例：`public int Health { get; private set; }`、`public bool IsDead { get; }`。

## イベント

- **PascalCase**を使用します。
- イベント名は通常、動詞または動詞句です。
- 例：`public event Action OnDeath;`、`public event Action<int> OnScoreChanged;`。

## インターフェース

- 大文字の **I** で始まり、その後に**PascalCase**が続きます。
- 例：`interface IMovable`、`interface IHealthSystem`。

## 列挙型

- 型とメンバーには**PascalCase**を使用します。
- 例：`enum GameState { Running, Paused, Ended }`。
