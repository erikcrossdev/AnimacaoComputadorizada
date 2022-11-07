using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    [SerializeField] protected Animator Animator;
    [SerializeField] protected Rigidbody Rigidbody;
    [SerializeField] protected BoxCollider BoxCollider;

    public GameObject root;
    private Vector3 _pos;
    private Vector3 _rot;

    private bool activate = true;

    [SerializeField] private float _timeToEnableRagdoll;

    [SerializeField] protected List<Rigidbody> Rigidbodies;

    // Start is called before the first frame update
    void Start()
    {
        _pos = root.transform.position;
        _rot = root.transform.eulerAngles;
        // Animator = GetComponent<Animator>();
        // Rigidbody = GetComponent<Rigidbody>();
        // BoxCollider = GetComponent<BoxCollider>();
        AnimationActive(activate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {            
            activate = !activate;
            if (!activate)
            {
                Animator.SetTrigger("Die");
                StartCoroutine(WaitForSecondsToEnableRagDoll());
            }
            else
            {
                Animator.SetTrigger("Revive");
                AnimationActive(activate);
            }
        }
    }

    IEnumerator WaitForSecondsToEnableRagDoll() {
        yield return new WaitForSeconds(_timeToEnableRagdoll);
        AnimationActive(activate);
    }

    public void AnimationActive(bool active) {
        Animator.enabled = active;
        Rigidbody.detectCollisions = active;
        Rigidbody.isKinematic = active;
        BoxCollider.enabled = active;

        for (int i = 0; i < Rigidbodies.Count; i++)
        {
            Rigidbodies[i].isKinematic = active;
        }
        if (active) {
            root.transform.position = _pos;
            root.transform.eulerAngles = _rot;
        }
    }

}
