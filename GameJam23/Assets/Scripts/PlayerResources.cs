using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public int MaxHealth = 6;
    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Max(value, MaxHealth);
        }
    }

    public float MaxMana = 100f;
    private float _mana;
    public float Mana
    {
        get { return _mana; }
        set
        {
            _mana = Mathf.Max(value, MaxMana);
        }
    }

    public static PlayerResources _instance;
    public static PlayerResources Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Health = MaxHealth;
        Mana = MaxMana;
    }
}
