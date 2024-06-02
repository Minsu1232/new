using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string bowName;
    public int enhancementLevel; // 강화 레벨
    public int baseAttackPower; // 기본 공격력
    public int enhancedAttackPower; // 강화된 공격력 계산 후 저장
    public int enhancementCost; // 강화 비용 

}
