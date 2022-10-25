using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private CardPlace[] gameCardPlaces;
        private CardDeck _cardDeck;

        private void Awake()
        {
            _cardDeck = FindObjectOfType<CardDeck>();
        }

        private void Start()
        {
            GenerateField();
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
    }
}