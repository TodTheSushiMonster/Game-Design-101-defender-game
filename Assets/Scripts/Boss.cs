using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float health = 10000;
    public int scoreValue = 1000;
    public float attackCounter;
    public List<GameObject> Attacks;
    public float minTimeBetweenAttacks = .2f;
    public float maxTimeBetweenAttacks = 2f;
    public bool attacking = false;
    public GameObject explosionVFX;
    public float durationOfExplosion;
    public float deathDelay = 2f;

    void Start()
    {
        attackCounter = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking)
        {
            CountDownAndAttack();
        }
    }

    private void CountDownAndAttack()
    {
        if (attackCounter <= 0f)
        {
            attacking = true;
            Attack();
            attackCounter = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
        }
        attackCounter -= Time.deltaTime;
    }

    private void Attack()
    {
        var attack = Attacks[Random.Range(0, Attacks.Count)];
        GameObject attacker = Instantiate(
            attack,
            transform.position,
            transform.rotation)
            as GameObject;
        Debug.Log(transform.position);
        attacking = false;
    }

    public void EndOfAttack()
    {
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        FindObjectOfType<Level>().SubtractEnemy();
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosion);
        Destroy(gameObject, deathDelay);
    }
}
