using UnityEngine;

public class RuinedArchivesManager : MonoBehaviour
{
    public bool button1pressed;
    public bool button2pressed;

    public GameObject door1;
    
    void Update()
    {
        if(button1pressed && button2pressed)
            door1.SetActive(false);
        else
            door1.SetActive(true);
    }
    
    public void Button1Pressed()
    {
        button1pressed = true;
    }

    public void Button1Released()
    {
        button1pressed = false;
    }

    public void Button2Pressed()
    {
        button2pressed = true;
    }

    public void Button2Released()
    {
        button2pressed = false;
    }
}
