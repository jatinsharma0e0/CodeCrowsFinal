using MR.Game.UI;
using UnityEngine;

namespace MR.Game.LevelManager
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Progression")]
        public int totalShardsThisLevel = 5;
        public int collectedShards = 0;
        public int yarn = 0;
        public int fishbones = 0;

        [Header("References")]
        public GameObject levelClearPortal;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddShards(int amount)
        {
            collectedShards += amount;  
            collectedShards = Mathf.Min(collectedShards, totalShardsThisLevel);
            UIManager.Instance.UpdateShardsUI(collectedShards, totalShardsThisLevel);

            if (collectedShards >= totalShardsThisLevel)
                UnlockPortal();
        }

        public void AddYarn(int a) { yarn += a; UIManager.Instance.UpdateYarnUI(yarn); }
        public void AddFishbones(int a) { fishbones += a; UIManager.Instance.UpdateFishboneUI(fishbones); }

        private void UnlockPortal()
        {
            if (levelClearPortal) levelClearPortal.SetActive(true);
            // Play fanfare etc.
        }

        public void ResetLevelProgress()
        {
            collectedShards = 0;
            yarn = 0;
            fishbones = 0;
        }
    }
}
