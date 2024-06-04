using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Boss")]
public class BossScriptable : ScriptableObject
{
    public string MonsterName;
    public int health = 400;
    public int damage = 25;
    public int walkSpeed = 6;
    public int runSpeed = 8;
    public int neutralizeValue = 150; // 무력화하는데 필요한 수치
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
