using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public bool raging = false;
    public float minVelocityFall = -13f;

    //private CharacterController characterController;
	private GameObject player;
    private NavMeshAgent agent;
    private float verticalVelocity = 0;

	void Start()
	{
        //characterController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update()
    {
        Gravity();
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

    void Gravity()
    {
        if (verticalVelocity > minVelocityFall) {
            verticalVelocity += 2.0f * Physics.gravity.y * Time.fixedDeltaTime;
        }
        Vector3 realSpeed = new Vector3(0f, verticalVelocity, 0f);
        //characterController.Move(realSpeed * Time.deltaTime);
    }
}
