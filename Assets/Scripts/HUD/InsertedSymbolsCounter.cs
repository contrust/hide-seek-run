using TMPro;
using UnityEngine;

public class InsertedSymbolsCounter : MonoBehaviour
{
    public SymbolManager symbolManager;
    public TextMeshProUGUI textMesh;
    private MatchSettings matchSettings;
    private const string ValueTemplate = "Осталось ввести символов: {count}";
    private int count;

    private void Start()
    {
        matchSettings = FindObjectOfType<MatchSettings>();
        count = matchSettings.CountCorrectSymbolsToWin;
        textMesh.text = ValueTemplate.Replace("{count}", count.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTextIfValueChanged();
    }

    private void UpdateTextIfValueChanged()
    {
        if (count != matchSettings.CountCorrectSymbolsToWin - symbolManager.CurrentCorrectInsertions)
        {
            count = matchSettings.CountCorrectSymbolsToWin - symbolManager.CurrentCorrectInsertions;
            textMesh.text = ValueTemplate.Replace("{count}", count.ToString());
        }
    }
}
