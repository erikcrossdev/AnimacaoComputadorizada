using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MouseFollow : MonoBehaviour
{
    public float Distance = 10;
    public GameObject staff;
    public GameObject staffPivot;
    public Vector3 punch;
    private Vector3 originalScale;
    public Animator anim;
    Vector3 pos1;
    Vector3 pos2;

    public float velocity1;
    public float velocity2;
    public float velocity3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = ray.GetPoint(Distance);
        
        var target = pos;

        pos1 = Vector3.Lerp(pos1, target, Time.deltaTime * velocity1);
        pos2 = Vector3.Lerp(pos2, pos1, Time.deltaTime * velocity2);

        pos2.y = pos1.y - velocity3;

        var vel = pos2 - pos1;

        var angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg + 90;

        staffPivot.transform.eulerAngles = new Vector3(0, 0, angle);
        transform.position = pos1;

    }

    public void OnClickStaff(DoPunchOnClick caulderon) {
        staff.transform.DOPunchScale(punch, 1f, 10, 2);
        anim.SetTrigger("shake");
       
    }

}
