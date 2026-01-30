using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    private Button playButton;
    private Button settingsButton;
    private Button quitButton;
    private Button CloseSettingsButton;
    private VisualElement settingsParent;



    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        playButton = root.Q<Button>("Play");
        settingsButton = root.Q<Button>("Settings");
        quitButton = root.Q<Button>("Quit");
        CloseSettingsButton = root.Q<Button>("Close");
        settingsParent = root.Q<VisualElement>("SettingsPanel");

        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
        CloseSettingsButton.RegisterCallback<ClickEvent>(OnCloseSettingsButtonClicked);
        settingsParent.style.display = DisplayStyle.None;
    }

    void OnPlayButtonClicked(ClickEvent clickEvent)
    {
        SceneManager.LoadScene("Map");
    }
    void OnSettingsButtonClicked(ClickEvent clickEvent)
    {
        settingsParent.style.display = DisplayStyle.Flex;
    }
    void OnQuitButtonClicked(ClickEvent clickEvent)
    {
        Application.Quit();
        print("you quited the game");
    }
    void OnCloseSettingsButtonClicked(ClickEvent clickEvent)
    {
        settingsParent.style.display = DisplayStyle.None;
    }
}
