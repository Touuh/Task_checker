# BoneController セットアップガイド

## 📋 **スクリプトの配置とアタッチ手順**

### 1. **基本版を使用する場合（推奨）**

#### ファイル配置
```
Assets/
├── scripts/
│   ├── ModelOptions/
│   │   ├── BoneController.cs          ← メインスクリプト
│   │   └── ReadOnlyAttribute.cs       ← 属性定義
│   └── Editor/
│       └── ReadOnlyPropertyDrawer.cs  ← エディター拡張
```

#### アタッチ手順
1. **MMDモデルのGameObjectを選択**
2. **既存のBoneControllerコンポーネントを削除**（ある場合）
3. **新しいBoneController.csをアタッチ**
   - `Add Component` → `Scripts` → `Bone Controller`
4. **設定完了！**

---

### 2. **高機能版を使用する場合**

#### ファイル配置
```
Assets/
├── scripts/
│   ├── ModelOptions/
│   │   ├── BoneControllerAdvanced.cs  ← 高機能版メインスクリプト
│   │   └── ReadOnlyAttribute.cs       ← 属性定義
│   └── Editor/
│       └── ReadOnlyPropertyDrawer.cs  ← エディター拡張
```

#### アタッチ手順
1. **MMDモデルのGameObjectを選択**
2. **既存のBoneControllerコンポーネントを削除**
3. **BoneControllerAdvanced.csをアタッチ**
   - `Add Component` → `Scripts` → `Bone Controller Advanced`
4. **インスペクターで設定を調整**

---

## ⚙️ **設定項目の詳細**

### 基本版（BoneController）の設定
```
┌─ Bone Controller (Script) ──────────────┐
│ ✓ Script: BoneController               │
│                                        │
│ 基本設定                                │
│ Turn Proportional Bias: 4.5           │
│                                        │
│ デバッグ情報                            │
│ Current Mouse Input: (0.0, 0.0)       │
└────────────────────────────────────────┘
```

### 高機能版（BoneControllerAdvanced）の設定
```
┌─ Bone Controller Advanced (Script) ─────┐
│ ✓ Script: BoneControllerAdvanced       │
│                                        │
│ 基本設定                                │
│ Turn Proportional Bias: 4.5           │
│ ✓ Enable Mouse Tracking               │
│                                        │
│ 初期ポーズ設定                          │
│ Size: 2                               │
│ ├─ Element 0                          │
│ │  Bone Name: "右腕"                  │
│ │  Rotation: (0, 0, -30)              │
│ └─ Element 1                          │
│    Bone Name: "左腕"                   │
│    Rotation: (0, 0, 30)               │
│                                        │
│ マウス追従設定                          │
│ Size: 3                               │
│ ├─ Element 0                          │
│ │  Bone Name: "頭"                    │
│ │  Motion Multiplier: (1, 1, 0.5)     │
│ │  Motion Limit: (20, 10, 5)          │
│ │  ✓ Enabled                         │
│ ├─ Element 1                          │
│ │  Bone Name: "首"                    │
│ │  Motion Multiplier: (0.5, 0.5, 0.5) │
│ │  Motion Limit: (6, 18, 9)           │
│ │  ✓ Enabled                         │
│ └─ Element 2                          │
│    Bone Name: "上半身"                 │
│    Motion Multiplier: (0.5, 0.5, 0.5) │
│    Motion Limit: (2, 6, 3)            │
│    ✓ Enabled                         │
│                                        │
│ デバッグ情報                            │
│ Current Mouse Input: (0.0, 0.0)       │
│ Registered Bones Count: 0             │
└────────────────────────────────────────┘
```

---

## 🔧 **必須の前提条件**

### 必要なコンポーネント
1. **MMD4MecanimModel**コンポーネントが同じGameObjectにアタッチされている
2. **MMDモデル**が正しくインポートされている

### 確認方法
```
GameObject (MMDモデル)
├── MMD4MecanimModel ← これが必要
├── BoneController   ← 新しく追加
└── その他のコンポーネント...
```

---

## 📝 **設定手順の詳細**

### Step 1: 既存コンポーネントの削除
1. MMDモデルのGameObjectを選択
2. インスペクターで古い`Bone Controller`を探す
3. 右上の`⚙️`メニューから`Remove Component`

### Step 2: 新しいスクリプトをアタッチ
1. `Add Component`ボタンをクリック
2. 検索ボックスに`Bone Controller`と入力
3. 使いたいバージョンを選択
   - `Bone Controller` = 基本版
   - `Bone Controller Advanced` = 高機能版

### Step 3: 設定の調整（高機能版の場合）
1. **Turn Proportional Bias**: マウス感度（デフォルト: 4.5）
2. **Enable Mouse Tracking**: マウス追従の有効/無効
3. **初期ポーズ設定**: 起動時のボーン角度
4. **マウス追従設定**: 各ボーンの動作パラメータ

---

## 🎮 **動作確認**

### 正常に動作している場合
- ✅ コンソールにエラーが表示されない
- ✅ マウスを動かすと頭・首・上半身が連動する
- ✅ デバッグ情報でマウス入力値が更新される
- ✅ 初期ポーズ（右腕・左腕）が設定される

### トラブルシューティング
- ❌ **エラー: MMD4MecanimModel component not found**
  → MMD4MecanimModelコンポーネントを追加
- ❌ **エラー: Bone 'ボーン名' not found**
  → ボーン名を確認（日本語表記）
- ❌ **動作しない**
  → GameObjectが非アクティブでないか確認

---

## 🔄 **移行時の注意点**

### 既存プロジェクトから移行する場合
1. **必ずバックアップを取る**
2. 一度にすべてのモデルを変更せず、一つずつ確認
3. 設定値を記録しておく（感度など）
4. 動作確認を十分に行う

### 設定値の引き継ぎ
- 元の`Turn_Propotional_bias`値を`Turn Proportional Bias`に設定
- 必要に応じて微調整

---

## 💡 **おすすめの使い分け**

### 基本版を使う場合
- ✅ シンプルに使いたい
- ✅ 設定変更の頻度が少ない
- ✅ パフォーマンスを重視する

### 高機能版を使う場合
- ✅ 設定をGUIで調整したい
- ✅ 複数のボーンを個別に設定したい
- ✅ 実験的な調整を行いたい
- ✅ デバッグ情報を詳しく見たい
