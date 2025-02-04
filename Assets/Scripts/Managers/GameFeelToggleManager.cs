using System;
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
        public AGameFeelModule module;
    }
    public GameFeelFeature[] gameFeelObjects;

    private void Update()
    {
        CheckInputGameFeelFeature();
    }


    private void CheckInputGameFeelFeature()
    {
        for (int i = 0; i < gameFeelObjects.Length; i++)
        {
            if (Input.GetKeyDown(gameFeelObjects[i].key))
            {
                gameFeelObjects[i].enabled = !gameFeelObjects[i].enabled;
                
                if (gameFeelObjects[i].enabled)
                    gameFeelObjects[i].module.ToggleOffGamefeelFeatures();
                else
                    gameFeelObjects[i].module.ToggleOnGamefeelFeatures();
                
                Debug.Log("Key:" + gameFeelObjects[i].key + " enabled:" + gameFeelObjects[i].enabled);
            }
        }
    }
}
