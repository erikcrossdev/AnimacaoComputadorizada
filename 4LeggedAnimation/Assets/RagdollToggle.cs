using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    [SerializeField] protected Animator Animator;
    [SerializeField] protected Rigidbody Rigidbody;
    [SerializeField] protected BoxCollider BoxCollider;

    public GameObject root;

    private bool activate = false;

    [SerializeField] protected List<Rigidbody> Rigidbodies;

    // Start is called before the first frame update
    void Start()
    {
        
        // Animator = GetComponent<Animator>();
        // Rigidbody = GetComponent<Rigidbody>();
        // BoxCollider = GetComponent<BoxCollider>();
        RagdollActive(activate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RagdollActive(bool active) {
        Animator.enabled = !active;
        Rigidbody.detectCollisions = !active;
        Rigidbody.isKinematic = !active;
        BoxCollider.enabled = !active;

        for (int i = 0; i < Rigidbodies.Count; i++)
        {
            Rigidbodies[i].isKinematic = !active;
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("RagdollActive"))
        {
            activate = !activate;
            RagdollActive(activate);
        }
    }
}
