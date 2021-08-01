/***
* Create OnTriggerEnter Event in Hierarchy.
* invoke it, when trigger enters.
***/

using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnter : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;

    public UnityEvent onTriggerEnter;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // get Player's RigidBody2D
        Rigidbody2D player = collider.GetComponent<Rigidbody2D>();

        //invoke event if player isn't null and alive.
        if (player != null && !playerScript.heartsHealthVisual.IsDead() && onTriggerEnter != null)
        {
            onTriggerEnter.Invoke();
        }
    }
}
