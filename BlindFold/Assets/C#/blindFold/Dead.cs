using UnityEngine;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    public GameObject bloodObject;

    private float timer = 0f;
    [SerializeField] private float maxTime = 2f;

    void Update()
    {
        if (GameManager.Instance.IsCutscenePlaying) return;

        if (bloodObject != null && bloodObject.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer >= maxTime)
            {
                GameOver();
            }
        }
        else
        {
            timer = 0f;
        }
    }

    void GameOver()
    {
        Debug.Log("Game Verloren! Het bloed was te lang actief.");

        HorrorManager.Instance.PlayRandomScare();

    }
}