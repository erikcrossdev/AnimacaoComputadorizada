using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;
using System.Collections;

public class EggRacer : Agent
{
    [Header("Specific to Ball3D")]
    public GameObject ball;
    [Tooltip("Whether to use vector observation. This option should be checked " +
        "in 3DBall scene, and unchecked in Visual3DBall scene. ")]
    public bool useVecObs;
    EnvironmentParameters m_ResetParams;

    [HideInInspector]
    public GoalRace goalDetect;

    public GameObject agent;
    public Transform resetPos;

    public GameObject goal;

    public float _oldDist;

    Rigidbody _ballRigidbody;  //cached on initialization
    Rigidbody _agentRigidbody;  //cached on initialization
    Material _goalMaterial; //cached on Awake()
    Renderer _goalRenderer;

    EggRaceSettings _eggRaceSettings;

    void Awake()
    {
        _eggRaceSettings = FindObjectOfType<EggRaceSettings>();
    }
    public override void Initialize()
    {

        goalDetect = agent.GetComponent<GoalRace>();
        goalDetect.agent = this;

        _agentRigidbody = GetComponent<Rigidbody>();
        // Cache the block rigidbody
        
        _goalRenderer = goal.GetComponent<Renderer>();
        // Starting material
        _goalMaterial = _goalRenderer.material;

        _ballRigidbody = ball.GetComponent<Rigidbody>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVecObs)
        {
            sensor.AddObservation(gameObject.transform.rotation.z);
            sensor.AddObservation(gameObject.transform.rotation.x);
            sensor.AddObservation(ball.transform.position - gameObject.transform.position);
            sensor.AddObservation(_ballRigidbody.velocity);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        
        var actionZ = 2f * Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
        var actionX = 2f * Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);

        MoveAgent(actionBuffers.DiscreteActions);

        RewardByDistance();

        if ((gameObject.transform.rotation.z < 0.25f && actionZ > 0f) ||
            (gameObject.transform.rotation.z > -0.25f && actionZ < 0f))
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 1), actionZ);
        }

        if ((gameObject.transform.rotation.x < 0.25f && actionX > 0f) ||
            (gameObject.transform.rotation.x > -0.25f && actionX < 0f))
        {
            gameObject.transform.Rotate(new Vector3(1, 0, 0), actionX);
        }
        if ((ball.transform.position.y - gameObject.transform.position.y) < -2f ||
            Mathf.Abs(ball.transform.position.x - gameObject.transform.position.x) > 3f ||
            Mathf.Abs(ball.transform.position.z - gameObject.transform.position.z) > 3f ||
            (gameObject.transform.position.y - goal.transform.position.y) < -3f)
        {
            SetReward(-5f);
            EndEpisode();
        }
        else
        {
            SetReward(0.25f);
        }

       

        SetReward(-1f / MaxStep);

    }

    public void RewardByDistance() {
        Vector3 offset = goal.transform.position - agent.transform.position;
        float sqrLen = offset.sqrMagnitude;

        // square the distance we compare with
        if (sqrLen < _oldDist * _oldDist)
        {
           // Debug.Log("Got Closer, old dist was: " + _oldDist);
            _oldDist -= 5f;
            //Debug.Log("old now: " + _oldDist);
            SetReward(1f);
        }
    }

    public void ScoredAGoal()
    {
        // We use a reward of 5.
        AddReward(10f);

        // By marking an agent as done AgentReset() will be called automatically.
        EndEpisode();
        //Debug.Log("GOAL SCORED!!!!!");
        // Swap ground material for a bit to indicate we scored.
        StartCoroutine(GoalScoredSwapGroundMaterial(_eggRaceSettings.goalScoredMaterial, 0.5f));
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                dirToGo = transform.right * -0.75f;
                break;
            case 6:
                dirToGo = transform.right * 0.75f;
                break;
        }
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        _agentRigidbody.AddForce(dirToGo * _eggRaceSettings.agentRunSpeed,
            ForceMode.VelocityChange);
    }

    public void SetGroundMaterialFriction()
    {
        var groundCollider = goal.GetComponent<Collider>();

        groundCollider.material.dynamicFriction = m_ResetParams.GetWithDefault("dynamic_friction", 0);
        groundCollider.material.staticFriction = m_ResetParams.GetWithDefault("static_friction", 0);
    }

    IEnumerator GoalScoredSwapGroundMaterial(Material mat, float time)
    {
        _goalRenderer.material = mat;
        yield return new WaitForSeconds(time); // Wait for 2 sec
        _goalRenderer.material = _goalMaterial;
    }

    public override void OnEpisodeBegin()
    {
       gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(1, 0, 0), Random.Range(-10f, 10f));
        gameObject.transform.Rotate(new Vector3(0, 0, 1), Random.Range(-10f, 10f));
        _ballRigidbody.velocity = new Vector3(0f, 0f, 0f);
        ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f))
            + gameObject.transform.position;
        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = -Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    public void SetBall()
    {
        //Set the attributes of the ball by fetching the information from the academy
        _ballRigidbody.mass = m_ResetParams.GetWithDefault("mass", 1.0f);
        var scale = m_ResetParams.GetWithDefault("scale", 1.0f);
        ball.transform.localScale = new Vector3(scale, scale, scale);
    }

    void ResetBlock()
    {
        // Get a random position for the block.
        agent.transform.position = resetPos.position;
        agent.transform.rotation = resetPos.rotation;

        // Reset block velocity back to zero.
        _agentRigidbody.velocity = Vector3.zero;

        // Reset block angularVelocity back to zero.
        _agentRigidbody.angularVelocity = Vector3.zero;
    }

    public void SetResetParameters()
    {
        ResetBlock();
        SetBall();
    }


}
