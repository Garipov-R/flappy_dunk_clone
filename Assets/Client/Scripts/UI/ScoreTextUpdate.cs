using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Client.Game;

namespace Client.UI
{
    public class ScoreTextUpdate : MonoBehaviour
    {
        private Text _Text;


        private void Awake()
        {
            _Text = GetComponent<Text>();
            Score.OnChanged += UpdateText;
        }

        private void Start()
        {
            UpdateText(Score.ScoreCount);
        }

        private void OnDestroy()
        {
            Score.OnChanged -= UpdateText;
        }

        private void UpdateText(int score)
        {
            if (_Text == null)
                return;

            _Text.text = score.ToString();
        }
    }
}