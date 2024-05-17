

using UnityEngine;
[CreateAssetMenu(fileName = "Money", menuName = "Inventory/Money")]
public class Money : ScriptableObject
{
    public int money;
    public delegate void MoneyChanged(int newAmount);
    public event MoneyChanged OnMoneyChanged;

    public int MoneyValue
    {
        get { return money; }
        set
        {
            if (money != value)
            {
                money = value;
                OnMoneyChanged?.Invoke(money);
            }
        }
    }

    private void OnEnable()
    {
         // test ���Ǽ��� ���� (������ ���� �� ���￹��)
    }
    
}

