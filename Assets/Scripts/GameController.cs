using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        private const int WIN_PLACE_VALUE = 78;

        [SerializeField] private CardPlace[] gameCardPlaces;
        private CardDeck _cardDeck;
        private PlayerController _playerController;
        private readonly Dictionary<CardType, int> mainPlaceInfo = new Dictionary<CardType, int>();

        public event Action OnWin;

        private void Awake()
        {
            _cardDeck = FindObjectOfType<CardDeck>();
            _playerController = GetComponent<PlayerController>();
        }

        private void Start()
        {
            GenerateField();
            _playerController.OnAddToMain += OnAddToMain;
            _playerController.OnRemoveFromMain += OnRemoveFromMain;
        }

        private void OnDestroy()
        {
            _playerController.OnAddToMain -= OnAddToMain;
            _playerController.OnRemoveFromMain -= OnRemoveFromMain;
        }

        public void Reset()
        {
            _cardDeck.Reset();
            _cardDeck.RandomizeDeck();
            FillGamePlaces();
            mainPlaceInfo.Clear();
        }

        private void GenerateField()
        {
            _cardDeck.Initialize();
            FillGamePlaces();
        }

        private void FillGamePlaces()
        {
            for (int i = 0; i < gameCardPlaces.Length; i++)
            {
                int counter = i;
                CardPlace prevCardPlace = gameCardPlaces[i];
                PlayingCard card = null;

                while (counter > 0)
                {
                    card = _cardDeck.GetCard();
                    card.SetParent(prevCardPlace);
                    prevCardPlace = card;
                    counter--;
                }

                card = _cardDeck.GetCard();
                card.SetParent(prevCardPlace);
                card.Open();
            }
        }

        private void OnRemoveFromMain(CardType type, int value)
        {
            if (mainPlaceInfo.ContainsKey(type))
            {
                mainPlaceInfo[type] -= value;
            }
        }

        private void OnAddToMain(CardType type, int value)
        {
            if (mainPlaceInfo.ContainsKey(type))
            {
                mainPlaceInfo[type] += value;
            }
            else
            {
                mainPlaceInfo.Add(type, value);
            }

            CheckMainPlaces();
        }

        private void CheckMainPlaces()
        {
            var keys = mainPlaceInfo.Keys;
            if (keys.Count < 4)
            {
                return;
            }

            foreach (var value in mainPlaceInfo.Values)
            {
                if (value != WIN_PLACE_VALUE)
                {
                    return;
                }
            }

            OnWin?.Invoke();
        }
    }
}