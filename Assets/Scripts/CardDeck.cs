using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class CardDeck : MonoBehaviour
    {
        [SerializeField] private PlayingCard cardPrefab;
        [SerializeField] private CardStyleConfig config;
        [SerializeField] private Transform showContainer;

        private readonly List<PlayingCard> _cards = new List<PlayingCard>();
        private readonly List<PlayingCard> _allCard = new List<PlayingCard>();

        private int _currentShown = -1;
        private MeshRenderer _meshRenderer;

        public void Initialize()
        {
            _meshRenderer = GetComponent<MeshRenderer>();

            GenerateCards();
            RandomizeDeck();
        }

        private void GenerateCards()
        {
            GenerateType(config.Diamonds, CardType.Diamonds, CardColor.Red);
            GenerateType(config.Hearts, CardType.Hearts, CardColor.Red);
            GenerateType(config.Spades, CardType.Spades, CardColor.Black);
            GenerateType(config.Clubs, CardType.Clubs, CardColor.Black);
            _allCard.AddRange(_cards);
        }

        private void GenerateType(Material[] materials, CardType type, CardColor color)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                var newCard = Instantiate(cardPrefab, showContainer);
                newCard.Initialize(i, color, type, materials[i]);
                newCard.gameObject.SetActive(false);
                newCard.Close();
                newCard.IsInDeck = true;
                _cards.Add(newCard);
            }
        }

        private void RandomizeDeck()
        {
            List<PlayingCard> tempList = new List<PlayingCard>();
            while (_cards.Count > 0)
            {
                int rndIndex = Random.Range(0, _cards.Count);
                tempList.Add(_cards[rndIndex]);
                _cards.RemoveAt(rndIndex);
            }

            _cards.AddRange(tempList);
        }

        public PlayingCard GetCard()
        {
            if (_cards.Count == 0) return null;

            var card = _cards[0];
            _cards.RemoveAt(0);
            card.gameObject.SetActive(true);
            card.IsInDeck = false;
            return card;
        }
    }
}