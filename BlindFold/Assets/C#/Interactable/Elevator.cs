using UnityEngine;
using System.Collections;

public class Elevator : Interactable
{
    public override void OnFocus()
    {
        GameManager.Instance.InteractionTextUIChange("Use elevator");
    }

    public override void OnInteract()
    {
        if (!GameManager.Instance.HasAllFuses())
        {
            int missing = GameManager.Instance.totalFuses -
                          GameManager.Instance.GetCollectedFuses();

            StartCoroutine(
                GameManager.Instance.ShowTempMessage(
                    $"You need {missing} more fuses", 1f
                )
            );
        }
        else
        {
            GameManager.Instance.StartCutscene();
        }
    }

    public override void OnLoseFocus()
    {
        GameManager.Instance.InteractionTextUIChange("");
    }
}
