using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StateMachine
{
    WANDER_FOR_FOOD,
    FIND_FOOD,
    SURROUND,
    RUN_AWAY,
    KEEP_RUNNING,
    DASH_AND_SCAPE,
}

[CreateAssetMenu(menuName = "AIBehaviours/PlayerbotErik")]
public class PlayerbotErik : AIBehaviour
{

    [SerializeField] private StateMachine _state;
    [SerializeField] private float _perceptionRange = 10f;
    [SerializeField] private int _nearbyBots;
    [SerializeField] public GameObject OrbsParent;
    [SerializeField] public GameLogic GameLogicInstance;
    private List<Transform> _snakePartsInRange = new List<Transform>();
    private List<SnakeMovement> _snakes = new List<SnakeMovement>();
    private Transform _target = null;
    private float _timeInState = 0.0f;
    private Vector3 _keepRunningDirection;

    // body finder yay
    List<SnakeBody> _snakeBody = new List<SnakeBody>();

    // Detect orb stuff very nice
    int _collidersInRangeCount;
    Collider2D[] _collidersInRange = new Collider2D[256];

    public override void Init(GameObject own, SnakeMovement ownMove)
    {
        base.Init(own, ownMove);
        OrbsParent = GameObject.Find("Orbs");
        GameLogicInstance = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        GoToState(StateMachine.WANDER_FOR_FOOD);
    }

    //seria interessante ter um controlador com o colisor que define o mundo pra poder gerar pontos dentro desse colisor

    public override void Execute()
    {
        //Debug.Log("Update Perception!!!!!");
        /*if(_snakes.Count<=0)*/ 
        
        FindStuff();
        UpdateBahaviour();
        MoveForward();
    }

    void FindStuff()
    {
        _collidersInRangeCount = Physics2D.OverlapCircleNonAlloc(owner.transform.position, _perceptionRange, _collidersInRange);
    }

    //ia basica, move, muda de direcao e move
    void MoveForward()
    {
        
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, rotation, ownerMovement.speed * Time.deltaTime);
        owner.transform.position += direction * ownerMovement.speed * Time.deltaTime;

        /*float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, rotation, ownerMovement.speed * Time.deltaTime);

        owner.transform.position = Vector2.MoveTowards(owner.transform.position, randomPoint, ownerMovement.speed * Time.deltaTime);*/
    }



    private Transform FindFoodInRange()
    {
        var closetDistSqr = Mathf.Infinity;
        Transform closestChild = null;
        for(int i = 0; i < _collidersInRangeCount; i++)
        {
            var coll = _collidersInRange[i];
            if (!coll.CompareTag("Orb")) continue;

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

        /*var oldDist = Mathf.Infinity;
        Transform closestChild = null;
        for (int i = 0; i < OrbsParent.transform.childCount; i++)
        {
            var child = OrbsParent.transform.GetChild(i);
            Vector3 offset = child.transform.position - owner.transform.position;
            float distanceSqr = offset.sqrMagnitude;
            //Debug.Log($"Child id {i}, distance is: {distanceSqr} , BotName{owner.gameObject.name}", owner);
            if (distanceSqr < _perceptionRange * _perceptionRange)
            {
                // Debug.Log($"distanceSqr  is:::::  {distanceSqr}, BotName{owner.gameObject.name}");
                if (distanceSqr < oldDist)
                {
                    //Debug.Log($"Found a closer post: old pos was:{oldDist}, New pos is: {distanceSqr} New Child ID: {i}, BotName{owner.gameObject.name}");
                    oldDist = distanceSqr;
                    closestChild = child;
                }
            }
        }
        //if (closestChild != null) Debug.Log($"ClosestChild vale is:::::  {closestChild.name}, BotName{owner.gameObject.name}");
        return closestChild;*/
    }
    private void FindSnakes() { 
        _snakes.Clear();
        for (int i = 0; i < GameLogicInstance.snakes.Count; i++)
        {
            //var snakeBody = GameLogicInstance.snakes[i].GetComponentsInChildren<SnakeBody>();
            var snakeHead = GameLogicInstance.snakes[i].GetComponentInChildren<SnakeMovement>();
            if (snakeHead != null) {
                _snakes.Add(snakeHead);
            }
        }
     }
    private void FindOtherBotsAndTheirBodies()
    {
        _snakePartsInRange.Clear();
        for (int i = 0; i < _collidersInRangeCount; i++)
        {
            var coll = _collidersInRange[i];
            if (!coll.CompareTag("Bot") && !coll.CompareTag("Body")) continue;
            if (coll.transform.root.GetComponentInChildren<SnakeMovement>() == ownerMovement) continue;

            _snakePartsInRange.Add(coll.transform);
        }

        /*var parts = new List<Transform>();

        /*int bots = 0;
        _snakePartsInRange.Clear();

        //Debug.Log($"Snakes instantiated {GameLogicInstance.snakes.Count}");
        /*for (int i = 0; i < _snakes.Count; i++)
        {
            var snakeHead = _snakes[i];
            if (snakeHead == ownerMovement) continue;

            // Debug.Log($"Snakes Body at i = {i} , amount: {snakeBody.Length}");
            // Debug.Log($"Snakes Body at i = {i} , head: {snakeHead}", snakeHead);

            parts.Add(snakeHead.transform);

            _snakeBody.Clear();
            snakeHead.transform.parent.GetComponentsInChildren<SnakeBody>(_snakeBody);
            parts.AddRange(_snakeBody.Select(body => body.transform));

            foreach(var part in parts)
            {
                Vector3 offset = part.position - owner.transform.position;
                float distanceSqr = offset.sqrMagnitude;
                // Debug.Log($"BOT id {i}, distance is: {distanceSqr} , BotName{owner.gameObject.name}");
                if (distanceSqr < _perceptionRange * _perceptionRange)
                {
                    bots++;

                    // Debug.Log($"Found one more bot: {bots}, BotName{owner.gameObject.name}");
                    _snakePartsInRange.Add(part);
                }
            }
        }
        return bots;*/
    }




    private void UpdateBahaviour()
    {
        _timeInState += Time.deltaTime;

        FindOtherBotsAndTheirBodies();

       // Debug.Log($"Bots in range {nearbyBots}, BotName{owner.gameObject.name}");
        if (_snakePartsInRange.Count > 0)
        {
            // Debug.Log("RUN AWAY!!!!!!");
            GoToState(StateMachine.RUN_AWAY);
        }
        /*else {
            _state = StateMachine.WANDER_FOR_FOOD;
            _timer = timeChangeDir;
        }*/
        switch (_state)
        {
            case StateMachine.WANDER_FOR_FOOD:
                WanderForFood();
                break;
            case StateMachine.FIND_FOOD:
                FindFood();
                break;
            case StateMachine.SURROUND:
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
            case StateMachine.DASH_AND_SCAPE:
                break;
        }
        //Debug.Log($"Current State: {_state}");
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
        //Debug.Log($"Direction is nulll  finding target!!!!!!!!!!!!!!!!!!!!, BotName{owner.gameObject.name}");
        _target = FindFoodInRange();

        if (_target == null)
        {
            Debug.Log($"WANDER_FOR_FOOD, BotName{owner.gameObject.name}");
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
