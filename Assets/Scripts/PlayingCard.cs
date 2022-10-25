using UnityEngine;

public class PlayingCard : CardPlace
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform openCardContainer;
    [SerializeField] private Transform closeCardContainer;

    public int Value { get; private set; }
    public CardColor Color { get; private set; }
    public CardType Type { get; private set; }
    public CardPlace CurrentPlace { get; private set; }
    public bool IsInDeck { get; set; }

    public void Initialize(int value, CardColor color,CardType type, Material material)
    {
        Value = value;
        nextCardValue = value - 1;
        Color = color;
        nextCardColor = color == CardColor.Red ? CardColor.Black : CardColor.Red;
        Type = type;
        nextCardType = CardType.Any;
        meshRenderer.material = material;
    }

    public void Open()
    {
        if(IsOpen) return;

        IsOpen = true;
        cardContainer = openCardContainer;
        transform.Rotate(Vector3.forward * 180, Space.Self);
    }
    
    public void Close()
    {
        if(!IsOpen) return;

        IsOpen = false;
        cardContainer = closeCardContainer;
        transform.Rotate(Vector3.forward * -180, Space.Self);
    }

    public void SetParent(CardPlace parent = null)
    {
        if (parent == null)
        {
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.SetParent(parent.CardContainer);
            transform.localPosition = Vector3.zero;

            SetAtMain(parent.IsMain);
            
            if (CurrentPlace is PlayingCard card)
            {
                card.Open();
            }

            CurrentPlace = parent;
        }
    }

    private void SetAtMain(bool state)
    {
        if (state)
        {
            nextCardColor = Color;
            nextCardType = Type;
            nextCardValue = Value + 1;
        }
        else
        {
            nextCardColor = Color == CardColor.Red ? CardColor.Black : CardColor.Red;
            nextCardType = CardType.Any;
            nextCardValue = Value - 1;
        }

        isMain = state;
    }
}
