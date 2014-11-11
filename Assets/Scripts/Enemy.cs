using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public bool raging = false;
    public int health = 2;

    [HideInInspector]
    public Vector3 addedForceDirection = Vector3.zero;
    [HideInInspector]
    public Vector3 addedForceRotation = Vector3.zero;
    [HideInInspector]
    public float addedForceDuration = 0f;

	private GameObject player;
    private NavMeshAgent agent;
    private AudioSource audioSource;

	void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
	}
	
	void FixedUpdate()
    {
        if (addedForceDuration <= 0f) {
            Navigation();
        } else {
            Force();
        }
    }

    void Navigation()
    {
        if (agent.enabled == true) {
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

    public void Hit(Vector3 force)
    {
        agent.enabled = false;
        addedForceDirection = force;
        addedForceDuration = 1f;
        if (health > 0) {
            health--;
            audioSource.PlayOneShot(Resources.Load("carot_hit") as AudioClip);
            addedForceRotation = Vector3.zero;
        } else if (health == 0) {
            audioSource.PlayOneShot(Resources.Load("carot_death") as AudioClip);
            addedForceRotation = -Vector3.right * 40f;
        }
    }

    void Force()
    {
        rigidbody.MovePosition(transform.position + addedForceDirection * Time.fixedDeltaTime);
        if (health <= 0) {
            transform.Rotate(addedForceRotation * Time.fixedDeltaTime);
        }
        addedForceDuration -= Time.fixedDeltaTime;
        if (addedForceDuration <= 0f) {
            addedForceDirection = Vector3.zero;
            addedForceRotation = Vector3.zero;
            addedForceDuration = 0f;
            if (health > 0) {
                agent.enabled = true;
            }
        }
    }
}
