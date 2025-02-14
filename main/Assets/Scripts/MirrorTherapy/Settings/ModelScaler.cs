using UnityEngine;
using System.IO;

namespace MirrorTherapy.Settings
{
    /// <summary>
    /// モデルのスケールを変更するクラス
    /// </summary>
public class ModelScaler : MonoBehaviour
{
    [System.Serializable]
    public class ScaleData
    {
        public float decisionRatio;
    }

    public string filename;
    private string filePath;
    public Transform model; // サイズを変更したいモデル

    void Start()
    {
        ApplyScaleFromJson();
    }

    public void ApplyScaleFromJson()
    {
        filePath=Path.Combine(Application.persistentDataPath,"..","CalibrationData",filename);
        // JSONファイルを読み込む
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log($"Loaded JSON: {json}");
            ScaleData scaleData = JsonUtility.FromJson<ScaleData>(json);
            Debug.Log($"Scale data loaded: {scaleData.decisionRatio}");
            if (scaleData != null && model != null)
            {
                // 現在のスケールを取得
                Vector3 currentScale = model.localScale;

                // 比率を保ちながらスケールを変更
                Vector3 newScale = currentScale * scaleData.decisionRatio;

                // モデルに新しいスケールを適用
                model.localScale = newScale;
                Debug.Log($"Model scaled to: {model.localScale}");
            }
            else
            {
                Debug.LogError("Scale data or model is null.");
            }
        }
        else
        {
            Debug.LogError($"File not found at path: {filePath}");
        }
    }
}

}