using System;
using TMPro;
using UnityEngine;

public class State : MonoBehaviour
{
    [SerializeField] private TextMeshPro hpText;
    [SerializeField] private int _hp;
    [SerializeField] private int _maxHp;

    public void Start()
    {
        hpText = GetComponentInChildren<TextMeshPro>();
        Hp = _maxHp;
    }

    public int Hp
    {
        get => _hp;
        set
        {
            if (_hp > value)
            {
                try
                {
                    transform.SendMessage("Hit");
                }
                catch
                {
                    // Nothing 
                }
            }
            
            _hp = Math.Max(value, 0);
            hpText.text = $"{_hp}/{_maxHp}";
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