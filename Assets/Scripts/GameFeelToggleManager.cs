using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class GameFeelToggleManager : MonoBehaviour
{
    public struct GameFeelFeature
    {
        public KeyCode key;
        public bool enabled;
    }
    public GameFeelFeature[] gameFeelObjects;
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
    }
    void OnKeyDown(KeyDownEvent ev)
    {
        for (int i = 0; i < gameFeelObjects.Length; i++)
        {
            if(gameFeelObjects[i].key == ev.keyCode)
            {
                gameFeelObjects[i].enabled = !gameFeelObjects[i].enabled;
                Debug.Log("Key:" + ev.keyCode + " enabled:" + gameFeelObjects[i].enabled);
            }
        }
    }

   
    

    
}
