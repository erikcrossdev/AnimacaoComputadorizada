using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoStaffPunch : MonoBehaviour
{
    public Vector3 punch;
    private Vector3 originalScale;
    public Animator anim;
    public GameObject staff;
    public DoPunchOnClick caulderon;
    // Start is called before the first frame update
   
    void Start()
    {
        originalScale = staff.transform.lossyScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoShakeInTheEnd()
    {
        staff.transform.DOPunchScale(punch, 1f, 10, 2).OnComplete(() => {
            staff.transform.DOScale(originalScale, 0.25f);
        });
        caulderon.DoPunch();
    }

   
}
