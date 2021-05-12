using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PathGeneration : MonoBehaviour
{
    public enum PathTypes
    {
        linear,
        loop
    }
    public GameObject savingData;
    public PathTypes pathType;
    public int movementDirection = 1;
    public int moveTo = 0;
    public Transform[] pathElements;
    [Header ("Объект точки")]
    public GameObject generationObject;
    [Header("Минимум и максимум координат рандомизации")]
    public int minRandomRange;
    public int maxRandomRange;
    private float randomPositionX;
    private float randomPositionY;
    private float randomPositionZ;
    [SerializeField] bool load = false;
    [SerializeField] private bool genReady;
    [SerializeField] private int currentElementsNumber;
    [SerializeField] private InputField quantityField;
    private void Start()
    {
        currentElementsNumber = pathElements.Length;
        genReady = false;
        if(!load)
        {
            Load();
        }
    }
    private void Update()
    {
        if (currentElementsNumber != pathElements.Length)
        {
            genReady = false;
            Delete();
        }
        if (genReady == false && pathElements.Length >= 2)
        {
            genReady = true;
            Generation();
        }
    }
    public void PathT(Toggle toggle)
    {
        if(!toggle.isOn)
        {
            pathType = PathTypes.linear;
        }
        else
        {
            pathType = PathTypes.loop;
        }
    }
    public void Load()
    {
        genReady = true;
        load = true;
        savingData.GetComponent<Save>().LoadArray();
        pathElements = new Transform[savingData.GetComponent<Save>().tmp.Length];
        currentElementsNumber = pathElements.Length;
        pathElements = savingData.GetComponent<Save>().tmp;
    }

    public void ElementsSize(int size)
    {
            size = Convert.ToInt32(quantityField.text);
            pathElements = new Transform[size];
            Start();
            Delete();
            Update();
    }
    private void Generation()
    {
        for (int i = 0; i <= pathElements.Length - 1; i++)
        {
            PointRandomization();
            Vector3 newPosition = new Vector3(randomPositionX, randomPositionY, randomPositionZ);
            this.generationObject.transform.localPosition = newPosition;
            var element = Instantiate(generationObject, generationObject.transform.position, generationObject.transform.rotation);
            pathElements[i] = element.transform;
            pathElements[i].SetParent(gameObject.transform);
        }
        currentElementsNumber = pathElements.Length;
    }
    private void PointRandomization()
    {
        randomPositionX = UnityEngine.Random.Range(minRandomRange, maxRandomRange);
        randomPositionY = UnityEngine.Random.Range(minRandomRange, maxRandomRange);
        randomPositionZ = UnityEngine.Random.Range(minRandomRange, maxRandomRange);
    }
    private void Delete()
    {
        GameObject[] objectsForDelete = GameObject.FindGameObjectsWithTag("Point");
        foreach (GameObject g in objectsForDelete)
        {
            Destroy(g);
        }
    }
    public void OnDrawGizmos()
    {
        if (pathElements == null || pathElements.Length < 2)
        {
            return;
        }
        for (var i = 1; i < pathElements.Length; i++)
        {
            Gizmos.DrawLine(pathElements[i - 1].position, pathElements[i].position);
        }
        if (pathType == PathTypes.loop)
        {
            Gizmos.DrawLine(pathElements[0].position, pathElements[pathElements.Length - 1].position);
        }
    }
    public IEnumerator<Transform> GetNextPathPoint()
    {
        if (pathElements == null || pathElements.Length < 1)
        {
            yield break;
        }
        if( moveTo > pathElements.Length)
        {
            moveTo = 0;
        }
        while (true)
        {
            yield return pathElements[moveTo];
            if (pathElements.Length == 1)
            {
                continue;
            }
            if (pathType == PathTypes.linear)
            {
                if (moveTo <= 0)
                {
                    movementDirection = 1;
                }
                else if (moveTo >= pathElements.Length - 1)
                {
                    movementDirection = -1;
                }
            }
            moveTo = moveTo + movementDirection;
            if (pathType == PathTypes.loop)
            {
                if (moveTo >= pathElements.Length)
                {
                    moveTo = 0;
                }
                if (moveTo < 0)
                {
                    moveTo = pathElements.Length - 1;
                }
            }
        }
    }
}
