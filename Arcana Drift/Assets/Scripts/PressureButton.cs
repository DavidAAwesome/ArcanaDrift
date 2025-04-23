using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    public UnityEvent onPressed;
    public UnityEvent onReleased;
    public Animator animator;
    // public ButtonNumber buttonNumber;

    public GameObject[] objectsToAffect;

    private int objectsOnButton = 0;

    // public enum ButtonNumber
    // {
    //     button1,
    //     button2,
    //     button3,
    // }

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
        // return other.GetComponent<Rigidbody>() != null;//other.CompareTag("Player") || other.CompareTag("Box");
    }

    public void Pressed()
    {
        animator.SetBool("Pressed", true);
        // switch (buttonNumber)
        // {
        //     case ButtonNumber.button1:
        //         objectsToAffect[0].SetActive(false);
        //         break;
        //     case ButtonNumber.button2:
        //         break;
        //     case ButtonNumber.button3:
        //         break;
        // }
        
        Debug.Log("Pressed");
    }

    public void Released()
    {
        animator.SetBool("Pressed", false);
        // switch (buttonNumber)
        // {
        //     case ButtonNumber.button1:
        //         objectsToAffect[0].SetActive(true);
        //         break;
        //     case ButtonNumber.button2:
        //         break;
        //     case ButtonNumber.button3:
        //         break;
        // }
        Debug.Log("Released");
    }
}