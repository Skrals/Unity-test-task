using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public TransformCam cam = null;
    public Transform target = null;
    public static Vector3 offset;

    Ray ray;
    RaycastHit hit;
    public float limit = 80; // ограничение вращения по Y
    public float zoom = 1; // чувствительность при увеличении, колесиком мышки
    public float zoomMax = 10; // макс. увеличение
    public float zoomMin = 3; // мин. увеличение
    public float MouseSense = 3; // чувствительность мышки
    private float X, Y;

    void Start()
    {
        cam = gameObject.AddComponent<TransformCam>();
        limit = Mathf.Abs(limit);
        if (limit > 90) limit = 90;
        offset = new Vector3(offset.x, offset.y, -Mathf.Abs(zoomMax) / 2);
        transform.position = target.position + offset;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        SwitchCameraViewTarget();

        if (Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Mouse0))
        {
            MouseInputR();
            cam.Transform(target, offset);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && offset.z < -zoomMin)// приблизить
        {
            offset.z += zoom;
            cam.MouseZoom(offset, zoomMax, zoomMin);
            cam.Transform(target, offset);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && offset.z > -zoomMax)// отдалить
        {
            offset.z -= zoom;
            cam.MouseZoom(offset, zoomMax, zoomMin);
            cam.Transform(target, offset);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (target == null)
        {
            try { target = GameObject.FindGameObjectWithTag("Player").transform; }
            catch
            {
                target = GameObject.Find("PathGenerator").transform;
            }

        }
        cam.Transform(target, offset);
    }
    void MouseInputR()
    {
        X = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * MouseSense;
        Y += Input.GetAxis("Mouse Y") * MouseSense;
        Y = Mathf.Clamp(Y, -limit, limit);
        transform.localEulerAngles = new Vector3(-Y, X, 0);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void SwitchCameraViewTarget()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                target = hit.collider.gameObject.GetComponent<Transform>();
            }
            cam.Transform(target, offset);
        }
    }
}
