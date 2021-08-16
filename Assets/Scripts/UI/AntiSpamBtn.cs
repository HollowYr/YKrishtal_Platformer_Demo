/***
 * Invoke event second time only when the requiredPressDelay have passed.
***/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class AntiSpamBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float requiredPressDelay = .5f;
    private float pointerDownTimer = 0;
    private bool pointerDown, pressed, isWaiting = true;

    public UnityEvent onPress;

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    void Update()
    {
        if (GameTime.Instance.isPaused) return;

        if (pointerDown && !pressed)
        {
            pressed = true;
        }
        // acrive only when button was pressed and/or timer is less then required press delay time
        if (pressed)
        {

            if (onPress != null && isWaiting)
            {
                // ivoke function onPressed btn without delay first time 
                onPress.Invoke();
                isWaiting = false;
            }

            // wait requiredPressDelay before next invoke
            pointerDownTimer += GameTime.Instance.deltaTime;
            if (pointerDownTimer >= requiredPressDelay)
            {
                // resetting timer after requiredPressDelay
                pressed = false;
                isWaiting = true;
                pointerDownTimer = 0;
            }
        }
    }
}
