using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    public void restart()
    {
        SceneManager.LoadScene("RuinedArchives");
    }
}
