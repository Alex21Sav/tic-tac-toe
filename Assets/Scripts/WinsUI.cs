using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class WinsUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject _uiCanvas;
    [SerializeField] private TMP_Text _uiWinnerText;
    [SerializeField] private Button _uiRestartButton;

    [Header("Boord Reference")]
    [SerializeField] private Board _board;

    private void Start()
    {
        _uiRestartButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        _board.OnWinAction += OnWinEvent;
    }
    private void OnWinEvent(Mark mark, Color color)
    {
        _uiWinnerText.text =(mark == Mark.None) ? "Nobody Wins" : mark.ToString() + "Wins";
        _uiWinnerText.color = color;

        _uiCanvas.SetActive(true);
    }
    private void OnDestroy()
    {
        _uiRestartButton.onClick.RemoveAllListeners();
        _board.OnWinAction -= OnWinEvent;
    }
}
