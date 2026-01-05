using UnityEngine;
using UnityEngine.UI;

namespace MR.Game.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public Text shardText;
        public Text yarnText;
        public Text fishText;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void UpdateShardsUI(int have, int total)
        {
            if (shardText) shardText.text = $"Shards: {have}/{total}";
        }

        public void UpdateYarnUI(int amount)
        {
            if (yarnText) yarnText.text = $"Yarn: {amount}";
        }

        public void UpdateFishboneUI(int amount)
        {
            if (fishText) fishText.text = $"Fishbones: {amount}";
        }
    }
}
