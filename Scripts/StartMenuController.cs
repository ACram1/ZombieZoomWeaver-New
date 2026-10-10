using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    //Starts game on Street Scene
    public void OnStartClick()
    {
        SceneManager.LoadScene(0);
    }

    //Quits the program
    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    //Opens Controls
    public void OnControlsClick()
    {
        SceneManager.LoadScene(3);
    }
    
    //Goes back to Start menu
    public void OnBackClick()
    {
        SceneManager.LoadScene(2);
    }

    //Restarts at Street Scene after death
    public void OnRestartClick()
    {
        SceneManager.LoadScene(0);
    }

    //Goes back to Start menu if you quit after dying
    public void Quit()
    {
        SceneManager.LoadScene(2);
    }

    //Goes back to Start menu when you click Return on the Escaped page
    public void Return()
    {
        SceneManager.LoadScene(2);
    }
}
