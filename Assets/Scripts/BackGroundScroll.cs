using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public float ScrollSpeed;
    public GameObject[] BackGrounds;
    private float _BackgroundScroll = 0;
    

    void Start()
    {
        //ScrollSpeed = 0.5f;
    }
    void Update()
    {
        _BackgroundScroll += Time.deltaTime * ScrollSpeed;
        gameObject.transform.position = new Vector3(0, gameObject.transform.position.y+Time.deltaTime * ScrollSpeed, 0);
        if (_BackgroundScroll > 10.8f)
        {
            _BackgroundScroll -= 10.8f;
            BackGrounds[0].transform.position = new Vector3(0, BackGrounds[1].transform.position.y - (10.8f*2), 0);
            GameObject[] temp = new GameObject[BackGrounds.Length];
            temp[0] = BackGrounds[1];
            temp[1] = BackGrounds[2];
            temp[2] = BackGrounds[0];
            BackGrounds = temp;
        }
    }
}
