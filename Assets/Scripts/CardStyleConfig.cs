using UnityEngine;

[CreateAssetMenu(fileName = "CardStyleConfig", menuName = "Configs/CardStyleConfig", order = 0)]
public class CardStyleConfig : ScriptableObject
{
    [SerializeField] private Material[] diamonds;
    [SerializeField] private Material[] hearts;
    [SerializeField] private Material[] clubs;
    [SerializeField] private Material[] spades;

    public Material[] Diamonds => diamonds;

    public Material[] Hearts => hearts;

    public Material[] Clubs => clubs;

    public Material[] Spades => spades;
}