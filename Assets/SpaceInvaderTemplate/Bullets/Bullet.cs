using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Vector3 startVelocity; 
    [SerializeField] AnimationCurve speedOverTime;
    [SerializeField] Sprite[] CornSprites;
    [SerializeField] float TimeUntilPop = 1.5f;
    [SerializeField] float RandomPopOffset = 0.5f;


    private float elapsed = 0f;
    private bool popped = false;
    private float randomOffset = 0f;
    // Start is called before the first frame update
    void Start()
    {
        randomOffset = Random.Range(-RandomPopOffset, RandomPopOffset);
    }
    void Update()
    {

        elapsed += Time.deltaTime;
        if (startVelocity.y > 0) // if ally or ennemy
        {
            if (!popped && elapsed >= TimeUntilPop + randomOffset)
            {
                popped = true;
                SoundManager.instance.PlayClip("PopCornPop");
                //take random corn sprite
                Sprite randomSprite = CornSprites[Random.Range(0, CornSprites.Length)];
                GetComponent<SpriteRenderer>().sprite = randomSprite;
            }

        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = startVelocity * speedOverTime.Evaluate(elapsed);
        if (elapsed >= 5)
        {
            Destroy(gameObject);
        }
    }
}
