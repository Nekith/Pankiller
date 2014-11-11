using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public bool raging = false;

    //private CharacterController characterController;
	private GameObject player;
    private NavMeshAgent agent;

	void Start()
	{
        //characterController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update()
    {
        Navigation();
    }

    void Navigation()
    {
        if (raging == false) {
            Vector3 distance = transform.position - player.transform.position;
            if (Mathf.Abs(distance.y) <= 5 && distance.magnitude <= 20f) {
                raging = true;
                agent.SetDestination(player.transform.position);
            }
        } else {
            agent.SetDestination(player.transform.position);
        }
	}
}
