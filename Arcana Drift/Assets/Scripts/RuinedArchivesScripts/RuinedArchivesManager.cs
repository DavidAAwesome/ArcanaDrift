using UnityEngine;

public class RuinedArchivesManager : MonoBehaviour
{
    public bool button1pressed;
    public bool button2pressed;
    public bool button3pressed;
    public bool button4pressed;
    public bool button5pressed;

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door4;
    
    void Update()
    {
        if(button1pressed && button2pressed)
            door1.SetActive(false);
        else
            door1.SetActive(true);
        
        if(button3pressed)
            door2.SetActive(false);
        else
            door2.SetActive(true);
        
        if(button4pressed)
            door3.SetActive(false);
        else
            door3.SetActive(true);
        
        if(button5pressed)
            door4.SetActive(false);
        else
            door4.SetActive(true);
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
    
    public void Button3Pressed()
    {
        button3pressed = true;
    }

    public void Button3Released()
    {
        button3pressed = false;
    }

    public void Button4Pressed()
    {
        button4pressed = true;
    }

    public void Button4Released()
    {
        button4pressed = false;
    }
    
    public void Button5Pressed()
    {
        button5pressed = true;
    }

    public void Button5Released()
    {
        button5pressed = false;
    }
    
    
}
