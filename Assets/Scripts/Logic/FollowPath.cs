using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public PathGeneration path;
    public float speed;
    public float maxDistance = .1f;
    public float pathTimerForward;
    public float pathTimerBack;
    [SerializeField]private bool timerSwitch;

    private IEnumerator<Transform> pointInPath;
    void Start()
    {
        timerSwitch = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            try
            {
                pointInPath = path.GetNextPathPoint();
                pointInPath.MoveNext();// следующая точка в пути
                transform.position = pointInPath.Current.position;
            }
            catch
            {
                Debug.Log("Нет пути по которому можно следовать");
            }
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
