

using UnityEngine;
[CreateAssetMenu(fileName = "Money", menuName = "Inventory/Money")]
public class Money : ScriptableObject
{
    public int money;


    private void OnEnable()
    {
        money = 10000; // test ���Ǽ��� ���� (������ ���� �� ���￹��)
    }
    
}

