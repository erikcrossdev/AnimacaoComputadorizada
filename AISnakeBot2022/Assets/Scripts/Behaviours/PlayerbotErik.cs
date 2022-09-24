using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateMachine
{
    WANDER_FOR_FOOD,
    FIND_FOOD,
    SURROUND,
    RUN_AWAY,
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
    private Transform target = null;
    private float _timer = 0.0f;
    public override void Init(GameObject own, SnakeMovement ownMove)
    {
        base.Init(own, ownMove);
        OrbsParent = GameObject.Find("Orbs");
        GameLogicInstance = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        _state = StateMachine.WANDER_FOR_FOOD;
        _timer = timeChangeDir;
    }

    //seria interessante ter um controlador com o colisor que define o mundo pra poder gerar pontos dentro desse colisor

    public override void Execute()
    {
        Debug.Log("Update Perception!!!!!");
        UpdateBahaviour();
        MoveForward();
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
        var oldDist = Mathf.Infinity;
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
                    Debug.Log($"Found a closer post: old pos was:{oldDist}, New pos is: {distanceSqr} New Child ID: {i}, BotName{owner.gameObject.name}");
                    oldDist = distanceSqr;
                    closestChild = child;
                }
            }
        }
        if (closestChild != null) Debug.Log($"ClosestChild vale is:::::  {closestChild.name}, BotName{owner.gameObject.name}");
        return closestChild;
    }
    private int FindOtherBotsAndTheirBodies()
    {
        var oldDist = 0.0f;
        int bots = 0;
        for (int i = 0; i < GameLogicInstance.snakes.Count; i++)
        {
            var snakeBody = GameLogicInstance.snakes[i].GetComponents<SnakeBody>();
            var snakeHead = GameLogicInstance.snakes[i].GetComponent<SnakeMovement>();
            if (snakeHead != null)
            {
                Vector3 offset = snakeHead.transform.position - owner.transform.position;
                float distanceSqr = offset.sqrMagnitude;
                Debug.Log($"BOT id {i}, distance is: {distanceSqr} , BotName{owner.gameObject.name}");
                if (distanceSqr < _perceptionRange * _perceptionRange)
                {
                    bots++;
                    Debug.Log($"Found one more bot: {bots}, BotName{owner.gameObject.name}");
                }
            }
            for (int j = 0; j < snakeBody.Length; j++)
            {
                Vector3 offset = snakeBody[i].transform.position - owner.transform.position;
                float distanceSqr = offset.sqrMagnitude;
                Debug.Log($"Child id {i}, distance is: {distanceSqr} , BotName{owner.gameObject.name}");
                if (distanceSqr < _perceptionRange * _perceptionRange)
                {
                    bots++;
                    Debug.Log($"Found one more bot: {bots}, BotName{owner.gameObject.name}");
                }
            }
        }
        return bots;
    }


    private void UpdateBahaviour()
    {
        var nearbyBots = FindOtherBotsAndTheirBodies();
        /*
        Debug.Log($"Bots in range {nearbyBots}, BotName{owner.gameObject.name}");
        if (nearbyBots == 0)
        {
            _state = StateMachine.FIND_FOOD;
        }
        else
        {
            _state = StateMachine.RUN_AWAY;
            Debug.Log($"CORRE MANO!!! BotName{owner.gameObject.name}");
        }*/
        switch (_state)
        {
            case StateMachine.WANDER_FOR_FOOD:
                WanderForFood();
                break;
            case StateMachine.FIND_FOOD:
                FindFoodIfItIsSafe();
                break;
            case StateMachine.SURROUND:
                break;
            case StateMachine.RUN_AWAY:
                break;
            case StateMachine.DASH_AND_SCAPE:
                break;
        }
        Debug.Log($"Current State: {_state}");
    }

    private void WanderForFood()
    {
       
        target = FindFoodInRange();
        if (target != null)
        {
            _state = StateMachine.FIND_FOOD;
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= timeChangeDir)
        {
            direction = Random.onUnitSphere;
            direction.z = 0;
            direction.Normalize();
            /*randomPoint = new Vector3(
                   Random.Range(
                       Random.Range(owner.transform.position.x - 10, owner.transform.position.x - 5),
                       Random.Range(owner.transform.position.x + 5, owner.transform.position.x + 10)
                   ),
                   Random.Range(
                       Random.Range(owner.transform.position.y - 10, owner.transform.position.y - 5),
                       Random.Range(owner.transform.position.y + 5, owner.transform.position.y + 10)
                   ),
                   0
               );
            direction = randomPoint - owner.transform.position;
            direction.z = 0.0f;*/
            _timer = 0;
        }

    }

    private void FindFoodIfItIsSafe()
    {
        //if (target == null)
        {
            //Debug.Log($"Direction is nulll  finding target!!!!!!!!!!!!!!!!!!!!, BotName{owner.gameObject.name}");
            target = FindFoodInRange();

            if (target == null)
            {
                Debug.Log($"WANDER_FOR_FOOD, BotName{owner.gameObject.name}");
                _state = StateMachine.WANDER_FOR_FOOD;
                _timer = 0;
            }
            else
            {
                direction = target.position - owner.transform.position;
                direction.z = 0.0f;
                direction.Normalize();
            }
        }

    }


}
