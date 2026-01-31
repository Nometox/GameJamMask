using UnityEngine;
using Survive.Manager;
using System.Collections;

public class blindFold : MonoBehaviour
{
    private InputManager _inputManager;
    public Animator animator;
    private bool _isAnimating;

    public bool isBlindfolded = false;

    [SerializeField] private GameObject blindMan;
    [SerializeField] private GameObject Blood;

    private bool _isPressed;

    void Start()
    {
        _inputManager = GameObject.FindAnyObjectByType<InputManager>();
        animator.SetBool("isBlindfolded", isBlindfolded);
        Blood.SetActive(isBlindfolded);
    }

    void Update()
    {
        if (_isAnimating) return;

        if (_inputManager?.BlindFold == true && !_isPressed)
        {
            StartCoroutine(PlayBlindfoldAnimation());
            _isPressed = true;
        }
        else if (_inputManager?.BlindFold == false)
        {
            _isPressed = false;
        }
    }

    IEnumerator PlayBlindfoldAnimation()
    {
        _isAnimating = true;

        toggleBlindfold();
        toggleBlood();

        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length
        );

        _isAnimating = false;
    }
    void toggleBlindfold()
    {
        isBlindfolded = !isBlindfolded;
        animator.SetBool("isBlindfolded", isBlindfolded);
    }

    void toggleBlood()
    {
        Blood.SetActive(isBlindfolded);
    }
}