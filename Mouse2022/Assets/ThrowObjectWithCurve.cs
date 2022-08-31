using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectWithCurve : MonoBehaviour
{
    public AnimationCurve curve;
    Vector3 startPosition;
    public Transform endPosition;
    public float timeToMove;
    public bool pingPong;
    void Start() {
        startPosition = transform.position;
        Move(); 
    }
    public void Move()
    {
        StartCoroutine(LerpPos(startPosition, endPosition.position, timeToMove));
    }
    public void MoveFinished()
    {
        if (pingPong)
        {
            Vector3 temp = startPosition;
            startPosition = endPosition.position;
            endPosition.position = temp;
            Move();
        }
    }
    IEnumerator LerpPos(Vector3 start, Vector3 end, float timeToMove)
    {
        float t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(start, end, curve.Evaluate(t));
            t = t + Time.deltaTime / timeToMove;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
        MoveFinished();
    }
}