using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    [SerializeField] private float deadzone = 0.3f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float windupTime = 0.5f;

    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private float shootCooldown = 0.1f;
    [SerializeField] private string collideWithTag = "Untagged";

    private float elapsedWindup = 0f;
    private float lastShootTimestamp = Mathf.NegativeInfinity;

    void Update()
    {
        UpdateMovement();
        UpdateActions();
    }

    void UpdateMovement()
    {
        float move = Input.GetAxis("Horizontal");
        if (Mathf.Abs(move) < deadzone) { return; }

        move = Mathf.Sign(move);
        float delta = move * speed * Time.deltaTime;
        transform.position = GameManager.Instance.KeepInBounds(transform.position + Vector3.right * delta);
    }

    void UpdateActions()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (elapsedWindup == 0)
            {
                SoundManager.instance.PlayClip("Windup");
            }
            elapsedWindup = math.clamp(elapsedWindup + Time.deltaTime, 0, windupTime);
            if (elapsedWindup >= windupTime && Time.time > lastShootTimestamp + shootCooldown)
            {
                elapsedWindup = windupTime;
                Shoot();
            }
        }
        else
        {
            elapsedWindup = math.clamp(elapsedWindup - Time.deltaTime, 0, windupTime);
        }
    }

    void Shoot()
    {
        Vector2 shootAtPosition = shootAt.position + new Vector3(math.sin(Time.time*25)/6, 0, 0);
        Instantiate(bulletPrefab, shootAtPosition, Quaternion.identity);
        lastShootTimestamp = Time.time;
        EventsManager.Instance.OnShoot.Invoke();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != collideWithTag) { return; }

        GameManager.Instance.PlayGameOver();
        EventsManager.Instance.OnGameOver.Invoke();
    }
}
