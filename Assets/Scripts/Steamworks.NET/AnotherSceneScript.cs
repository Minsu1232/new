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
            Debug.Log(name); // Steam 사용자 이름을 출력합니다.
        }
        else
        {
            Debug.LogError("Steam is not initialized in this scene.");
        }
    }
}
