using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MouseFollow : MonoBehaviour
{
    public AnimationCurve curve;
    public float Distance = 10;
    public GameObject staff;
    public Vector3 punch;
    private Vector3 originalScale;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = ray.GetPoint(Distance);
        transform.position = Vector3.Lerp(transform.position, pos, curve.Evaluate(0.01f));

    }

    public void OnClickStaff(DoPunchOnClick caulderon) {
        staff.transform.DOPunchScale(punch, 1f, 10, 2);
        anim.SetTrigger("shake");
       
    }

}
