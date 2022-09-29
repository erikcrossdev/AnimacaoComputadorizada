using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StateMachine
{
    WANDER_FOR_FOOD,
    FIND_FOOD,
    RUN_AWAY,
    KEEP_RUNNING,
}

[CreateAssetMenu(menuName = "AIBehaviours/PlayerbotErik")]
public class PlayerbotErik : AIBehaviour
{

    [SerializeField] private StateMachine _state;
    [SerializeField] private float _perceptionRange = 10f;
    private List<Transform> _snakePartsInRange = new List<Transform>();
    private Transform _target = null;
    private float _timeInState = 0.0f;
    private Vector3 _keepRunningDirection;

    int _collidersInRangeCount;
    Collider2D[] _collidersInRange = new Collider2D[256];

    public const string ORB_TAG = "Orb";
    public const string BOT_TAG = "Bot";
    public const string BODY_TAG = "Body";

    public override void Init(GameObject own, SnakeMovement ownMove)
    {
        base.Init(own, ownMove);
        GoToState(StateMachine.WANDER_FOR_FOOD);
    }

    public override void Execute()
    {        
        FindOverlapingObjects();
        UpdateBahaviour();
        MoveForward();
    }

    void FindOverlapingObjects()
    {
        _collidersInRangeCount = Physics2D.OverlapCircleNonAlloc(owner.transform.position, _perceptionRange, _collidersInRange);
    }

    void MoveForward()
    {        
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, rotation, ownerMovement.speed * Time.deltaTime);
        owner.transform.position += direction * ownerMovement.speed * Time.deltaTime;
    }

    private Transform FindFoodInRange()
    {
        var closetDistSqr = Mathf.Infinity;
        Transform closestChild = null;
        for(int i = 0; i < _collidersInRangeCount; i++)
        {
            var coll = _collidersInRange[i];
            if (!coll.CompareTag(ORB_TAG)) continue;

            Vector3 offset = coll.transform.position - owner.transform.position;
            float distanceSqr = offset.sqrMagnitude;
            if (distanceSqr < _perceptionRange * _perceptionRange)
            {
                if (distanceSqr < closetDistSqr)
                {
                    closetDistSqr = distanceSqr;
                    closestChild = coll.transform;
                }
            }
        }

        return closestChild;

    }
  
    private void FindOtherBotsAndTheirBodies()
    {
        _snakePartsInRange.Clear();
        for (int i = 0; i < _collidersInRangeCount; i++)
        {
            var coll = _collidersInRange[i];
            if (!coll.CompareTag(BOT_TAG) && !coll.CompareTag(BODY_TAG)) continue;
            if (coll.transform.root.GetComponentInChildren<SnakeMovement>() == ownerMovement) continue;

            _snakePartsInRange.Add(coll.transform);
        }

    }


    private void UpdateBahaviour()
    {
        _timeInState += Time.deltaTime;

        FindOtherBotsAndTheirBodies();

        if (_snakePartsInRange.Count > 0)
        {
            GoToState(StateMachine.RUN_AWAY);
        }
        switch (_state)
        {
            case StateMachine.WANDER_FOR_FOOD:
                WanderForFood();
                break;
            case StateMachine.FIND_FOOD:
                FindFood();
                break;
           
            case StateMachine.RUN_AWAY:
                RunFromBots();
                break;
            case StateMachine.KEEP_RUNNING:
                direction = _keepRunningDirection;
                if(_timeInState > 2f)
                {
                    GoToState(StateMachine.WANDER_FOR_FOOD);
                }
                break;
        }
    }

    private void GoToState(StateMachine newState)
    {
        _state = newState;
        _timeInState = 0;

        if(newState == StateMachine.WANDER_FOR_FOOD)
        {
            direction = Random.onUnitSphere;
            direction.z = 0;
            direction.Normalize();

            _timeInState = 0;
        }
    }

    private void RunFromBots()
    {
        if (_snakePartsInRange.Count == 0)
        {
            GoToState(StateMachine.KEEP_RUNNING);

            return;
        }

        Vector3 moveDirection = Vector3.zero;
        foreach(var part in _snakePartsInRange)
        {
            var directionEscapeFromTarget = owner.transform.position - part.position;
            direction += directionEscapeFromTarget;
        }
        direction.z = 0;
        direction.Normalize();

        _keepRunningDirection = direction;
    }

    private void WanderForFood()
    {
        _target = FindFoodInRange();
        if (_target != null)
        {
            GoToState(StateMachine.FIND_FOOD);
            return;
        }

        if (_timeInState >= timeChangeDir)
        {
            GoToState(StateMachine.WANDER_FOR_FOOD);
        }

    }

    private void FindFood()
    {
        _target = FindFoodInRange();

        if (_target == null)
        {
            GoToState(StateMachine.WANDER_FOR_FOOD);
        }
        else
        {
            direction = _target.position - owner.transform.position;
            direction.z = 0.0f;
            direction.Normalize();
        }
    }

}
