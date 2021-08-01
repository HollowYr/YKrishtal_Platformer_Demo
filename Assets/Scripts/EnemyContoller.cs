/***
 * Enemy Behaviour logic for attack, move, die and take Damage.
 ***/
using System;
using System.Collections;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int damageAmount, atkDamageAmount;
    [SerializeField] private Dissolve dissolve;
    [SerializeField] private Transform[] attackPoints;
    [SerializeField] private float attackRange = .5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] internal Transform[] patrolPoints;

    bool facingRight = false;
    private Animator animator;
    private int currentHealth;
    internal Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public bool isDead()
    {
        return currentHealth <= 0;
    }

    public void MoveTo(Transform destination, float speed)
    {
        rb.position = Vector2.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);

        if ((transform.position.x < destination.position.x && facingRight)
            || (transform.position.x > destination.position.x && !facingRight))
        {
            Flip();
        }
    }
    public void Attack()
    {
        animator.Play(transform.name + "_Attack1");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoints[0].position, attackRange, enemyLayer);
        StartCoroutine(WaitFor(hitEnemies));
    }

    IEnumerator WaitFor(Collider2D[] hitEnemies)
    {
        float seconds = animator.GetCurrentAnimatorStateInfo(0).length / 2;
        // wait for half of the attack animation to damage player right when there will be hit animation.
        yield return new WaitForSeconds(seconds);

        // Array with names of the colliders that hitted Circle around Attack Point
        string[] enemyNames = new string[hitEnemies.Length];
        int i = 0;
        foreach (Collider2D enemy in hitEnemies)
        {
            // Same collider names is because Player contains 2 colliders, 
            // so to not damage twice per time we add names to array 
            // and then search if there are same name before damage player.
            if (Array.IndexOf(enemyNames, enemy.name) == -1)
            {
                enemy.gameObject.GetComponent<PlayerScript>().heartsHealthVisual.Test_Damage(atkDamageAmount);
                enemyNames[i] = enemy.name;
                i++;
            }
        }
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length
            + animator.GetCurrentAnimatorStateInfo(0).normalizedTime - seconds);
        if (!isDead())
            animator.Play(transform.name + "_Idle");
    }

    // Draws circle around attack point with same radius Attack Range.
    private void OnDrawGizmosSelected()
    {
        foreach (Transform child in attackPoints)
        {
            if (child == null) return;
            Gizmos.DrawWireSphere(child.position, attackRange);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            animator.Play(transform.name + "_Hurt");
            FindObjectOfType<AudioManager>().Play(transform.name + "_Hurt");
        }
    }

    public IEnumerator Die()
    {
        // Die Animation
        animator.Play(transform.name + "_Die");
        FindObjectOfType<AudioManager>().Play(transform.name + "_Die");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // disable enemy
        dissolve.SetDissolving(true);
        BoxCollider2D enemyCollider = GetComponent<BoxCollider2D>();
        enemyCollider.size = new Vector2(enemyCollider.size.x, 0);
        enemyCollider.offset = new Vector2(enemyCollider.offset.x, -0.35f);

    }

    private void Flip()
    {
        // Switch the way the enemy is labelled as facing.
        facingRight = !facingRight;

        // Multiply the enemy's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
