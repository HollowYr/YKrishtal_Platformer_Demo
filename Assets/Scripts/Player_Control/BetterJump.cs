/**
 * This class changes gravity.y when player jumping up and falling down with different numbers.
 * When jumping up and button held gravity is normal, then when falling it changes. 
 * If button was released at jumping up other scale will be added to gravity, 
 * so player will immediately fall down.
 **/

using UnityEngine.EventSystems;
using UnityEngine;

public class BetterJump : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float requiredHoldTime = .1f;
    [SerializeField] private Rigidbody2D rb;

    private bool pointerDown;
    private float pointerDownTimer;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDownTimer = 0;
        pointerDown = false;
    }


    void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime)
            {
                // Rb is falling down
                if (rb.velocity.y < 0)
                {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }

            }
        }
        // if Rb is just jumping up
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        }
    }
}
