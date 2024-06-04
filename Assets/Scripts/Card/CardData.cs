using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public string CardName;
    public string CardType;
    public string CardRace; 
    [TextArea(10, 10)]
    public string CardAbilty1;
    [TextArea(10, 10)]
    public string CardAbility2;
    public int CardCost;
    public int CardCost2;
    public int CardRank;
    public int CardAttack;
    public int CardDefense;
}
