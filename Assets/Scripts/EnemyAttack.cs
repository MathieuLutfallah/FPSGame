using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Collections.Generic;


public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 5;
    public float nextTimeFire = 0f;
    public float fireRate = 1.25f;
    Animator anim;
    public float turnSpeed;
    GameObject player;
    PlayerScript playerHealth;
    public GameObject pistolAmmoBox;
    public GameObject HealthBox;
    public GameObject ArAmmoBox;
    public bool isRanged;
    public AudioSource AS;
    public AudioClip AC1;
    public AudioClip AC2;
    UnityEngine.AI.NavMeshAgent agent;
    Transform target;
    public float playerInRange;
    public bool ranged;
    public float startingHealth;
    public float currentHealth;
    public float sinkSpeed = 1.5f;

    public GameObject headshot;

    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    public bool isDead;
    public bool isSinking;



    //Script managing the enemies speciying their damage, life and speed
    void Awake ()
    {
       // Debug.Log("my name is" + this.transform.name);
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerScript> ();
        AS = GetComponent<AudioSource>();
        anim = GetComponent <Animator> ();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = player.transform;
       
        capsuleCollider = GetComponent<CapsuleCollider>();
        //Debug.Log("StartHelth " + startingHealth);
        currentHealth = startingHealth;
        isDead = false;
    }

    //Set the life and damage of the zombie before its enabled in the game
    public void setLifeAndDamage(int life,int damage){
        startingHealth = life;
        attackDamage = damage;

    }


    void Update ()
    {
      
        float distance = Vector3.Distance(transform.position, target.position);
        if (isSinking)//Makes the enemy goes down after its dead
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
        else
        {
            if (!isDead)
            {
                if (distance > playerInRange)  //Check if the player is in the attacking range to decide wether to move or attack
                {
                    if (!AS.isPlaying)
                        AS.PlayOneShot(AC1);
                    //Enable the navigation agent to make the object walk towards player
                    if (!isRanged)
                    {
                        agent.updatePosition = true;
                        agent.updateRotation = true;
                    }
                    else
                    {
                        agent.enabled = true;
                    }
                    agent.SetDestination(target.position);
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttack", false);
                }
                else
                {
                    //Attack the player if the distance allows it
                    if (!isRanged)
                    {
                        //Debug.Log("attacking");
                        if (!AS.isPlaying)
                            AS.PlayOneShot(AC2);
                        
                        if (Time.time >= nextTimeFire && currentHealth > 0)
                        {
                            nextTimeFire = Time.time + 1f / fireRate;
                            Attack();
                        }
                        agent.updateRotation = false;
                        Vector3 direction = target.position - transform.position;
                        direction.y = 0;

                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
                        agent.updatePosition = false;
                        anim.SetBool("isWalking", false);
                        anim.SetBool("isAttack", true);
                    }
                    else
                    {
                        agent.enabled = false;
        
                        Vector3 direction = target.position - transform.position;
                        direction.y = 0;
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

                        anim.SetBool("isAttack", true);
                        anim.SetBool("isWalking", false);

                    }

                }
            }
        }
 


    }

    //Attack the player by calling its health script and removing some if his health
    void Attack ()
    {
        AS.Stop();
        if (!AS.isPlaying & isRanged)
        {
            Debug.Log("shooting sound");
            AS.PlayOneShot(AC2);
        }
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }

    //function called when the player shoots the enemy
    public bool TakeDamage(float amount, Vector3 point)
    {
        if (isDead)
            return false;


        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
            return true;
        }
        return false;
    }

    //Reset the enemy components before being introduced to the game
    public void reset()
    {
        isDead = false;
        capsuleCollider.isTrigger = false;
        currentHealth = startingHealth;
        anim.SetBool("isWalking", true);
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;

        isSinking = false;

    }


    //Start the death animation: falling back then sinking down
    void Death()
    {
        isDead = true;



        StartCoroutine(DeathAnimation());
    }


    //Enemy dying by disabling it as well as all of his components, the enemy falls back and then goes down below the ground, finally the enemy has a probability to drop a box or not
    IEnumerator DeathAnimation()
    {
        anim.SetTrigger("Dead");
        //Debug.Log("dead");
        isDead = true;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        Vector3 positonEnemy = this.transform.position;
        positonEnemy.y += 1;
        float ran = Random.Range(0.0f, 1.0f);

        if (ran > 0.5f)
        {

        }
        else
        {
            if (ran < 0.25)
                Instantiate(pistolAmmoBox, positonEnemy, pistolAmmoBox.transform.rotation);
            else
            {
                if (ran < 0.4)
                    Instantiate(ArAmmoBox, positonEnemy, ArAmmoBox.transform.rotation);
                else
                    Instantiate(HealthBox, positonEnemy, HealthBox.transform.rotation);
            }
        }
        yield return new WaitForSeconds(1f);
        if(!isRanged)
        headshot.GetComponent<HeadShot>().stopblood();
        //Debug.Log("sinking");
        capsuleCollider.isTrigger = true;
        isSinking = true;
        yield return new WaitForSeconds(1);
        //Debug.Log("end of siniking");



        isSinking = false;
        gameObject.transform.parent.GetComponent<EnemyManager>().removeEnemy();
        if(!isRanged)
        headshot.GetComponent<HeadShot>().returnHead();
        //Debug.Log(this.transform.parent.GetComponent<EnemyManager>().pool.GetComponent<PoolScript>().batata);

        if (isRanged)
        {
            gameObject.transform.parent.GetComponent<EnemyManager>().pool[1].gameObject.GetComponent<PoolScript>().recyclePool(this.gameObject);
        }
        else
        {
            gameObject.transform.parent.GetComponent<EnemyManager>().pool[0].gameObject.GetComponent<PoolScript>().recyclePool(this.gameObject);
        }
    }
}
