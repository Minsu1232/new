using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character/Archer")]
public class PlayerState : ScriptableObject
{
   public string characterName = "유다희";
    public int health = 100;
    public int str = 2;
    public int dex = 5;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public int mp = 50;
    public int level;
    public int hpUpgradeCount = 0; // hp스탯 업 한 수
    public int mpUpgradeCount = 0; // mp스탯 업 한 수
    public int strUpgradeCount = 0; // str스탯 업 한 수
    public int dexUpgradeCount = 0; // dex스탯 업 한 수
    public int baseUpgradeCost = 100; // 초기 업그레이드 비용

}
