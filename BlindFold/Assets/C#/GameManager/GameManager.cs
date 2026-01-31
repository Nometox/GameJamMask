using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public TMP_Text InteractionText;
    public TMP_Text ObjectiveText;

    [Header("Fuses")]
    public int totalFuses = 3;
    private int collectedFuses = 0;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deniedSound;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CursorShow(false);
        UpdateObjective();
    }


    public void CollectFuse()
    {
        collectedFuses++;
        InteractionTextUIChange("");
        UpdateObjective();
    }

    public bool HasAllFuses()
    {
        return collectedFuses >= totalFuses;
    }

    public int GetCollectedFuses()
    {
        return collectedFuses;
    }

    void UpdateObjective()
    {
        if (collectedFuses < totalFuses)
        {
            ObjectiveText.SetText(
                $"Find fuses"
            );
        }
        else
        {
            ObjectiveText.SetText("Escape using the elevator");
        }
    }


    public void InteractionTextUIChange(string newText)
    {
        InteractionText.SetText(newText);
    }

    public IEnumerator ShowTempMessage(string msg, float time)
    {
        audioSource.PlayOneShot(deniedSound);
        InteractionTextUIChange(msg);
        yield return new WaitForSeconds(time);
        InteractionTextUIChange("");
    }


    public void StartCutscene()
    {
        SceneManager.LoadScene("Game");
    }


    public void CursorShow(bool On)
    {
        Cursor.lockState = On ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = On;
    }
}
