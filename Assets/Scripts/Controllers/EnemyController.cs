using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Biome biome;
    [SerializeField] int strength = 1;
    public void Fight(PlayerController player)
    {
        StartCoroutine(DoFight(player));
    }

    private IEnumerator DoFight(PlayerController player)
    {
        player.fighting = true;
        player.StateChanged();

        yield return new WaitForSeconds(1);
        player.Stamina -= 1;
        bool die = false;
        if (player.Stamina <= 0)
        {
            player.hurt = true;
        }
        else
        {
            die = true;
        }
        player.fighting = false;

        player.StateChanged();

        if (die)
        {
            Die();
        }
    }

    public void Die()
    {
        biome.enemy = null;
        GameObject.Destroy(this.gameObject);
        
    }
}
