using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamInit : MonoBehaviour
{
    void Start()
    {
        Debug.Log("SteamInit Start called.");
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam is not initialized.");
            return;
        }
        Debug.Log("Steam initialized successfully.");
        DontDestroyOnLoad(gameObject);
    }
}
