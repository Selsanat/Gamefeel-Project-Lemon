using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private string collideWithTag = "Player";
    
    [SerializeField] private int Dommage = 10;
    
    [SerializeField] private int Pv;
    [SerializeField] private int PvMax = 100;
    [SerializeField] private int PvPourcentLow = 30;   
    [SerializeField] private int PvPourcentMid = 50;
    
    private int PvStateMid;
    private int PvStateLow;
    
    [SerializeField] private float GrowMid = 1.5f;
    [SerializeField] private float GrowLow = 2.5f;
    [SerializeField] private float GrowSpeed = 1f;
    

    internal Action<Invader> onDestroy;
    
    enum InvaderState
    {
        MoveFull,
        MoveMid,
        MoveLow,
        Shoot
    }
    InvaderState state = InvaderState.MoveFull;
    public Vector2Int GridIndex { get; private set; }

    private void Start()
    {
        Pv = PvMax;
        PvStateMid = PvMax * PvPourcentMid / 100;
        PvStateLow = PvMax * PvPourcentLow / 100;
    }

    public void Initialize(Vector2Int gridIndex)
    {
        this.GridIndex = gridIndex;
    }

    public void OnDestroy()
    {
        onDestroy?.Invoke(this);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != collideWithTag) { return; }
        
        GetHit();
        Destroy(collision.gameObject);
    }

    private void GetHit()
    {
        Pv -= Dommage;
        if (Pv <= PvStateMid && Pv > PvStateLow)
        {
            state = InvaderState.MoveMid;
            AnimSwitch();
        }
        if (Pv <= PvStateLow && Pv > 0)
        {
            state = InvaderState.MoveLow;
            AnimSwitch();
        }
        if(Pv <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void AnimSwitch()
    {
        switch (state)
        {
            case InvaderState.MoveLow:
                this.gameObject.transform.localScale = new Vector3(GrowLow, GrowLow, GrowLow);
                break;
            case InvaderState.MoveMid:
                this.gameObject.transform.localScale = new Vector3(GrowMid, GrowMid, GrowMid);
                break;
            case InvaderState.MoveFull:
                break;
        }
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, shootAt.position, Quaternion.identity);
    }
}
