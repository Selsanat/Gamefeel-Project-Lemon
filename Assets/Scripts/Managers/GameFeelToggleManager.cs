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
    public GameFeelFeature[] gameFeelObjects = new GameFeelFeature[1];

    private void Start()
    {
        GameFeelFeature test = new GameFeelFeature();
        test.key = KeyCode.A;
        test.enabled = true;
        test.module = null;
        gameFeelObjects.Initialize();
        gameFeelObjects[0] = test;
    }

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
                Debug.Log("Key:" + gameFeelObjects[i].key + " enabled:" + gameFeelObjects[i].enabled);

                gameFeelObjects[i].enabled = !gameFeelObjects[i].enabled;
                
                if (gameFeelObjects[i].enabled && gameFeelObjects[i].module != null)
                    gameFeelObjects[i].module.ToggleOffGamefeelFeatures();
                else if(!gameFeelObjects[i].enabled && gameFeelObjects[i].module != null)
                    gameFeelObjects[i].module.ToggleOnGamefeelFeatures();
                
            }
        }
    }
}
