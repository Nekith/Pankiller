using UnityEngine;
using System.Collections;

public class ChefLogic : MonoBehaviour
{
    public float panAngle = 130f;
    public float panRange = 1f;
    public float panActiveLength = 1f;
    public float panCooldownLength = 1.5f;

    private SphereCollider meleeRange;
    private AudioSource audioSource;
    private float cooldown = 0f;
    private float activeTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        meleeRange = transform.FindChild("meleeRange").gameObject.GetComponent<SphereCollider>();
    }
    
    void Update()
    {
        if (cooldown <= 0f) {
            if (Input.GetButtonDown("Fire1")) {
                audioSource.PlayOneShot(Resources.Load("chef_pan") as AudioClip);
                meleeRange.enabled = true;
                cooldown = panCooldownLength;
                activeTimer = panActiveLength;
            }
        } else {
            cooldown -= Time.deltaTime;
            if (activeTimer > 0f) {
                activeTimer -= Time.deltaTime;
                if (activeTimer <= 0f) {
                    meleeRange.enabled = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy") {
            audioSource.PlayOneShot(Resources.Load("chef_pan_hit") as AudioClip);
            Vector3 force = transform.forward * 5f;
            force.y = 5f;
            col.gameObject.GetComponent<Enemy>().Hit(force);
        }
    }
}
