

using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Money", menuName = "Inventory/Money")]
public class Money : ScriptableObject
{
    [SerializeField]
    private int _money;
    private int lastAmount = 0;  // 이전 금액을 저장할 변수

    public int money
    {
        get => _money;
        set
        {
            if (_money != value)
            {
                lastAmount = _money;  // 변경 전 금액을 저장
                _money = value;
                OnMoneyChanged?.Invoke(_money, _money - lastAmount);  // 변경된 금액의 총액과 획득한 금액을 이벤트로 전달
            }
        }
    }

    public event Action<int, int> OnMoneyChanged;

}

