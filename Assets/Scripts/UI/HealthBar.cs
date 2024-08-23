using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    private Player player;
    public GameObject playerGO;
    private void Start()
    {
        player = playerGO.GetComponent<Player>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = player.health;
        healthBar.value = player.health;
    }
    private void Update()
    {
        healthBar.value = player.health;
    }
    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }
}
