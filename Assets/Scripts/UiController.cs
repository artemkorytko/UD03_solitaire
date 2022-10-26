using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class UiController : MonoBehaviour
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
            ShowMain();
        }

        private void OnDestroy()
        {
            _gameController.OnWin -= OnWin;
        }

        private void OnWin()
        {
            mainPanel.SetActive(false);
            winPanel.SetActive(true);
        }

        public void ShowMain()
        {
            mainPanel.SetActive(true);
            winPanel.SetActive(false);
        }
    }
}