# スクリプトのアタッチ対象一覧

## 📋 **アタッチが必要なスクリプト**

### 🎯 **メインスクリプト（どちらか一つを選択）**

#### 1. `BoneController.cs` - 基本版
**アタッチ対象**: MMDモデルのGameObject
```
対象: MMDモデルのGameObject（MMD4MecanimModelがついているオブジェクト）
例: "AyundaRisu"、"MMDModel"、"Character" など
```

#### 2. `BoneControllerAdvanced.cs` - 高機能版  
**アタッチ対象**: MMDモデルのGameObject
```
対象: MMDモデルのGameObject（MMD4MecanimModelがついているオブジェクト）
例: "AyundaRisu"、"MMDModel"、"Character" など
```

---

## 🚫 **アタッチ不要なスクリプト（補助ファイル）**

### 📄 **自動で機能するスクリプト**

#### 3. `ReadOnlyAttribute.cs`
```
アタッチ対象: なし
用途: 属性定義（自動で認識される）
場所: Assets/scripts/ModelOptions/
```

#### 4. `ReadOnlyPropertyDrawer.cs`
```
アタッチ対象: なし  
用途: エディター拡張（自動で機能する）
場所: Assets/scripts/Editor/
```

#### 5. `BoneSettings.cs`
```
アタッチ対象: なし
用途: データクラス定義（将来の拡張用）
場所: Assets/scripts/ModelOptions/
```

#### 6. `MouseInputHandler.cs`
```
アタッチ対象: なし
用途: 静的メソッド集（将来の拡張用）
場所: Assets/scripts/ModelOptions/
```

#### 7. `BoneAnimationConfig.cs`
```
アタッチ対象: なし
用途: ScriptableObject定義（将来の拡張用）
場所: Assets/scripts/ModelOptions/
```

---

## 🎮 **具体的なアタッチ手順**

### Step 1: 対象オブジェクトを見つける
```
Hierarchy ウィンドウで以下を探す:
├── Scene
    ├── MMDモデル ← このGameObjectを選択
    │   ├── MMD4MecanimModel ← これがついているオブジェクト
    │   ├── Animator
    │   ├── その他のコンポーネント
    │   └── 子オブジェクト（ボーン構造）
```

### Step 2: アタッチ作業
1. **MMDモデルのGameObjectを選択**
2. **Inspector ウィンドウで `Add Component` をクリック**
3. **検索ボックスに入力**:
   - 基本版: `Bone Controller`
   - 高機能版: `Bone Controller Advanced`
4. **該当するスクリプトを選択**

---

## ✅ **正しいアタッチの確認方法**

### 確認ポイント
```
選択したGameObjectのInspectorに以下が表示されていればOK:

✓ MMD4MecanimModel (Script)
✓ Bone Controller (Script)        ← 基本版の場合
または
✓ Bone Controller Advanced (Script) ← 高機能版の場合
```

### アタッチ後の設定確認
```
基本版の場合:
├── Turn Proportional Bias: 4.5
└── Current Mouse Input: (表示のみ)

高機能版の場合:  
├── Turn Proportional Bias: 4.5
├── Enable Mouse Tracking: ✓
├── Initial Pose Settings: Size 2
├── Mouse Tracking Settings: Size 3  
└── Debug Info: Current Mouse Input, Registered Bones Count
```

---

## 🔍 **トラブルシューティング**

### よくある間違い
❌ **間違ったオブジェクトにアタッチ**
```
× カメラにアタッチ
× 空のGameObjectにアタッチ  
× 子オブジェクト（ボーン）にアタッチ
```

✅ **正しいアタッチ**
```
○ MMDモデルのルートGameObjectにアタッチ
○ MMD4MecanimModelと同じオブジェクトにアタッチ
```

### エラーが出る場合
```
"MMD4MecanimModel component not found"
→ MMD4MecanimModelがないオブジェクトにアタッチしている

"Bone 'ボーン名' not found"  
→ ボーン名が間違っているか、モデルにそのボーンがない
```

---

## 📝 **まとめ**

### アタッチが必要
- ✅ `BoneController.cs` または `BoneControllerAdvanced.cs` のみ
- ✅ アタッチ先: MMDモデルのGameObject

### アタッチ不要  
- ✅ その他のスクリプトはすべて補助ファイル
- ✅ 自動で認識・機能する

### 重要なポイント
1. **一つのモデルには一つのコントローラーのみ**
2. **MMD4MecanimModelと同じオブジェクトにアタッチ**  
3. **基本版と高機能版は同時使用不可**
