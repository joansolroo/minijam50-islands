using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Biome biome;
    [SerializeField] int strength = 1;
    public float fightDuration;
    public List<Vector3> combatSpots;

    public void Fight(PlayerController player, PeopleManager followers)
    {
        StartCoroutine(DoFight(player, followers));
    }

    private IEnumerator DoFight(PlayerController player, PeopleManager followers)
    {
        // engage combat
        Debug.Log("toto");
        player.canMove = false;
        player.fighting = true;
        player.StateChanged();
        for(int i=0; i<combatSpots.Count; i++)
            combatSpots[i] += transform.position;
        followers.EngageCombat(combatSpots);
        yield return new WaitForSeconds(strength * fightDuration);


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
        player.canMove = true;
    }

    public void Die()
    {
        biome.enemy = null;
        GameObject.Destroy(this.gameObject);
        
    }
}
