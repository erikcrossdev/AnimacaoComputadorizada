using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    public AnimationCurve curve;
    public float Distance = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = ray.GetPoint(Distance);
        transform.position = Vector3.Lerp(transform.position, pos, curve.Evaluate(0.01f)); 
    }
}
