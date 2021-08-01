/***
 * Knockback in opposite direction
 * and hurt when trigger is entered
 ***/
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    [SerializeField] private HeartsHealthVisual heartsHealthVisual;
    [SerializeField] private Vector2 knockbackDistance, velocityMax, minVelocity;
    [SerializeField] private int damageAmount;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // get Player's RigidBody2D
        Rigidbody2D player = collider.GetComponent<Rigidbody2D>();
        if (player != null && !heartsHealthVisual.IsDead())
        {
            //Damage and knockback player
            DamageKnockback(player, knockbackDistance, damageAmount);
        }
    }

    private void DamageKnockback(Rigidbody2D rb, Vector2 knockbackDistance, int damageAmount)
    {
        Vector2 velocity = rb.velocity;

        if (Mathf.Abs(velocity.y) > velocityMax.y)
        {
            velocity.y = velocityMax.y;
        }
        else if (Mathf.Abs(velocity.y) < minVelocity.y)
        {
            velocity.y = minVelocity.y;
        }

        if (Mathf.Abs(velocity.x) < minVelocity.x)
        {
            if (velocity.x > 0)
            {
                velocity.x = minVelocity.x;
            }
            else
            {
                velocity.x = minVelocity.x * -1;
            }
        }
        else if (Mathf.Abs(velocity.x) > minVelocity.x)
        {
            if (velocity.x > 0)
            {
                velocity.x = velocityMax.x;
            }
            else
            {
                velocity.x = velocityMax.x * -1;
            }
        }
        // add opposite forse to x and same to y as player movement direction;
        rb.AddForce(new Vector2(-velocity.x * knockbackDistance.x, Mathf.Abs(velocity.y) * knockbackDistance.y));
        HeartsHealthVisual.heartHealthSystemStatic.Damage(damageAmount);
    }
}
