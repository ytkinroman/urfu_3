using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class DataLoader : MonoBehaviour
{
    private const string URL = "https://sheets.googleapis.com/v4/spreadsheets/XXXXXXXXXX";
    public static Dictionary<int, DataEntry> dataSet = new Dictionary<int, DataEntry>();
    public static event Action OnDataLoaded;

    private void Start () {
        StartCoroutine(GoogleSheets());
    }

    private IEnumerator GoogleSheets () {
        UnityWebRequest currentResp = UnityWebRequest.Get(URL);
        yield return currentResp.SendWebRequest();

        if (currentResp.result != UnityWebRequest.Result.Success) {
            Debug.LogError("Failed to load data: " + currentResp.error);
            yield break;
        }

        string rawResp = currentResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);

        int headerRowIndex = 14; // Индекс 15-й строки.
        int currentRowIndex = 0;

        foreach (var itemRawJson in rawJson["values"]) {
            if (currentRowIndex <= headerRowIndex) {
                currentRowIndex++;
                continue; // Пропускаем строки до заголовка
            }

            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList;

            int level = int.Parse(selectRow[0]);
            dataSet[level] = new DataEntry(
                float.Parse(selectRow[2]),
                float.Parse(selectRow[3]),
                float.Parse(selectRow[4]),
                float.Parse(selectRow[5]),
                float.Parse(selectRow[9])
            );
        }

        // Debug.Log(dataSet[10].Speed);
        OnDataLoaded?.Invoke();
    }

    public class DataEntry
    {
        public float Speed { get; }
        public float TimeBetweenEggDrops { get; }
        public float LeftRightDistance { get; }
        public float ChanceDirection { get; }

        public float EggsNumber { get; }

        public DataEntry (float speed, float timeBetweenEggDrops, float leftRightDistance, float chanceDirection, float eggsNumber) {
            Speed = speed;
            TimeBetweenEggDrops = timeBetweenEggDrops;
            LeftRightDistance = leftRightDistance;
            ChanceDirection = chanceDirection;
            EggsNumber = eggsNumber;
        }
    }
}
