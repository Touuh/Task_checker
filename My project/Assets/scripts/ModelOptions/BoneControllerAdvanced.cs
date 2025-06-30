using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MMDモデルのボーンをマウス入力に基づいて制御するコントローラー（設定可能版）
/// </summary>
public class BoneControllerAdvanced : MonoBehaviour
{
    [Header("基本設定")]
    [SerializeField, Range(0.1f, 10f)]
    private float turnProportionalBias = 4.5f;
    
    [SerializeField]
    private bool enableMouseTracking = true;
    
    [Header("初期ポーズ設定")]
    [SerializeField]
    private List<InitialPoseSetting> initialPoseSettings = new List<InitialPoseSetting>();
    
    [Header("マウス追従設定")]
    [SerializeField]
    private List<MouseTrackingSetting> mouseTrackingSettings = new List<MouseTrackingSetting>();
    
    [Header("デバッグ情報")]
    [SerializeField] // 読み取り専用 - 現在のマウス入力を表示
    private Vector2 currentMouseInput;
    
    [SerializeField] // 読み取り専用 - 登録されたボーン数を表示
    private int registeredBonesCount;
    
    // コンポーネント参照
    private MMD4MecanimModel model;
    
    // ボーン管理
    private Dictionary<string, int> boneIndexMap = new Dictionary<string, int>();
    private HashSet<int> modifiedBoneIndices = new HashSet<int>();
    
    /// <summary>
    /// 初期ポーズの設定
    /// </summary>
    [System.Serializable]
    public struct InitialPoseSetting
    {
        [Header("ボーン設定")]
        public string boneName;
        
        [Header("初期回転角度")]
        public Vector3 rotation;
        
        public InitialPoseSetting(string name, Vector3 rot)
        {
            boneName = name;
            rotation = rot;
        }
    }
    
    /// <summary>
    /// マウス追従の設定
    /// </summary>
    [System.Serializable]
    public struct MouseTrackingSetting
    {
        [Header("ボーン設定")]
        public string boneName;
        
        [Header("動作倍率")]
        [Tooltip("X: 上下の動き, Y: 左右の動き, Z: 左右の捻り")]
        public Vector3 motionMultiplier;
        
        [Header("動作制限（角度）")]
        [Tooltip("各軸の最大回転角度")]
        public Vector3 motionLimit;
        
        [Header("その他設定")]
        [Tooltip("この設定を有効にするか")]
        public bool enabled;
        
        public MouseTrackingSetting(string name, Vector3 multiplier, Vector3 limit, bool isEnabled = true)
        {
            boneName = name;
            motionMultiplier = multiplier;
            motionLimit = limit;
            enabled = isEnabled;
        }
    }
    
    void Start()
    {
        InitializeComponents();
        BuildBoneIndexMap();
        InitializeDefaultSettings();
        ApplyInitialPose();
    }
    
    void Update()
    {
        if (enableMouseTracking)
        {
            UpdateMouseTracking();
        }
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
        
        registeredBonesCount = boneIndexMap.Count;
    }
    
    /// <summary>
    /// デフォルト設定の初期化
    /// </summary>
    private void InitializeDefaultSettings()
    {
        // 初期ポーズ設定が空の場合、デフォルトを追加
        if (initialPoseSettings.Count == 0)
        {
            initialPoseSettings.Add(new InitialPoseSetting("右腕", new Vector3(0, 0, -30)));
            initialPoseSettings.Add(new InitialPoseSetting("左腕", new Vector3(0, 0, 30)));
        }
        
        // マウス追従設定が空の場合、デフォルトを追加
        if (mouseTrackingSettings.Count == 0)
        {
            mouseTrackingSettings.Add(new MouseTrackingSetting("頭", new Vector3(1f, 1f, 0.5f), new Vector3(20f, 10f, 5f)));
            mouseTrackingSettings.Add(new MouseTrackingSetting("首", new Vector3(0.5f, 0.5f, 0.5f), new Vector3(6f, 18f, 9f)));
            mouseTrackingSettings.Add(new MouseTrackingSetting("上半身", new Vector3(0.5f, 0.5f, 0.5f), new Vector3(2f, 6f, 3f)));
        }
    }
    
    /// <summary>
    /// 初期ポーズを適用
    /// </summary>
    private void ApplyInitialPose()
    {
        foreach (var setting in initialPoseSettings)
        {
            if (!string.IsNullOrEmpty(setting.boneName))
            {
                SetBoneRotation(setting.boneName, setting.rotation);
            }
        }
    }
    
    /// <summary>
    /// マウス追従の更新処理
    /// </summary>
    private void UpdateMouseTracking()
    {
        // マウス入力を正規化
        currentMouseInput = GetNormalizedMouseInput();
        
        // 各ボーンに設定を適用
        foreach (var setting in mouseTrackingSettings)
        {
            if (setting.enabled && !string.IsNullOrEmpty(setting.boneName))
            {
                Vector3 rotation = CalculateBoneRotation(currentMouseInput, setting);
                SetBoneRotation(setting.boneName, rotation);
            }
        }
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
    /// ボーンの回転角度を計算
    /// </summary>
    private Vector3 CalculateBoneRotation(Vector2 mouseInput, MouseTrackingSetting setting)
    {
        // 基本回転を計算
        Vector3 rawRotation = new Vector3(
            turnProportionalBias * setting.motionMultiplier.x * -mouseInput.y,
            turnProportionalBias * setting.motionMultiplier.y * -mouseInput.x,
            turnProportionalBias * setting.motionMultiplier.z * mouseInput.x
        );
        
        // 制限を適用
        return new Vector3(
            ClampRotation(rawRotation.x, setting.motionLimit.x),
            ClampRotation(rawRotation.y, setting.motionLimit.y),
            ClampRotation(rawRotation.z, setting.motionLimit.z)
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
    
    /// <summary>
    /// 設定をリセット
    /// </summary>
    [ContextMenu("Reset to Default Settings")]
    public void ResetToDefaultSettings()
    {
        initialPoseSettings.Clear();
        mouseTrackingSettings.Clear();
        InitializeDefaultSettings();
    }
    
    /// <summary>
    /// 利用可能なボーン名を取得（デバッグ用）
    /// </summary>
    [ContextMenu("Debug: Print Available Bone Names")]
    public void DebugPrintBoneNames()
    {
        if (model == null) return;
        
        Debug.Log("Available Bone Names:");
        foreach (var bone in model.boneList)
        {
            Debug.Log($"- {bone.boneData.nameJp} (ID: {bone.boneID})");
        }
    }
}
