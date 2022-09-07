using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoPunchOnClick : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] ParticleSystem _particle;
    [SerializeField] Vector3 punch;
    [SerializeField] Vector3 punchPos;
    [SerializeField] Vector3 punchRot;
    [SerializeField] float duration;
    [SerializeField] float backToNormalDuration;
    [SerializeField] int vibrato = 10;
    [SerializeField] float elasticity = 1;
    [SerializeField] MeshRenderer potion;

    [SerializeField] Color newColor;
    [SerializeField] Color color;

    private Vector3 originalScale;
    private Vector3 originalPos;
    private Vector3 originalRot;

    [Header("CameraShake")]
    [SerializeField] int camVibrato = 10;
    [SerializeField] float camStrength = 10;
    [SerializeField] float camRandomness = 90;

    [Header("Other")]
    [SerializeField] float waitTime = 0.5f;
    public SetMaterialProperty _Twirl;
    [SerializeField] private Animator anim;
    public MouseFollow _mouse;

    private Coroutine _backToNormalRoutine;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        originalPos = transform.position;
        originalRot = new Vector3(0,0,0);
        anim.SetBool("Twirl", false);

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnMouseDown()
    {
        _mouse.OnClickStaff(this);
    }

    public void DoPunch() {
        anim.SetBool("Twirl", true);
        transform.DOKill();
        transform.localScale = originalScale;
        transform.DOPunchScale(punch, duration, vibrato, elasticity);
        transform.DOShakeScale(duration, punch, vibrato, 90, true);
        transform.DOMove(punchPos, duration);
        transform.DORotate(punchRot, duration);
        _particle.Play();
        potion.material.DOColor(newColor, duration).OnComplete(() =>
        {
            if (_backToNormalRoutine != null)
            {
                StopCoroutine(BackToNormalRoutine());
            }
            _backToNormalRoutine = StartCoroutine(BackToNormalRoutine());
            anim.SetBool("Twirl", false);
        });

    }

    private IEnumerator BackToNormalRoutine() {
        yield return new WaitForSeconds(waitTime);
        transform.DOKill();
        transform.DOScale(originalScale, backToNormalDuration);
        potion.material.DOColor(color, backToNormalDuration);
        transform.DOMove(originalPos, 1f);
        transform.DORotate(originalRot, 1f);

    } 

}
