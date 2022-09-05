using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentFollowTarget : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Transform target;
    public float closeDistance;
    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = target.position - transform.position;
        float sqrLen = offset.sqrMagnitude;

        // square the distance we compare with
        if (!(sqrLen < closeDistance * closeDistance))
        {
            _agent.SetDestination(target.transform.position);
        }
    }
}
