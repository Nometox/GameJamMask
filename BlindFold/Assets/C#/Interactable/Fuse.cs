using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Fuse : Interactable
{
    public override void OnFocus()
    {
        GameManager.Instance.InteractionTextUIChange("Fuse");
    }

    public override void OnInteract()
    {
        print("Interacted with" + gameObject.name);
        GameManager.Instance.CollectFuse();
        Destroy(gameObject);
    }

    public override void OnLoseFocus()
    {
        GameManager.Instance.InteractionTextUIChange("");
    }


}
