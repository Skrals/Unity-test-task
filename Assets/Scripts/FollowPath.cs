using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public PathGeneration path;
    public float speed = 1;
    public float maxDistance = .1f;
    public float pathTimerForward;
    public float pathTimerBack;
    [SerializeField]private bool timerSwitch;

    private IEnumerator<Transform> pointInPath;
    void Start()
    {
        timerSwitch = false;
        if (path == null)
        {
            Debug.Log("Отсутствует путь");
            return;
        }
        pointInPath = path.GetNextPathPoint();
        pointInPath.MoveNext();// следующая точка в пути

        if (pointInPath.Current == null)
        {
            Debug.Log("Нет точек");
            return;
        }
        transform.position = pointInPath.Current.position;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
                pointInPath = path.GetNextPathPoint();
                pointInPath.MoveNext();// следующая точка в пути
                transform.position = pointInPath.Current.position;
        }

        if (pointInPath == null || pointInPath.Current == null)// проверка отсутсвия пути
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, pointInPath.Current.position, Time.deltaTime * speed);
        var distance = (transform.position - pointInPath.Current.position).sqrMagnitude;
        if (distance < maxDistance * maxDistance)
        {
            pointInPath.MoveNext();
        }
        Timers();

        
    }
    private void Timers()
    {
        if (path.movementDirection> 0)
        {
            if (!timerSwitch)
            {
                pathTimerBack = 0;
                timerSwitch = true;
            }
            pathTimerForward += Time.deltaTime;
        }
        else
        {
            if (timerSwitch)
            {
                pathTimerForward = 0;
                timerSwitch = false;
            }
            pathTimerBack += Time.deltaTime;
        }
    }
}
