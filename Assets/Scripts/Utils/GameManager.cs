using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private PlayerInput playerInput;

    private float timer = 0f;


    #region Unity Methods

    private void OnEnable()
    {
        inputReader.OnKeyRPress += RestartGame;
    }

    private void OnDisable()
    {
        inputReader.OnKeyRPress -= RestartGame;
    }

    private void RestartGame()
    {
        Time.timeScale = 0;

        inputReader.OnKeyRPress -= RestartGame;


        SceneManager.LoadScene(0);

    }


    #endregion
}
