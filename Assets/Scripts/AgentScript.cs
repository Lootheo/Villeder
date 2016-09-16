using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AgentScript : MonoBehaviour {
    public GameObject _agentObject;
    public List<NavMeshAgent> agents;
    public List<NodeAI> targets;
	public Transform AreaSpawn;
	// Use this for initialization
	void Start () {
        targets = FindObjectsOfType<NodeAI>().ToList();
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].AS = this;
        }
        for (int x = 0; x < 10; x++)
        {
            CreateAgent();
        }
	}

    public void CreateAgent()
    {
        GameObject _newAgent = Instantiate(_agentObject, targets[Random.Range(0, targets.Count)].transform.position, Quaternion.identity) as GameObject;
		_newAgent.transform.SetParent (AreaSpawn);
        NavMeshAgent _nav = _newAgent.GetComponent<NavMeshAgent>();
        agents.Add(_nav);
        _nav.destination = targets[Random.Range(0, targets.Count)].transform.position;
    }
}
