using UnityEngine;

/// <summary>
/// マウス入力を処理し、正規化された値を提供するクラス
/// </summary>
public class MouseInputHandler
{
    /// <summary>
    /// 現在のマウス位置を正規化された座標で取得
    /// </summary>
    /// <returns>正規化されたマウス座標(-1 to 1)</returns>
    public static Vector2 GetNormalizedMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        
        // スクリーンサイズで正規化
        float normalizedX = (mousePosition.x / Screen.width) * 2f - 1f;
        float normalizedY = (mousePosition.y / Screen.height) * 2f - 1f;
        
        return new Vector2(normalizedX, normalizedY);
    }
    
    /// <summary>
    /// マウスの移動量を取得（オプション機能として将来使用可能）
    /// </summary>
    /// <returns>前フレームからのマウス移動量</returns>
    public static Vector2 GetMouseDelta()
    {
        // 将来的にマウスの移動量ベースの制御を追加する場合に使用
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}
