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
    public AudioSource audiosource;
    public AudioClip enemmyDie;

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
        
        // fight
        for(int i=0; i<strength; i++)
        {
            yield return new WaitForSeconds(i==0 ? 5f : fightDuration);
            player.Stamina--;
            if (player.Stamina <= 0)
                break;
        }
        
        // resolve
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
        player.canMove = true;
        player.StateChanged();

        audiosource.clip = enemmyDie;
        audiosource.Play();
        GetComponent<PlayerController>().death = true;

        yield return new WaitForSeconds(0.43f * 6);
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
