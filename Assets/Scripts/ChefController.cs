using UnityEngine;
using System.Collections;

public class ChefController : MonoBehaviour
{
	public float movementSpeed = 10f;
	public float rotationSpeed = 20f;
	public float jumpSpeed = 10f;
	public float flyFactorInitial = 50f;
	public float flyFactorMod = 15f;
	public float airControl = 0.05f;
	public float addedForceControl = 0.02f;
	public float flyControl = 0.2f;
	public float diveFactor = 1.6f;
	public float minVelocityFall = -13f;
	
	[HideInInspector]
	public float xAxis;
	[HideInInspector]
	public float yAxis;
	[HideInInspector]
	public Vector3 direction;
	[HideInInspector]
	public bool jump;

	[HideInInspector]
	public Vector3 previousDirection = new Vector3(0f, 0f, 0f);
	[HideInInspector]
	public Vector3 speed = new Vector3(0f, 0f, 0f);
	[HideInInspector]
	public float verticalVelocity = 0;
	[HideInInspector]
	public int fly = 0;
	[HideInInspector]
	public float angleSlowFactor = 1f;
	[HideInInspector]
	public float wallSlowFactor = 1f;
	[HideInInspector]
	public bool isJumping = false;
	[HideInInspector]
	public bool isDiving = false;
	[HideInInspector]
	public Vector3 addedForceDirection = Vector3.zero;
	[HideInInspector]
	public float addedForceFactor = 0f;
	[HideInInspector]
	public float addedForceDuration = 0f;
	
	private CharacterController characterController;
	//private Animator animator;
	private ChefHud chefHud;
	//private AudioSource audioSource;
	
	void Start()
	{
		characterController = GetComponent<CharacterController>();
		//animator = GetComponent<Animator>();
		chefHud = GetComponent<ChefHud>();
		//audioSource = GetComponent<AudioSource>();
	}
	
	void Update()
	{
		GetMovementInput();
		View();
		Gravity();
		if (addedForceFactor == 0f) {
			Movement();
		} else {
			Force();
		}
		//animator.SetFloat("velocity", speed.magnitude);
	}
	
	void GetMovementInput()
	{
		xAxis = Input.GetAxis("Mouse Y");
		yAxis = Input.GetAxis("Mouse X");
		direction = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
		jump = Input.GetButton("Jump");
	}
	
	void View()
	{
		transform.Rotate(new Vector3(0f, yAxis, 0f) * rotationSpeed * Time.fixedDeltaTime);
		Vector3 angles = transform.FindChild("cameraMain").localEulerAngles;
		angles.x -= xAxis * rotationSpeed * Time.fixedDeltaTime;
		if (angles.x > 88 && angles.x < 271) {
			if (angles.x < 180) {
				angles.x = 88;
			} else {
				angles.x = 271;
			}
		}
		transform.FindChild("cameraMain").localEulerAngles = angles;
	}
	
	void Gravity()
	{
		if (verticalVelocity > minVelocityFall) {
			verticalVelocity += 2.0f * Physics.gravity.y * Time.fixedDeltaTime;
		}
	}
	
	void Movement()
	{
		Vector3 baseDirection = previousDirection;
		if (characterController.isGrounded == true) {
			if (jump == true) {
				//animator.SetBool("jumping", true);
				verticalVelocity = jumpSpeed;
				if (isDiving) {
					verticalVelocity *= diveFactor;
					isDiving = false;
				}
				if (direction != Vector3.zero) {
					fly++;
					baseDirection = new Vector3(Mathf.Lerp(baseDirection.x, direction.x, flyControl), 0f, Mathf.Lerp(baseDirection.z, direction.z, flyControl));
				}
				//audioSource.PlayOneShot(Resources.Load("jump") as AudioClip);
			} else {
				if (isJumping == true) {
					//audioSource.PlayOneShot(Resources.Load("land") as AudioClip);
					isJumping = false;
				}
				//animator.SetBool("jumping", false);
				fly = 0;
				//baseDirection = new Vector3(Mathf.Lerp(baseDirection.x, direction.x, 0.4f), 0f, Mathf.Lerp(baseDirection.z, direction.z, 0.4f));
				baseDirection = direction;
			}
		} else {
			isJumping = true;
			//animator.SetBool("jumping", true);
			baseDirection = new Vector3(Mathf.Lerp(baseDirection.x, direction.x, airControl), 0f, Mathf.Lerp(baseDirection.z, direction.z, airControl));
		}
		speed = baseDirection * movementSpeed;
		float factor = flyFactorInitial;
		for (int i = 0; i < fly; i++) {
			speed += baseDirection * movementSpeed * factor / 100;
			if (factor > flyFactorMod) {
				factor -= flyFactorMod;
			} else {
				break;
			}
		}
		float angle = Mathf.Abs(Vector3.Angle(previousDirection, direction));
		if (direction != Vector3.zero && angle > 90f) {
			angleSlowFactor = 1f - (Mathf.Clamp(angle - 90f, 0f, 120f) / 120f * 0.6f);
			speed *= angleSlowFactor;
		} else if (characterController.isGrounded == true && angleSlowFactor != 1f) {
			angleSlowFactor = Mathf.Lerp(angleSlowFactor, 1f, 0.005f);
			if (angleSlowFactor >= 0.99f) {
				angleSlowFactor = 1f;
			}
		}
		if (wallSlowFactor != 1f) {
			speed *= wallSlowFactor;
			wallSlowFactor = Mathf.Lerp(wallSlowFactor, 1f, 0.05f);
			if (wallSlowFactor >= 0.99f) {
				wallSlowFactor = 1f;
			}
		}
		Vector3 realSpeed = speed + new Vector3(0f, verticalVelocity, 0f);
		characterController.Move(realSpeed * Time.deltaTime);
		previousDirection = baseDirection;
	}
	
	void Force()
	{
		addedForceDirection = new Vector3(Mathf.Lerp(addedForceDirection.x, direction.x, addedForceControl), 0f, Mathf.Lerp(addedForceDirection.z, direction.z, addedForceControl));
		characterController.Move((addedForceDirection * addedForceFactor + new Vector3(0f, verticalVelocity, 0f)) * Time.deltaTime);
		addedForceDuration -= Time.deltaTime;
		previousDirection = addedForceDirection;
		if (addedForceDuration <= 0f) {
			addedForceDirection = Vector3.zero;
			addedForceFactor = 0f;
			addedForceDuration = 0f;
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.moveDirection.y > -0.5f) {
			wallSlowFactor = 0.7f;
		} else if (hit.normal.y <= 0.9f && isDiving == false) {
			if (previousDirection.magnitude >= 0.5f) {
				if (Mathf.Abs(hit.normal.x) >= 0.4f && Mathf.Abs(hit.normal.x) <= 0.6f && Mathf.Abs(previousDirection.x) >= 0.4f) {
					if ((previousDirection.x > 0f && hit.normal.x < 0f) || (previousDirection.x < 0f && hit.normal.x > 0f)) {
						isDiving = true;
					}
				} else if (Mathf.Abs(hit.normal.z) >= 0.4f && Mathf.Abs(hit.normal.z) <= 0.6f && Mathf.Abs(previousDirection.z) >= 0.4f) {
					if ((previousDirection.z > 0f && hit.normal.z < 0f) || (previousDirection.z < 0f && hit.normal.z > 0f)) {
						isDiving = true;
					}
				}
			}
		} else {
			isDiving = false;
		}
	}
}
