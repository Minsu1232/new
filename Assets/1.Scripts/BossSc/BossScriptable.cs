using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Boss")]
public class BossScriptable : ScriptableObject
{
    public string MonsterName;
    public int health = 300;
    public int damage = 30;
    public int walkSpeed = 5;
    public int runSpeed = 6;
    public int neutralizeValue = 100; // ����ȭ�ϴµ� �ʿ��� ��ġ
    public int destructionValue = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
