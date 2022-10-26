using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject winPanel;
    private GameController _gameController;

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        _gameController.OnWin += OnWin;
    }

    private void OnDestroy()
    {
        _gameController.OnWin -= OnWin;
    }

    private void OnWin()
    {
        winPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void ShowMain()
    {
        winPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}