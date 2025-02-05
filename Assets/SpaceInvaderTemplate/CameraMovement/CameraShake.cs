using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float strength = 1.0f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float randomness = 90f;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }
    
    public void Shake()
    {
        return;
        transform.DOShakePosition(duration, strength, vibrato, randomness).OnComplete(() => transform.position = originalPosition);;
        
    }
    
    
    
}
