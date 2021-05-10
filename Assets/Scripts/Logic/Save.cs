using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Save : MonoBehaviour
{
    public GameObject pathArray;
    public Transform[] tmp;
    public SaveData sv = new SaveData();
    public List<Vector3> vectors = new List<Vector3>();
    private string filePath;
    private void Start()
    {
        filePath = Path.Combine(Application.dataPath, "save.txt");
        tmp = new Transform[sv.transformDataList.Count];
    }
    public void LoadArray()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePath, FileMode.Open);
            sv = (SaveData)bf.Deserialize(fs);
            fs.Close();
            ConvertVectorsToTransform();
        }
    }
    public void ConvertVectorsToTransform()
    {
        float x, y, z;
        for (int i = 0; i <= sv.transformDataList.Count-1;i++ )
        {
            x = sv.transformDataList[i].position.x;
            y = sv.transformDataList[i].position.y;
            z = sv.transformDataList[i].position.z;
            tmp[i].transform.position.Set(x, y, z);
        }
    }
    public void SaveArray()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);
        sv.SaveTransform(pathArray.GetComponent<PathGeneration>().pathElements);
        bf.Serialize(fs, sv);
        fs.Close();
    }
}
[Serializable]
public class SaveData
{
    [Serializable]
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    [Serializable]
    public struct TransfromSaver
    {
        public Vector3 position;
        public TransfromSaver(Vector3 pos)
        {
            position = pos;
        }
    }
    public List<TransfromSaver> transformDataList = new List<TransfromSaver>();
    public void SaveTransform(Transform[] savingArray)
    {
         foreach( var s in savingArray)
        {
            Vector3 pos = new Vector3(s.transform.position.x, s.transform.position.y, s.transform.position.z);
            transformDataList.Add(new TransfromSaver(pos));
        }
    }
}
