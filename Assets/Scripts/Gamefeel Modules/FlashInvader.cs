using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlashInvader : MonoBehaviour
{
    [SerializeField] Material material;

    private void Start()
    {
        material.DOFade(0, 0.000000000001f).SetEase(Ease.OutQuint);
    }

    public void Flash()
    {
        Debug.Log("flash");
        material.DOFade(5.1f, 1f).SetEase(Ease.OutQuint);
        material.DOFade(0, 1f).SetEase(Ease.OutQuint);

    }
}
