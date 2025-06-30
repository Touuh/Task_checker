using System;
using UnityEngine;

/// <summary>
/// ボーンの動作設定を管理するクラス
/// </summary>
[Serializable]
public class BoneSettings
{
    [Header("ボーン名")]
    public string boneName;
    
    [Header("動作倍率")]
    public Vector3 motionMultiplier = Vector3.one;
    
    [Header("動作制限（角度）")]
    public Vector3 motionLimit = new Vector3(30f, 30f, 30f);
    
    [Header("初期角度")]
    public Vector3 initialRotation = Vector3.zero;
    
    /// <summary>
    /// マウス入力に基づいてボーンの回転角度を計算
    /// </summary>
    /// <param name="normalizedMouseInput">正規化されたマウス入力(-1 to 1)</param>
    /// <param name="baseBias">基本的な感度バイアス</param>
    /// <returns>制限された回転角度</returns>
    public Vector3 CalculateRotation(Vector2 normalizedMouseInput, float baseBias)
    {
        // 入力に倍率を適用
        Vector3 rawRotation = new Vector3(
            baseBias * motionMultiplier.x * -normalizedMouseInput.y,
            baseBias * motionMultiplier.y * -normalizedMouseInput.x,
            baseBias * motionMultiplier.z * normalizedMouseInput.x
        );
        
        // 制限を適用
        Vector3 limitedRotation = new Vector3(
            ClampRotation(rawRotation.x, motionLimit.x),
            ClampRotation(rawRotation.y, motionLimit.y),
            ClampRotation(rawRotation.z, motionLimit.z)
        );
        
        return limitedRotation;
    }
    
    /// <summary>
    /// 回転角度を制限内に収める
    /// </summary>
    private float ClampRotation(float value, float limit)
    {
        if (limit <= 0) return 0f;
        return Mathf.Abs(value) <= limit ? value : limit * Mathf.Sign(value);
    }
}
