
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentSteering
{
    Seek,
    Flee,
    FleeAndArrival,
    Wander,
}

public class SeekBehaviour : MonoBehaviour
{
    public CurrentSteering _CurrentBehaviour;
    public Transform Target;
    private Vector3 _position;
    private Vector3 _velocity;
    [Header("General Settings")]
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _mass;
    [Header("Flee and Arrival Settings")]
    [SerializeField] private float _slowingRadius;
    [Header("Wander Settings")]
    [SerializeField] private float _circleRadius;
    [SerializeField] private float _wanderAngle;
    [SerializeField] private float _angleChange;
    [SerializeField] private float _circleDist;
    [SerializeField] private Vector2 _randomNum;

    private Vector3 _desiredVelocity;
    private Vector3 _steering;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (_CurrentBehaviour)
        {
            case CurrentSteering.Seek:
                Seek();
                break;
            case CurrentSteering.Flee:
                Flee();
                break;
            case CurrentSteering.FleeAndArrival:
                FleeAndArrival();
                break;
            case CurrentSteering.Wander:
                Wander();
                break;

        }
    }

    private void Seek()
    {
        float dist = Vector3.Distance(Target.position, transform.position);
        if (dist > 0.5f)
        {
            _desiredVelocity = Vector3.Normalize(Target.transform.position - transform.position) * _maxVelocity;
            _steering = _desiredVelocity - _velocity;

            _steering = Vector3.ClampMagnitude(_steering, _maxForce);
            _steering = _steering / _mass;

            _velocity = Vector3.ClampMagnitude(_velocity + _steering, _maxSpeed);
            transform.position = transform.position + _velocity * Time.deltaTime;

            var angle = (Mathf.Rad2Deg * Mathf.Atan2(_velocity.y, _velocity.x)) - 90;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void UpdateSpriteRotation()
    {
        var angle = (Mathf.Rad2Deg * Mathf.Atan2(_velocity.y, _velocity.x)) - 90;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void UpdatePosition()
    {

        _velocity = Truncate(_velocity + _steering, (int)_maxSpeed);
        transform.position = transform.position + _velocity * Time.deltaTime;
        UpdateSpriteRotation();
    }
    private void Flee()
    {
        float dist = Vector3.Distance(Target.position, transform.position);
        if (dist < 10.5f)
        {

            _desiredVelocity = Vector3.Normalize(transform.position - Target.transform.position) * _maxVelocity;
            _steering = _desiredVelocity - _velocity;

            _steering = Vector3.ClampMagnitude(_steering, _maxForce);
            _steering = _steering / _mass;

            UpdatePosition();

        }
    }

    private void FleeAndArrival()
    {
        _desiredVelocity = Target.position - transform.position;
        float distance = _desiredVelocity.magnitude;

        if (distance < _slowingRadius)
        {
            _desiredVelocity = Vector3.Normalize(transform.position - Target.transform.position) * _maxVelocity * (distance / _slowingRadius);
        }
        else
        {
            _desiredVelocity = Vector3.Normalize(_desiredVelocity) * _maxVelocity;
        }

        _steering = _desiredVelocity - _velocity;

        _steering = Vector3.ClampMagnitude(_steering, _maxForce);
        _steering = _steering / _mass;

        UpdatePosition();

    }

    private void Wander()
    {
        _steering = CalculateWanderForce();
        _steering = Vector3.ClampMagnitude(_steering, _maxForce);
        _steering = _steering / _mass;
        _velocity = Vector3.ClampMagnitude(_velocity + _steering, _maxSpeed);
        transform.position = transform.position + _velocity * Time.deltaTime;

        UpdateSpriteRotation();

    }

    private Vector3 CalculateWanderForce()
    {
        var circleCenter = _velocity;
        circleCenter = Vector3.Normalize(circleCenter);
        circleCenter = new Vector3(circleCenter.x * _circleDist, circleCenter.y * _circleDist, circleCenter.z * _circleDist);

        var displacement = new Vector3(0, -1);
        displacement = new Vector3(displacement.x * _circleRadius, displacement.y * _circleRadius, displacement.z * _circleRadius);

        displacement = SetAngle(displacement, _wanderAngle);

        _wanderAngle = ((Random.Range(_randomNum.x, _randomNum.y) * _angleChange) - (_angleChange * 0.5f));

        var _wanderForce = circleCenter + displacement;
        return _wanderForce;
    }

    private Vector3 Truncate(Vector3 vec, int max)
    {
        float number;
        number = max / vec.magnitude;
        number = (float)(number < 1.0 ? number : 1.0);
        vec = new Vector3(vec.x * number, vec.y * number, vec.z * number);
        return vec;
    }


    private Vector3 SetAngle(Vector3 vector, float value)
    {
        var length = vector.magnitude;
        vector.x = Mathf.Cos(value) * length;
        vector.y = Mathf.Sin(value) * length;
        return vector;
    }
}
