using System.Collections;
using System.Collections.Generic;
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
        OnGameStart.AddListener(() => Debug.Log("Game Started"));
        OnGameOver.AddListener(() => Debug.Log("Game Over"));
        OnGameWin.AddListener(() => Debug.Log("Game Win"));
        OnEnnemyKilled.AddListener(() => Debug.Log("Ennemy Killed"));
        OnShoot.AddListener(() => Debug.Log("Shoot"));
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
