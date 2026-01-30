using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Laptop : Interactable
{
    public override void OnFocus()
    {
        GameManager.Instance.InteractionTextUIChange("Laptop");
    }

    public override void OnInteract()
    {
        print("Interacted with" + gameObject.name);
        GameManager.Instance.CursorShow(true);

    }

    public override void OnLoseFocus()
    {
        GameManager.Instance.InteractionTextUIChange("");
    }


    [SerializeField]
    public GameObject StudyPanel;
    public TMP_Text Exercise;

    public Button[] Answer;

    public void testButton()
    {
        print("hi");
    }

}
