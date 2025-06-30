using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MMDモデルのボーンをマウス入力に基づいて制御するコントローラー
/// </summary>
public class BoneController : MonoBehaviour
{
    [Header("基本設定")]
    [SerializeField, Range(0.1f, 10f)]
    private float turnProportionalBias = 4.5f;
    
    [Header("デバッグ情報")]
    [SerializeField] // 読み取り専用 - 現在のマウス入力を表示
    private Vector2 currentMouseInput;
    
    // コンポーネント参照
    private MMD4MecanimModel model;
    
    // ボーン管理
    private Dictionary<string, int> boneIndexMap = new Dictionary<string, int>();
    private HashSet<int> modifiedBoneIndices = new HashSet<int>();
    
    // ボーンの設定データ
    private Dictionary<string, BoneMotionSettings> boneSettings;
    
    /// <summary>
    /// ボーンの動作設定を格納する構造体
    /// </summary>
    [System.Serializable]
    public struct BoneMotionSettings
    {
        public Vector3 motionMultiplier;
        public Vector3 motionLimit;
        public Vector3 initialRotation;
        
        public BoneMotionSettings(Vector3 multiplier, Vector3 limit, Vector3 initial = default)
        {
            motionMultiplier = multiplier;
            motionLimit = limit;
            initialRotation = initial;
        }
    }
    
    void Start()
    {
        InitializeComponents();
        InitializeBoneSettings();
        BuildBoneIndexMap();
        ApplyInitialPose();
    }
    
    void Update()
    {
        UpdateMouseTracking();
    }
    
    /// <summary>
    /// コンポーネントの初期化
    /// </summary>
    private void InitializeComponents()
    {
        model = GetComponent<MMD4MecanimModel>();
        
        if (model == null)
        {
            Debug.LogError($"MMD4MecanimModel component not found on {gameObject.name}");
            enabled = false;
        }
    }
    
    /// <summary>
    /// ボーン設定の初期化
    /// </summary>
    private void InitializeBoneSettings()
    {
        boneSettings = new Dictionary<string, BoneMotionSettings>
        {
            // 初期ポーズ用設定
            ["右腕"] = new BoneMotionSettings(Vector3.zero, Vector3.zero, new Vector3(0, 0, -30)),
            ["左腕"] = new BoneMotionSettings(Vector3.zero, Vector3.zero, new Vector3(0, 0, 30)),
            
            // マウス追従用設定
            ["頭"] = new BoneMotionSettings(
                new Vector3(1f, 1f, 0.5f),      // 動作倍率
                new Vector3(20f, 10f, 5f)       // 動作制限
            ),
            ["首"] = new BoneMotionSettings(
                new Vector3(0.5f, 0.5f, 0.5f),  // 動作倍率
                new Vector3(6f, 18f, 9f)        // 動作制限
            ),
            ["上半身"] = new BoneMotionSettings(
                new Vector3(0.5f, 0.5f, 0.5f),  // 動作倍率
                new Vector3(2f, 6f, 3f)         // 動作制限
            )
        };
    }
    
    /// <summary>
    /// ボーン名とインデックスのマップを構築
    /// </summary>
    private void BuildBoneIndexMap()
    {
        boneIndexMap.Clear();
        
        foreach (var bone in model.boneList)
        {
            if (!boneIndexMap.ContainsKey(bone.boneData.nameJp))
            {
                boneIndexMap.Add(bone.boneData.nameJp, bone.boneID);
            }
        }
    }
    
    /// <summary>
    /// 初期ポーズを適用
    /// </summary>
    private void ApplyInitialPose()
    {
        SetBoneRotation("右腕", boneSettings["右腕"].initialRotation);
        SetBoneRotation("左腕", boneSettings["左腕"].initialRotation);
    }
    
    /// <summary>
    /// マウス追従の更新処理
    /// </summary>
    private void UpdateMouseTracking()
    {
        // マウス入力を正規化
        currentMouseInput = GetNormalizedMouseInput();
        
        // 各ボーンの回転を計算・適用
        ApplyBoneRotation("頭", currentMouseInput);
        ApplyBoneRotation("首", currentMouseInput);
        ApplyBoneRotation("上半身", currentMouseInput);
    }
    
    /// <summary>
    /// 正規化されたマウス入力を取得
    /// </summary>
    private Vector2 GetNormalizedMouseInput()
    {
        Vector3 mousePosition = Input.mousePosition;
        float normalizedX = (mousePosition.x / Screen.width) * 2f - 1f;
        float normalizedY = (mousePosition.y / Screen.height) * 2f - 1f;
        return new Vector2(normalizedX, normalizedY);
    }
    
    /// <summary>
    /// ボーンの回転を適用
    /// </summary>
    private void ApplyBoneRotation(string boneName, Vector2 mouseInput)
    {
        if (!boneSettings.TryGetValue(boneName, out BoneMotionSettings settings))
            return;
        
        Vector3 rotation = CalculateBoneRotation(mouseInput, settings);
        SetBoneRotation(boneName, rotation);
    }
    
    /// <summary>
    /// ボーンの回転角度を計算
    /// </summary>
    private Vector3 CalculateBoneRotation(Vector2 mouseInput, BoneMotionSettings settings)
    {
        // 基本回転を計算
        Vector3 rawRotation = new Vector3(
            turnProportionalBias * settings.motionMultiplier.x * -mouseInput.y,
            turnProportionalBias * settings.motionMultiplier.y * -mouseInput.x,
            turnProportionalBias * settings.motionMultiplier.z * mouseInput.x
        );
        
        // 制限を適用
        return new Vector3(
            ClampRotation(rawRotation.x, settings.motionLimit.x),
            ClampRotation(rawRotation.y, settings.motionLimit.y),
            ClampRotation(rawRotation.z, settings.motionLimit.z)
        );
    }
    
    /// <summary>
    /// 回転角度を制限内に収める
    /// </summary>
    private float ClampRotation(float value, float limit)
    {
        if (limit <= 0) return 0f;
        return Mathf.Abs(value) <= limit ? value : limit * Mathf.Sign(value);
    }
    
    /// <summary>
    /// ボーンの回転を設定
    /// </summary>
    private void SetBoneRotation(string boneName, Vector3 rotation)
    {
        if (!boneIndexMap.TryGetValue(boneName, out int boneIndex))
        {
            Debug.LogWarning($"Bone '{boneName}' not found in model");
            return;
        }
        
        model.boneList[boneIndex].userEulerAngles = rotation;
        modifiedBoneIndices.Add(boneIndex);
    }
}