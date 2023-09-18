using System;
using UnityEngine;

public class State : MonoBehaviour
{
    private int _hp;

    public int Hp
    {
        get => _hp;
        set
        {
            _hp = Math.Max(value, 0);
            if (_hp == 0)
            {
                transform.SendMessage("Dead");
            }
        }
    }

    public void GetDamage(int damage)
    {
        Hp -= damage;
    }
}