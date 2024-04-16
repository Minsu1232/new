using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Monster", menuName = "Monster/Normal")]
public class NormalMonsterScriptable : ScriptableObject
{
    public string MonsterName = "";
    public int health = 80;
    public int damage = 15;
    public int walkSpeed = 4;
    public int runSpeed = 6;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
