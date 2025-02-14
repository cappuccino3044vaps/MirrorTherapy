using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace MirrorTherapy.Settings{
public class SaveTimeToJson : MonoBehaviour
{
    public enum TimeType
    {
        Start,
        End,
    }
    // ローカルディレクトリの保存先
    private string filePath;

    void Start()
    {
        // 保存先ディレクトリを指定（プロジェクトの実行ディレクトリ内の "SavedData" フォルダ）
        string directoryPath = Path.Combine(Application.persistentDataPath, "TimeData");

        // ディレクトリが存在しない場合は作成
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // ファイルパスを設定
        filePath = Path.Combine(directoryPath, "SavedTime.json");
    }
    // ボタンが押されたときに呼び出される関数
    public void SaveCurrentTime(TimeType type)
    {
        // 現在時刻を取得
        string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // 現在の記録を読み込む
        List<TimeRecord> records = LoadRecords();
        // 新しい時刻をリストに追加
        records.Add(new TimeRecord {type=type.ToString(),time = currentTime });
        // JSON形式に変換して保存
        string json = JsonUtility.ToJson(new TimeRecordList { records = records }, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"時刻を保存しました: {filePath}");
    }
    private List<TimeRecord> LoadRecords()
    {
        // ファイルが存在する場合は読み込む
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            TimeRecordList recordList = JsonUtility.FromJson<TimeRecordList>(json);
            return recordList?.records ?? new List<TimeRecord>();
        }

        // ファイルが存在しない場合は空のリストを返す
        return new List<TimeRecord>();
    }
    // 時刻データを格納するクラス
    [Serializable]
    private class TimeRecord
    {
        public string type;
        public string time;
    }
    [Serializable]
    private class TimeRecordList
    {
        public List<TimeRecord> records;
    }
}
}
