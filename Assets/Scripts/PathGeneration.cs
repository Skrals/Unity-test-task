using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGeneration : MonoBehaviour
{
    public enum PathTypes
    {
        linear,
        loop
    }
    public PathTypes pathType;
    public int movementDirection = 1;
    public int moveTo = 0;
    public Transform[] pathElements;
    public GameObject generationObject;

    [SerializeField] private int minRandomRange;
    [SerializeField] private int maxRandomRange;
    private float randomPositionX;
    private float randomPositionY;
    private float randomPositionZ;
    private bool genReady;
    private int currentElementsNumber;
    private void Start()
    {
        currentElementsNumber = pathElements.Length;
        genReady = false;
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
    private void Generation()
    {
        for (int i = 0; i <= pathElements.Length - 1; i++)
        {
            PointRandomization();
            Vector3 newPosition = new Vector3(randomPositionX, randomPositionY, randomPositionZ);
            this.generationObject.transform.localPosition = newPosition;
            var element = Instantiate(generationObject, generationObject.transform.position, generationObject.transform.rotation);
            pathElements[i] = element.transform;
        }
        currentElementsNumber = pathElements.Length;
    }
    private void PointRandomization()
    {
        randomPositionX = Random.Range(minRandomRange, maxRandomRange);
        randomPositionY = Random.Range(minRandomRange, maxRandomRange);
        randomPositionZ = Random.Range(minRandomRange, maxRandomRange);
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
