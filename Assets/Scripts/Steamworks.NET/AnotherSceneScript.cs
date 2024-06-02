using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherSceneScript : MonoBehaviour
{
    void Start()
    {
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name); // Steam ����� �̸��� ����մϴ�.
        }
        else
        {
            Debug.LogError("Steam is not initialized in this scene.");
        }
    }
}
