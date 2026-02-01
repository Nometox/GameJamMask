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
    public Camera cutsceneCam;
    public Camera playerCam;

    public bool IsCutscenePlaying { get; private set; }

    [Header("Fuses")]
    public int totalFuses = 3;
    private int collectedFuses = 0;

    [Header("Audio")]
    public AudioSource audioSource;
    public bool PlayerMoving { get; private set; }
    public AudioClip deniedSound;

    [Header("objectiveText")]
    public float ObjectiveTextTime = 3f;

    [Header("Inventory UI")]
    public GameObject fuseIconPrefab; 
    public Transform inventoryParent; 

    public bool IsBlindfolded { get; private set; }

    public void SetBlindfold(bool value)
    {
        IsBlindfolded = value;
    }

    public void SetPlayerMoving(bool value)
    {
        PlayerMoving = value;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateObjective();
    }

    public void CollectFuse()
    {
        collectedFuses++;
        Instantiate(fuseIconPrefab, inventoryParent);
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
        StopCoroutine("TempObjectiveRoutine");

        if (collectedFuses < totalFuses)
        {
            StartCoroutine(TempObjectiveRoutine("Find fuses", ObjectiveTextTime));
        }
        else
        {
            ObjectiveText.SetText("Escape using the elevator");
        }
    }

    IEnumerator TempObjectiveRoutine(string msg, float time)
    {
        ObjectiveText.SetText(msg);
        yield return new WaitForSeconds(time);
        ObjectiveText.SetText("");
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
        StartCoroutine(CutsceneRoutine());
    }

    IEnumerator CutsceneRoutine()
    {
        IsCutscenePlaying = true;

        RandomEventManager.Instance.enabled = false;
        HorrorManager.Instance.enabled = false;

        playerCam.gameObject.SetActive(false);
        cutsceneCam.gameObject.SetActive(true);

        ObjectiveText.SetText("You feel something watching you...");

        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene("GameWon");
    }
}