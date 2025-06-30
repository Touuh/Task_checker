using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボーンアニメーションの設定を管理するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "BoneAnimationConfig", menuName = "Model/Bone Animation Config")]
public class BoneAnimationConfig : ScriptableObject
{
    [Header("基本設定")]
    [SerializeField, Range(0.1f, 10f)]
    private float turnProportionalBias = 4.5f;
    
    [Header("初期ポーズ設定")]
    [SerializeField]
    private List<BoneSettings> initialPoseSettings = new List<BoneSettings>();
    
    [Header("マウス追従設定")]
    [SerializeField]
    private List<BoneSettings> mouseTrackingSettings = new List<BoneSettings>();
    
    /// <summary>
    /// 基本的な回転感度バイアス
    /// </summary>
    public float TurnProportionalBias => turnProportionalBias;
    
    /// <summary>
    /// 初期ポーズの設定一覧
    /// </summary>
    public List<BoneSettings> InitialPoseSettings => initialPoseSettings;
    
    /// <summary>
    /// マウス追従の設定一覧
    /// </summary>
    public List<BoneSettings> MouseTrackingSettings => mouseTrackingSettings;
    
    /// <summary>
    /// デフォルト設定を作成
    /// </summary>
    [ContextMenu("Create Default Settings")]
    private void CreateDefaultSettings()
    {
        // 初期ポーズ設定
        initialPoseSettings.Clear();
        initialPoseSettings.Add(new BoneSettings
        {
            boneName = "右腕",
            initialRotation = new Vector3(0, 0, -30)
        });
        initialPoseSettings.Add(new BoneSettings
        {
            boneName = "左腕",
            initialRotation = new Vector3(0, 0, 30)
        });
        
        // マウス追従設定
        mouseTrackingSettings.Clear();
        
        // 頭の設定
        mouseTrackingSettings.Add(new BoneSettings
        {
            boneName = "頭",
            motionMultiplier = new Vector3(1f, 1f, 0.5f),
            motionLimit = new Vector3(20f, 10f, 5f)
        });
        
        // 首の設定
        mouseTrackingSettings.Add(new BoneSettings
        {
            boneName = "首",
            motionMultiplier = new Vector3(0.5f, 0.5f, 0.5f),
            motionLimit = new Vector3(6f, 18f, 9f)
        });
        
        // 上半身の設定
        mouseTrackingSettings.Add(new BoneSettings
        {
            boneName = "上半身",
            motionMultiplier = new Vector3(0.5f, 0.5f, 0.5f),
            motionLimit = new Vector3(2f, 6f, 3f)
        });
    }
}
