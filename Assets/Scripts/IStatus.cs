using System;
using static Define;
using UnityEngine;

public interface IStatus
{
    int Health { get;}
    void TakeDamage(int Damage, Elemental skillElement);

    void Die();
}