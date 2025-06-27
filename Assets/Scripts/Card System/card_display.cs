using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void Setup(CardData card)
    {
        nameText.text = card.cardName;
        descriptionText.text = card.description;
    }
}

