using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // 데미지 : amount > 0, 회복 : amount < 0
    void ChangeHealth(int amount);
    int CurrentHealth { get; }
    int MaxHealth { get; }
}