using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private float DurationScaleHit = 0.2f;
    [SerializeField] private float ScaleMutiplicatorHitY = 2f;
    [SerializeField] private float ScaleMutiplicatorHitX = 2f;
    public Vector3 BasePosition;
    internal Action<Invader> onDestroy;
    private CameraShake cameraShake;
    private float BaseScaleX;
    private float BaseScaleY;
    private bool isAlive = true;
    [SerializeField] private GameObject Poussin;
    [SerializeField] private Animator animator;

    
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
        cameraShake = Camera.main.GetComponent<CameraShake>();
        BaseScaleX = this.gameObject.transform.localScale.x;
        BaseScaleY = this.gameObject.transform.localScale.y;

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
        if(collision.gameObject.tag != collideWithTag || !isAlive) { return; }
        
        GetHit();
        Destroy(collision.gameObject);
    }

    private void GetHit()
    {
        AnimHit();
        animator.SetTrigger("Enemy_Hitted");
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
            
            Debug.Log("dead");
            cameraShake.Shake();
            FindObjectOfType<FlashInvader>().Flash();
            this.GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(Poussin, new Vector3(transform.position.x,transform.position.y , 0.28f), Quaternion.identity, new RectTransform());
            isAlive = false;
        }
    }

    private void AnimHit()
    {
        transform.DOScaleX(this.transform.localScale.x * ScaleMutiplicatorHitX, DurationScaleHit);
        transform.DOScaleX(BaseScaleX, DurationScaleHit);
        transform.DOScaleY(this.transform.localScale.y * -ScaleMutiplicatorHitY, DurationScaleHit);
        transform.DOScaleY(BaseScaleY, DurationScaleHit);
        
    }
    private void AnimSwitch()
    {
        switch (state)
        {
            case InvaderState.MoveLow:
                animator.SetInteger("Enemy_Phase", 2);
                break;
            case InvaderState.MoveMid:
                animator.SetInteger("Enemy_Phase", 1);
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
