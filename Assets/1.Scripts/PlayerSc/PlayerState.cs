using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character/Archer")]
public class PlayerState : ScriptableObject
{
   public string characterName = "������";
    public int health = 100;
    public int str = 2;
    public int dex = 5;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public int mp = 50;
    public int level;
    public int hpUpgradeCount = 0; // hp���� �� �� ��
    public int mpUpgradeCount = 0; // mp���� �� �� ��
    public int strUpgradeCount = 0; // str���� �� �� ��
    public int dexUpgradeCount = 0; // dex���� �� �� ��
    public int baseUpgradeCost = 100; // �ʱ� ���׷��̵� ���

}
