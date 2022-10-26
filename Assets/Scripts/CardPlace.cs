using UnityEngine;

namespace DefaultNamespace
{
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

        public bool IsCanConnect(PlayingCard playingCard)
        {
            if (!IsOpen)
                return false;

            if (playingCard.Value != nextCardValue && nextCardValue != -1)
            {
                return false;
            }

            if (nextCardColor != CardColor.Any && playingCard.Color != nextCardColor)
            {
                return false;
            }

            if (nextCardType != CardType.Any && playingCard.Type != nextCardType)
            {
                return false;
            }

            Vector3 position = playingCard.CardContainer.transform.localPosition;
            position.z = isMain ? 0f : onGameZOffset;
            playingCard.CardContainer.localPosition = position;
            return true;
        }
    }
}