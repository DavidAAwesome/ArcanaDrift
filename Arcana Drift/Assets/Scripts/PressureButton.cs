using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    public UnityEvent onPressed;
    public UnityEvent onReleased;
    public Animator animator;

    private int objectsOnButton = 0;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something Entered");
        if (IsPressingObject(other))
        {
            Debug.Log("Being Pressed");
            objectsOnButton++;
            if (objectsOnButton == 1)
            {
                onPressed.Invoke();
                // Optional: Animate button press
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPressingObject(other))
        {
            objectsOnButton--;
            if (objectsOnButton <= 0)
            {
                onReleased.Invoke();
                // Optional: Animate button release
            }
        }
    }

    bool IsPressingObject(Collider other)
    {
        return other.CompareTag("Player") || other.CompareTag("Box");
    }

    public void Pressed()
    {
        animator.SetBool("Pressed", true);
        Debug.Log("Pressed");
    }

    public void Released()
    {
        animator.SetBool("Pressed", false);
        Debug.Log("Released");
    }
}