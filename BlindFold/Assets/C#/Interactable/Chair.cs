using Microsoft.VisualBasic;
using UnityEngine;

public class Chair : Interactable
{
    public override void OnFocus()
    {
        GameManager.Instance.InteractionTextUIChange("Chair");
    }

    public override void OnInteract()
    {
        print("Interacted with" + gameObject.name);

    }

    public override void OnLoseFocus()
    {
        GameManager.Instance.InteractionTextUIChange("");
    }
}
