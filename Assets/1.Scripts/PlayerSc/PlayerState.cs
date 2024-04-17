using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character/Archer")]
public class PlayerState : ScriptableObject
{
   public string characterName = "πÃ¡§";
    public int health = 100;
    public int str = 2;
    public int dex = 5;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public int mp = 50;

}
