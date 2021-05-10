using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Save : MonoBehaviour
{
    public GameObject pathArray;
    public SaveData sv = new SaveData();
    private string filePath;
    private void Start()
    {
        filePath = Path.Combine(Application.dataPath, "Save.json");
    }
    public void LoadArray()
    {
        if (File.Exists(filePath))
        {
            sv = JsonUtility.FromJson<SaveData>(File.ReadAllText(filePath));
            Debug.Log(sv.savingArray);
        }
    }
    public void SaveArray()
    {
        sv.savingArray = pathArray.GetComponent<PathGeneration>().pathElements;
        File.WriteAllText(filePath, JsonUtility.ToJson(sv));
    }
}
[Serializable]
public class SaveData
{
    public Transform[] savingArray;
}
