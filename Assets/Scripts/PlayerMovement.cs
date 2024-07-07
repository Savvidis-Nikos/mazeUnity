using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f; // Adjust movement speed as needed
    public float gravity = -9.81f; // Adjust gravity strength
    public float jumpHeight = 1f; // Adjust jump height

    public float shootingRange = 10f;
   
    public Transform shootingPoint;
    public LayerMask obstacleLayer;

    Vector3 velocity;
    bool isGrounded;
    [SerializeField]
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public float bulletSpeed = 20f;// Speed of the bullet
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (shootingPoint == null)
        {
            GameObject shootingPointObject = new GameObject("ShootingPoint");
            shootingPointObject.transform.SetParent(transform);
            shootingPointObject.transform.localPosition = new Vector3(0, 0, 0); // Adjust as needed
            shootingPoint = shootingPointObject.transform;
        }
    }

    void Update()
    {
        // Get user input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement vector based on input and speed
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime;

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Jump logic
        isGrounded = Physics.Raycast(transform.position, Vector3.down, controller.height / 2);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Adjust for a smoother landing
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Move the character controller with collision detection
        controller.Move(movement + velocity * Time.deltaTime);

        // Shooting mechanism
        if (Input.GetKeyDown(KeyCode.V))
        {
            Shoot1();
        }
    }

     public void Shoot1()
    {
        // Instantiate the bullet at the shooting point
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.speed = bulletSpeed;

        // Add forward force to the bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = shootingPoint.forward * bulletSpeed;
    }

}
