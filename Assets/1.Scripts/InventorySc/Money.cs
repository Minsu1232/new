

using UnityEngine;
[CreateAssetMenu(fileName = "Money", menuName = "Inventory/Money")]
public class Money : ScriptableObject
{
    public int money;


    private void OnEnable()
    {
        money = 10000; // test 편의성을 위해 (데이터 저장 후 지울예정)
    }
    
}

