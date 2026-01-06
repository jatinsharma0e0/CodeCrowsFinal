using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
    void Start()
    {
        GetComponent<NavMeshAgent>()
            .SetDestination(transform.position + transform.forward * 5f);
    }
}
