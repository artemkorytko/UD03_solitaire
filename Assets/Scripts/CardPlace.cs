using UnityEngine;

public class CardPlace : MonoBehaviour
{
    [SerializeField] protected bool isMain;
    [SerializeField] protected float onGameZOffset = -2f;

    [SerializeField] protected int nextCardValue;
    [SerializeField] protected CardColor nextCardColor;
    [SerializeField] protected CardType nextCardType;

    [SerializeField] protected Transform cardContainer;

    public bool IsOpen { get; protected set; } = true;

    public bool IsMain => isMain;

    protected float OnGameZOffset => onGameZOffset;

    protected int NextCardValue => nextCardValue;

    protected CardColor NextCardColor => nextCardColor;

    protected CardType NextCardType => nextCardType;

    public Transform CardContainer => cardContainer;


    public bool IsCanConnect(PlayingCard card)
    {
        if (!IsOpen) return false;
        if (card.Value != nextCardValue && nextCardValue != -1) return false;
        if (nextCardColor != CardColor.Any && card.Color != nextCardColor) return false;
        if (nextCardType != CardType.Any && card.Type != nextCardType) return false;

        Vector3 position = card.CardContainer.transform.position;
        position.z = isMain ? 0f : onGameZOffset;
        card.CardContainer.localPosition = position;
        return true;
    }
}
