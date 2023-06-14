using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class VictoryUIScreen: UIScreen
    {
        [SerializeField] private TextMeshProUGUI hunterWonText;
        [SerializeField] private TextMeshProUGUI victimsWonText;
        
        public void SetWinner(Winner winner)
        {
            if (winner == Winner.Hunter)
            {
                victimsWonText.gameObject.SetActive(false);
                hunterWonText.gameObject.SetActive(true);
            }
            else
            {
                hunterWonText.gameObject.SetActive(false);
                victimsWonText.gameObject.SetActive(true);
            }
        }
    }

    public enum Winner
    {
        Hunter,
        Victims
    }
}