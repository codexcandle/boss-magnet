using UnityEngine;
using UnityEngine.UI;

namespace Codebycandle.BossMagnet
{
    public class PromptController:MonoBehaviour
    {
        private Text tField;

        void Awake()
        {
            tField = GetComponentInChildren<Text>();
        }

        void Start()
        {
            SetText();
        }

        public void SetText(string text = "")
        {
            tField.text = text;
        }
    }
}