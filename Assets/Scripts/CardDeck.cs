using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private PlayingCard cardPrefab;
    [SerializeField] private CardStyleConfig config;
    [SerializeField] private Transform showContainer;

    private readonly List<PlayingCard> _cards = new List<PlayingCard>();
    private readonly List<PlayingCard> _allCards = new List<PlayingCard>();

    private int _currentShown = -1;
    private MeshRenderer _meshRenderer;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _playerController.OnExcludeCurrentCard += ExcludeCurrentCard;
    }

    private void OnDestroy()
    {
        _playerController.OnExcludeCurrentCard -= ExcludeCurrentCard;
    }

    public void Reset()
    {
        _cards.Clear();

        for (int i = 0; i < _allCards.Count; i++)
        {
            _allCards[i].transform.SetParent(null);
            _allCards[i].Reset();
            _allCards[i].IsInDeck = true;
            _allCards[i].gameObject.SetActive(false);
        }

        _cards.AddRange(_allCards);
    }

    private void ExcludeCurrentCard()
    {
        _cards[_currentShown].IsInDeck = false;
        _cards.RemoveAt(_currentShown);

        if (_currentShown + 1 < _cards.Count)
        {
            _currentShown++;
        }
        else
        {
            _meshRenderer.enabled = true;
            _currentShown = -1;
        }
    }

    public void Initialize()
    {
        GenerateCards();
        RandomizeDeck();
    }
    
    private void GenerateCards()
    {
        GenerateType(config.Diamonds,CardType.Diamonds,CardColor.Red);
        GenerateType(config.Hearts,CardType.Hearts,CardColor.Red);
        GenerateType(config.Spades,CardType.Spades,CardColor.Black);
        GenerateType(config.Clubs,CardType.Clubs,CardColor.Black);
        _allCards.AddRange(_cards);
    }

    private void GenerateType(Material[] materials, CardType type, CardColor color)
    {
        for (int i = 0; i < materials.Length; i++)
        {
           var newCard= Instantiate(cardPrefab, showContainer);
           newCard.Initialize(i,color,type,materials[i]);
           newCard.Close();
           newCard.IsInDeck = true;
           _cards.Add(newCard);
           newCard.gameObject.SetActive(false);
        }
    }
    
    public void RandomizeDeck()
    {
        List<PlayingCard> tempList = new List<PlayingCard>();
        while (_cards.Count>0)
        {
            int randomIndex = Random.Range(0, _cards.Count);
            tempList.Add(_cards[randomIndex]);
            _cards.RemoveAt(randomIndex);
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
    
    private void OnMouseUpAsButton()
    {
        ShowNext();
    }

    private void ShowNext()
    {
        if (_currentShown >= 0)
        {
            _cards[_currentShown].gameObject.SetActive(false);
            _cards[_currentShown].Close();
        }
        
        _currentShown++;
        
        if (_currentShown == _cards.Count - 1 && _meshRenderer.enabled)
        {
            _meshRenderer.enabled = false;
            _cards[_currentShown].gameObject.SetActive(true);
            _cards[_currentShown].Open();
            return;
        }

        if (_currentShown >= _cards.Count)
        {
            _currentShown = -1;
            _meshRenderer.enabled = true;
        }
        else
        {
            _cards[_currentShown].gameObject.SetActive(true);
            _cards[_currentShown].Open();
        }
    }
}
