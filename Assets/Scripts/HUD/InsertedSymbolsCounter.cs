using Symbols;
using TMPro;
using UnityEngine;

public class InsertedSymbolsCounter : MonoBehaviour
{
    public SymbolManager symbolManager;
    public TextMeshProUGUI textMesh;
    private const string ValueTemplate = "Введено символов: {count}";
    private int count = -1;

    // Update is called once per frame
    void Update()
    {
        UpdateTextIfValueChanged();
    }

    private void UpdateTextIfValueChanged()
    {
        if (count != symbolManager.CurrentCorrectInsertions)
        {
            count = symbolManager.CurrentCorrectInsertions;
            textMesh.text = ValueTemplate.Replace("{count}", count.ToString());
        }
    }
}
