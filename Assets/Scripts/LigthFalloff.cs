using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class LigthFalloff : MonoBehaviour
{
    public float FalloffTime = 1.5f;

    void Start()
    {
        Light2D light = GetComponent<Light2D>();
        light.color = new Color(255, Random.Range(0, 255f), 0);
        DOTween.To(() => light.intensity, x => light.intensity = x, 0, FalloffTime).OnComplete(() => Destroy(gameObject));
    }
}
