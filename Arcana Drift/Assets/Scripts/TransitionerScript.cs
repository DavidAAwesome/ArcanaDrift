using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionerScript : MonoBehaviour
{
    public Scene targetScene;
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
            SwitchToTarget();
    }

    private void SwitchToTarget()
    {
        switch (targetScene)
        {
            case Scene.SampleScene:
                SceneManager.LoadScene("SampleScene");
                break;
            case Scene.RuinedArchives:
                SceneManager.LoadScene("RuinedArchives");
                break;
            case Scene.YouDidIt:
                SceneManager.LoadScene("You Did It!");
                break;
        }
    }

    public enum Scene
    {
        SampleScene,
        RuinedArchives,
        YouDidIt,
    }
}
