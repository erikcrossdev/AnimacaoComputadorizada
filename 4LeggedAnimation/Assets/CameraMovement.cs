using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public List<Transform> _targets = new List<Transform>();
    public Transform target;
    public float distance = 20.0f;
    public float zoomSpd = 2.0f;

    public float xSpeed = 240.0f;
    public float ySpeed = 123.0f;

    public int yMinLimit = -723;
    public int yMaxLimit = 877;

    private float x = 22.0f;
    private float y = 33.0f;

    private int _index;

    public void Start()
    {

        x = 22f;
        y = 33f;

        // Make the rigid body not change rotation
       
    }

    public void LateUpdate()
    {
        if (target)
        {
            x -= Input.GetAxis("Horizontal") * xSpeed * 0.02f;
            y += Input.GetAxis("Vertical") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            distance += Input.GetAxis("Mouse ScrollWheel") * zoomSpd * 0.02f;
            //distance += Input.GetAxis("Mouse ScrollWheel") * zoomSpd * 0.02f;

            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            var currentIndex = _index;
            _index = (currentIndex - 1) % _targets.Count;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var currentIndex = _index;
            _index = (currentIndex + 1) % _targets.Count;
        }
        target = _targets[Mathf.Abs(_index)];
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
}
