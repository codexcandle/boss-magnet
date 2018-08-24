using UnityEngine;

namespace Codebycandle.BossMagnet
{
    public class MinimapViewController:MonoBehaviour
    {
        public void ShowView(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}