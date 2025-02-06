using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // make it a singleton
    public static CameraManager instance;

    [Header("Shooting Shake Parameters")]
    public float ShootShakePower = 0.1f;
    public float ShootShakeDuration = 0.1f;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
}
