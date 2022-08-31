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
 

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        originalPos = transform.position;
        originalRot = new Vector3(0,0,0);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {

        transform.DOKill();
        transform.localScale = originalScale;
        Debug.Log("ON MOUSE DOWN");
        transform.DOPunchScale(punch, duration, vibrato, elasticity);
        transform.DOShakeScale(duration, punch, vibrato, 90, true);
        transform.DOMove(punchPos, duration);
        transform.DORotate(punchRot, duration);
        _particle.Play();
        potion.material.DOColor(newColor, duration).OnComplete(() =>
        {
            transform.DOScale(originalScale, backToNormalDuration);
            potion.material.DOColor(color, backToNormalDuration);
            transform.DOMove(originalPos, 1f);
            transform.DORotate(originalRot, 1f);
        });

    }

}
