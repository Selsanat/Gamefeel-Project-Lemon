using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    private static EventsManager _instance;

    [Header("Events List")]
    public UnityEvent OnGameStart;

    public UnityEvent OnGameOver;
    public UnityEvent OnGameWin;
    public UnityEvent OnEnnemyKilled;
    public UnityEvent OnShoot;


    // Print something when events are called
    private void Start()
    {
        OnGameStart.AddListener(OnGameStartBehaviour);
        OnGameOver.AddListener(OnGameOverBehaviour);
        OnGameWin.AddListener(OnGameWinBehaviour);
        OnEnnemyKilled.AddListener(OnEnnemyKilledBehaviour);    
        OnShoot.AddListener(OnShootBehaviour);
        OnGameStart.Invoke();
    }

    private void OnGameStartBehaviour()
    {
        SoundManager.instance.PlayClip("BGMGameplay");
    }
    private void OnGameOverBehaviour()
    {
        SoundManager.instance.Pauseclip("BGMGameplay");
        SoundManager.instance.PlayClip("BGMDefeat");
    }
    private void OnGameWinBehaviour()
    {
        SoundManager.instance.Pauseclip("BGMGameplay");
        SoundManager.instance.PlayClip("BGMVictory");
    }
    private void OnEnnemyKilledBehaviour()
    {
        SoundManager.instance.PlayClip("ChickDeath");
    }

    private void OnShootBehaviour()
    {
        SoundManager.instance.PlayRandomClip("Gun");
        // make the camera shake using dotween
        CameraManager instance = CameraManager.instance;
        if (DOTween.IsTweening(Camera.main.transform)) return;
        Camera.main.transform.DOShakePosition(instance.ShootShakeDuration, instance.ShootShakePower);
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
}
