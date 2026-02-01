using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{

    void Start()
    {
        CursorShow(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }

    public void CursorShow(bool On)
    {
        Cursor.lockState = On ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = On;
    }
}