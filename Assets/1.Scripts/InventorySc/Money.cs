

using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Money", menuName = "Inventory/Money")]
public class Money : ScriptableObject
{
    [SerializeField]
    private int _money;
    private int lastAmount = 0;  // ���� �ݾ��� ������ ����

    public int money
    {
        get => _money;
        set
        {
            if (_money != value)
            {
                lastAmount = _money;  // ���� �� �ݾ��� ����
                _money = value;
                OnMoneyChanged?.Invoke(_money, _money - lastAmount);  // ����� �ݾ��� �Ѿװ� ȹ���� �ݾ��� �̺�Ʈ�� ����
            }
        }
    }

    public event Action<int, int> OnMoneyChanged;

}

