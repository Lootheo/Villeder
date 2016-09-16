using UnityEngine;
using System.Collections;

public class navmesh : MonoBehaviour {

	// Use this for initialization
	private NavMeshAgent _agente;
	public Transform _destino;

	void Start () {
		_agente = this.gameObject.GetComponent<NavMeshAgent>();
		_agente.destination = _destino.position;
	}

	void Update () {}

}
