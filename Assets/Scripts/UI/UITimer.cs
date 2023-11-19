using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UITimer : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private float time;
        void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            time = GameTimer.gameTimeInSeconds;
        }

        // Update is called once per frame
        void Update()
        {
            if(time <= 0) return;
            time -= Time.deltaTime;
            var min = (int)time / 60;
            var sec = ((int)(time - min*60)).ToString().PadLeft(2, '0');
            _text.SetText($"{min}:{sec}");
        }
    }
}