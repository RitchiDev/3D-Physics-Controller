using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IKnockbackAble, IDamageAble
{
    public void TakeDamage(int amount, bool forcedDamage = false)
    {

    }

    public void TakeKnockback(Vector3 sourcePosition)
    {

    }
}
