using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateWithCurve : MonoBehaviour
{
    public AnimationCurve curve;
    private Transform startPos;
    public Transform endPos;

    public float speed = 1.0f;

    private float startTime;

    private float journeyLength;
    
    void Start()
    {
        startPos = transform;
        startTime = Time.time;

        journeyLength = Vector3.Distance(startPos.position, endPos.position);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distanceCovered / journeyLength;

        transform.position = Vector3.Lerp(startPos.position, endPos.position, curve.Evaluate(fractionOfJourney));
    }
}
