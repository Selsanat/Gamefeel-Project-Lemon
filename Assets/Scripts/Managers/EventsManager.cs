using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventsManager : MonoBehaviour
{
    private static EventsManager _instance;

    [Header("Events List")]
    public UnityEvent OnGameStart;

    public UnityEvent OnGameOver;
    public UnityEvent OnGameWin;
    public UnityEvent OnEnnemyKilled;
    public UnityEvent OnShoot;
    public UnityEvent OnMove;
    public UnityEvent OnStop;
    public UnityEvent OnEnnemyHit;
    public UnityEvent OnBulletPop;

    public Player Player;
    public Wave Wave;
    public BackGroundScroll BackGroundScroll;

    public Image UIbg;
    public Image UIdefeat;
    public Image UIvictory;
    public Image UIfirePanel;


    // Print something when events are called
    private void Start()
    {
        OnGameStart.AddListener(OnGameStartBehaviour);
        OnGameOver.AddListener(OnGameOverBehaviour);
        OnGameWin.AddListener(OnGameWinBehaviour);
        OnEnnemyKilled.AddListener(OnEnnemyKilledBehaviour);    
        OnShoot.AddListener(OnShootBehaviour);
        OnMove.AddListener(OnMoveBehavior);
        OnStop.AddListener(OnStopBehavior);
        OnEnnemyHit.AddListener(OnEnnemyHitBehavior);
        OnBulletPop.AddListener(OnBulletPopBehavior);
        OnGameStart.Invoke();
    }

    private void OnGameStartBehaviour()
    {
        SoundManager.instance.PlayClip("BGMGameplay");
        SoundManager.instance.PlayClip("Engine");
    }
    private void OnGameOverBehaviour()
    {
        SoundManager.instance.Pauseclip("BGMGameplay");
        SoundManager.instance.Pauseclip("Engine");
        SoundManager.instance.PlayClip("PlayerDeath");
        StartCoroutine(Death());
    }
    private void OnGameWinBehaviour()
    {
        SoundManager.instance.Pauseclip("BGMGameplay");
        SoundManager.instance.PlayClip("BGMVictory");
        SoundManager.instance.Pauseclip("Engine");
        UIdefeat.DOFade(0, 0.0001f);
        UIbg.DOFade(1, 0.5f);
        UIvictory.DOFade(1, 0.5f);
        Player.enabled = false;
        Wave.enabled = false;
    }
    private void OnEnnemyKilledBehaviour()
    {
        SoundManager.instance.PlayClip("ChickDeath");
    }

    private void OnMoveBehavior()
    {
        SoundManager.instance.ChangePitch("Engine", 0.5f);
    }

    private void OnStopBehavior()
    {
        SoundManager.instance.ChangePitch("Engine", 0f);
    }

    private void OnShootBehaviour()
    {
        print("Shooting");
        UIfirePanel.DOFade(0.03f, 0);
        UIfirePanel.DOFade(0, 0.1f);
        SoundManager.instance.PlayRandomClip("Gun");
        // make the camera shake using dotween
        CameraManager instance = CameraManager.instance;
        if (DOTween.IsTweening(Camera.main.transform)) return;
        Camera.main.transform.DOShakePosition(instance.ShootShakeDuration, instance.ShootShakePower);
    }

    private void OnEnnemyHitBehavior()
    {
        SoundManager.instance.PlayClip("ChickPiou");
    }

    private void OnBulletPopBehavior()
    {
        SoundManager.instance.PlayClip("PopcornPop");
    }

    public static EventsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EventsManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(EventsManager).ToString());
                    _instance = singleton.AddComponent<EventsManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Death()
    {
        if (Player != null)
        {
            Player.enabled = false;
        }
        if (BackGroundScroll != null)
        {
            BackGroundScroll.ScrollSpeed = 0.5f;
        }
        if (Wave != null)
        {
            Wave.enabled = false;
        }
        CameraManager instance = CameraManager.instance;
        Camera.main.transform.DOShakePosition(instance.PlayerDeathShakeDuration, instance.PlayerDeathShakePower, 10);
        yield return new WaitForSeconds(2f);
        UIbg.DOFade(1, 0.5f);
        UIdefeat.DOFade(1, 0.5f);
        //Time.timeScale = 0;
        SoundManager.instance.PlayClip("BGMDefeat");
        
    }
}
