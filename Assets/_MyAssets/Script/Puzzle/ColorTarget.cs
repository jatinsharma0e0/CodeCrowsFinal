using UnityEngine;

namespace MR.Game.Puzzle
{
    public class ColorTarget : MonoBehaviour
    {
        public string colorName;
        public bool isActive = false;
        public float openHeight = 3f;
        public float moveSpeed = 2f;
        public Color color = Color.white; // Assign in Inspector
        public Renderer targetRenderer;   // Assign in Inspector

        private Vector3 closedPosition;
        private Vector3 openPosition;
        private Coroutine moveCoroutine;

        private void Start()
        {
            closedPosition = transform.position;
            openPosition = closedPosition + Vector3.up * openHeight;
            targetRenderer = GetComponent<Renderer>();

            // Set the material color
            if (targetRenderer != null)
            {
                // Make sure we're not editing a shared material unless that's intended
                targetRenderer.material = new Material(targetRenderer.material)
                {
                    color = color
                };
            }
            else
            {
                Debug.LogWarning("ColorTarget: targetRenderer not assigned!", this);
            }
        }

        public void OpenTarget()
        {
            if (!isActive)
            {
                isActive = true;
                MoveTarget(openPosition);
            }
        }

        public void CloseTarget()
        {
            if (isActive)
            {
                isActive = false;
                MoveTarget(closedPosition);
            }
        }

        public void ToggleTarget()
        {
            if (isActive)
                CloseTarget();
            else
                OpenTarget();
        }

        private void MoveTarget(Vector3 target)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveToPosition(target));
        }

        private System.Collections.IEnumerator MoveToPosition(Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = target;
        }

        public bool IsActive() => isActive;
    }
}
