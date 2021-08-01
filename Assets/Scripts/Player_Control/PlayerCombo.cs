/***
 * Combo Attack contloller.
 * Allows player to make combos if attack button was pressed in period timeBetweenAttacks.
 * Otherwise attack will start from default attack animation.
 * Also next attack could be only after previous attack finished.
 ***/

using System.Collections;
using UnityEngine;

public class PlayerCombo : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = .5f;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private byte attackAnimationCount;
    [SerializeField] private float timeBetweenAttacks = 0.3f;

    [SerializeField] private float cameraShakeIntensity;

    private byte attackIndex = 1;
    private float timer = 0;
    bool startTimer = false;

    private static string PLAYER_ATK = "Player_Attack";

    private void Update()
    {
        // start timer after first attack
        if (startTimer)
        {
            timer += Time.deltaTime;
        }

        // refresh timer and attack index if player don't attack in timeBetweenAttacks period
        if (timer >= timeBetweenAttacks)
        {
            startTimer = false;
            timer = 0;
            attackIndex = 1;
        }
    }

    public void Attack()
    {
        if (timer <= timeBetweenAttacks)
        {
            playerScript.isHighRankAnimation = true;
            InvokeAttack(PLAYER_ATK + attackIndex);
            timer = 0;

            if (attackIndex >= attackAnimationCount)
            {
                attackIndex = 1;
            }
            else
            {
                attackIndex++;
            }
        }
    }

    private void InvokeAttack(string playerAtk)
    {
        playerScript.ChangeAnimationState(playerAtk);
        playerScript.audioManager.Play(playerAtk);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        StartCoroutine(WaitFor(hitEnemies));
    }

    /// <summary>
    /// Wait for half of the animation, then take damage for all enemies
    /// in the attack range
    /// </summary>
    /// <param name="hitEnemies">All enemies that are in AttackRange stored here.</param>
    IEnumerator WaitFor(Collider2D[] hitEnemies)
    {
        float seconds = playerScript.animator.GetCurrentAnimatorStateInfo(0).length / 2;
        yield return new WaitForSeconds(seconds);

        CameraShake.Instance.ShakeCamera(cameraShakeIntensity, seconds);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null)
            {
                enemy.gameObject.GetComponent<EnemyContoller>().TakeDamage(1);
            }
        }

        yield return new WaitForSeconds(seconds);
        startTimer = true;
        playerScript.isHighRankAnimation = false;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Transform child in attackPoint)
        {
            if (child == null) return;
            Gizmos.DrawWireSphere(child.position, attackRange);
        }
    }
}
