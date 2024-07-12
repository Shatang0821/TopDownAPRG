# データ構造

## GameLevel

**属性:**
- `LevelName`: レベル名称
- `TriggerZones`: トリガーゾーンリスト
- `EnemySpawnPoints`: 敵生成ポイントリスト
- `Portals`: ポータルリスト
- `IsCleared`: レベルクリア状況
- `EnemiesData`: 敵データリスト
- `GridMap`: レベルのグリッドマップデータ

**用途:** レベルの基本情報と設定を保存する。

## TriggerZone

**属性:**
- `Position`: トリガーゾーンの位置
- `Radius`: トリガーゾーンの半径
- `OnPlayerEnter`: プレイヤーがゾーンに入った際のイベント

**用途:** プレイヤーが特定のエリアに入ったかどうかを検出し、対応するイベント（例えば敵の生成）をトリガーする。

## SpawnPoint

**属性:**
- `Position`: 生成ポイントの位置
- `EnemyType`: 敵のタイプ

**用途:** 敵の生成位置とタイプを保存する。

## Portal

**属性:**
- `Position`: ポータルの位置
- `TargetLevel`: 目的の関卡
- `IsActive`: ポータルの使用可否

**用途:** ポータルの位置と目的の関卡情報を管理する。

## EnemyData

**属性:**
- `Position`: 敵生成位置
- `EnemyType`: 敵のタイプ

**用途:** 敵を生成するためのデータを保存する。

## GridMap

**属性:**
- `grid`: 関卡のグリッドデータ
- `width`: グリッドの幅
- `height`: グリッドの高さ

**用途:** A*パスファインディングに使用されるグリッドデータを管理する。

## WaveConfig

**属性:**
- `enemySpawns`: 敵生成情報リスト

**用途:** 各ウェーブの敵生成情報を設定する。

# 管理クラス

## LevelManager

**属性:**
- `CurrentLevel`: 現在の関卡
- `EnemyManager`: 敵マネージャー
- `Pathfinding`: A*パスファインディングアルゴリズムのインスタンス

**方法:**
- `LoadLevel(string levelName)`: 関卡を読み込み、初期化する
- `InitializeLevel()`: 関卡ロジックを初期化する
- `SpawnEnemies(TriggerZone triggerZone)`: トリガーゾーンで敵を生成する
- `OnEnemyDefeated()`: 敵が全滅した際のロジックを処理する
- `ActivatePortals()`: ポータルをアクティブにする
- `OnPlayerEnterPortal(Portal portal)`: プレイヤーがポータルに入った際のロジックを処理する

**用途:** 関卡の読み込み、切り替え、初期化を管理する。

## EnemyManager

**属性:**
- `Enemies`: 現在の関卡の敵リスト

**方法:**
- `Initialize(List<EnemyData> enemiesData)`: 敵マネージャーを初期化し、敵データを読み込む
- `SpawnEnemy(EnemyData data)`: データに基づいて敵インスタンスを生成する
- `AddEnemy(Enemy enemy)`: 敵をマネージャーに追加する
- `RemoveEnemy(Enemy enemy)`: 敵をマネージャーから削除する
- `AreAllEnemiesDefeated()`: 全ての敵が倒されたかどうかを検出する

**用途:** 現在の関卡内の全ての敵を管理する。

## Pathfinding

**属性:**
- `GridMap`: 関卡のグリッドデータ

**方法:**
- `Initialize(GridMap map)`: パスファインダーを初期化する
- `FindPath(Vector3 start, Vector3 goal)`: 始点から目的地までの経路を計算する

**用途:** A*パスファインディングアルゴリズムを実装する。

## GameManager

**属性:**
- `Player`: プレイヤー
- `LevelManager`: 関卡マネージャー

**方法:**
- `StartGame()`: ゲームを初期化し、最初の関卡を読み込む
- `Update()`: ゲームのメインループで、プレイヤーの移動と関卡のインタラクションを処理する

**用途:** ゲーム全体の状態とメインループを管理する。

## WaveManager

**属性:**
- `waveConfigs`: ウェーブ設定リスト
- `currentWaveIndex`: 現在のウェーブインデックス

**方法:**
- `Start()`: ウェーブの敵生成を開始する
- `SpawnWaves()`: ウェーブの敵を生成する

**用途:** WaveConfigを読み込み、設定に基づいて敵を生成する。

# 実体クラス

## Entity

**属性:**
- `Name`: 実体名称
- `Health`: 実体の体力
- `Position`: 実体の現在位置

**方法:**
- `TakeDamage(int amount)`: 実体のダメージ処理
- `Die()`: 実体の死亡処理

**用途:** 共通の実体属性と方法を定義し、PlayerとEnemyの基底クラスとして機能する。

## Player

**説明:** Entityを継承し、プレイヤー特有の属性と方法を含む。

**方法:**
- `TakeDamage(int amount)`: プレイヤー特有のダメージ処理を実装する
- `Die()`: プレイヤー特有の死亡処理を実装する

**用途:** プレイヤーの行動とロジックを実装する。

## Enemy

**説明:** Entityを継承し、敵特有の属性と方法を含む。

**方法:**
- `TakeDamage(int amount)`: 敵特有のダメージ処理を実装する
- `Die()`: 敵特有の死亡処理を実装する

**用途:** 敵の行動とロジックを実装する。

## EnemyAI

**属性:**
- `detectionRadius`: 検出範囲
- `attackRadius`: 攻撃範囲
- `moveSpeed`: 移動速度

**方法:**
- `Start()`: 敵の状態機械を初期化する
- `Update()`: 敵の状態機械を更新する
- `IsPlayerInDetectionRange()`: プレイヤーが検出範囲内にいるかを検出する
- `IsPlayerInAttackRange()`: プレイヤーが攻撃範囲内にいるかを検出する
- `MoveTowardsPlayer()`: プレイヤーの位置に向かって移動する
- `StopMoving()`: 移動を停止する
- `AttackPlayer()`: プレイヤーを攻撃する
- `Die()`: 敵の死亡処理

**用途:** 敵の行動ロジックを処理し、敵の状態機械を管理する。

# インターフェース

## IDamageable

**方法:**
- `TakeDamage(int amount)`: 被ダメージ行動を定義する

**用途:** 被ダメージロジックの実装を強制するインターフェースを定義する。

# コンポーネント

## AttackComponent

**説明:** 敵の近接攻撃行動を処理するために使用される。

**用途:** 敵の攻撃範囲を検出し、攻撃アニメーションとロジックをトリガーし、ターゲットにダメージを与える。

## MoveComponent

**説明:** 敵の移動行動を処理するために使用される。

**用途:** 敵の移動方向と速度を制御し、プレイヤーを追跡する行動を実現する。
