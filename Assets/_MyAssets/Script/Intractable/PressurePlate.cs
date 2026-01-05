using UnityEngine;

namespace MR.Game.Puzzle
{
    public class PressurePlate : MonoBehaviour
    {
        public bool IsPressed { get; private set; } = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Weight"))
            {
                IsPressed = true;
                Debug.Log($"{name} Pressed");
            }
        }

        /*private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Weight"))
            {
                IsPressed = false;
                Debug.Log($"{name} Released");
            }
        }*/
    }
}
