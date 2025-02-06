using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class UIText : MonoBehaviour
{

    [SerializeField] float floatAmplitude;
    [SerializeField] float floatSpeed;
    private float i = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        i += floatSpeed;
        this.GetComponent<RectTransform>().localPosition = new Vector2(0,Mathf.Sin(i) * floatAmplitude);
    }
}
