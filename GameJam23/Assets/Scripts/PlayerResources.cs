using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources _instance;
    public static PlayerResources Instance => _instance;
 
    public bool isAlive => _health > 0;

    public int MaxHealth = 6;
    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            HealthChanged?.Invoke();
            if (_health <= 0)
            {
                Death?.Invoke();
            }
        }
    }

    public UnityAction HealthChanged;
    public UnityAction Death;

    public float MaxMana = 100f;
    private float _mana;
    public float Mana
    {
        get { return _mana; }
        set
        {
            _mana = Mathf.Clamp(value, 0, MaxMana);
            ManaChanged?.Invoke();
        }
    }

    public UnityAction ManaChanged;

    [SerializeField] List<AudioClip> TakeDamageClips;
    AudioSource _audioSource;

    private void Awake()
    {
        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Health = MaxHealth;
        Mana = MaxMana;
    }

    public void Damage(int amount=1)
    {
        Health -= amount;
        _audioSource.PlayOneShot(TakeDamageClips[Random.Range(0, TakeDamageClips.Count - 1)]);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 200), $"HP: {Health} MP: {Mana}");
    }
}
