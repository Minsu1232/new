using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string bowName;
    public int enhancementLevel; // ��ȭ ����
    public int baseAttackPower; // �⺻ ���ݷ�
    public int enhancedAttackPower; // ��ȭ�� ���ݷ� ��� �� ����
    public int enhancementCost; // ��ȭ ��� 

}
