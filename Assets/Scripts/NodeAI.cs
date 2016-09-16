using UnityEngine;
using System.Collections;

public class NodeAI : MonoBehaviour {
    public AgentScript AS;
	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider hit)
    {
        if (hit.transform.GetComponent<NavMeshAgent>())
        {
            NavMeshAgent _agent = hit.transform.GetComponent<NavMeshAgent>();
            _agent.destination = AS.targets[Random.Range(0, AS.targets.Count)].transform.position;
        }
    }
}
