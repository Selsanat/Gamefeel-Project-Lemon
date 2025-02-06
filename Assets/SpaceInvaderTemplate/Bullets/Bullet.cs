using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Vector3 startVelocity; 
    [SerializeField] AnimationCurve speedOverTime;

    private float elapsed = 0f;

    // Start is called before the first frame update
    //void Awake()
    //{
    //    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    //    rb.velocity = startVelocity;
    //}
    void Update()
    {
        elapsed += Time.deltaTime;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = startVelocity * speedOverTime.Evaluate(elapsed);
    }
}
