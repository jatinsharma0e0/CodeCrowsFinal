using UnityEngine;

namespace MR.Game.Player
{
    public class PlayerKeyHolder : MonoBehaviour
    {
        public int keyCount = 0;

        public void AddKey()
        {
            keyCount++;
        }

        public bool UseKey()
        {
            if (keyCount > 0)
            {
                keyCount--;
                return true;
            }
            return false;
        }
    }
}
