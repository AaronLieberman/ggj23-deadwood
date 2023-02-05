using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBinding : MonoBehaviour
{
    public GameObject HeartPrefab;

    public int HeartSize = 100;

    private List<HeartSpriteSwitch> hearts = new List<HeartSpriteSwitch>();

    private void Start()
    {
        var playerHealth = PlayerResources.Instance;
        playerHealth.HealthChanged += UpdateHealth;

        for (int i = 0; i < playerHealth.MaxHealth; i++)
        {
            var HeartObj = Instantiate(HeartPrefab, this.transform);
            hearts.Add(HeartObj.GetComponent<HeartSpriteSwitch>());
            var rectTransform = HeartObj.transform as RectTransform;
            rectTransform.anchoredPosition = new Vector2(0 + HeartSize * i, 0);
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //PlayerResources.Instance.Damage(1);
        }
    }

    void UpdateHealth()
    {
        var playerHealth = PlayerResources.Instance;
        for (int i = 0; i < playerHealth.MaxHealth; i++)
        {
            var heart = hearts[i];
            heart.Filled = (i + 1) <= playerHealth.Health;
        }
    }
}
