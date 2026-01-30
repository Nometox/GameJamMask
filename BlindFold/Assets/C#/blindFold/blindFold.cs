using UnityEngine;
using Survive.Manager;

public class blindFold : MonoBehaviour
{
    private InputManager _inputManager;
    
    [SerializeField] private GameObject blindMan; 
    [SerializeField] private GameObject Blood;
    
    private bool _isPressed;

    void Start() => _inputManager = GameObject.FindAnyObjectByType<InputManager>();
    
    void Update()
    {
        if (_inputManager?.BlindFold == true && !_isPressed)
        {
            toggleBlindfold();
            toggleBlood();
            _isPressed = true;
        }
        else if (_inputManager?.BlindFold == false)
        {
            _isPressed = false;
        }
    }

    void toggleBlindfold()
    {
        blindMan.SetActive(!blindMan.activeSelf);
    } 

    void toggleBlood()
    {
        Blood.SetActive(!blindMan.activeSelf);
    }
}