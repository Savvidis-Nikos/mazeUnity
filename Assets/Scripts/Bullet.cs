using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 1;// Adjust bullet speed as needed

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CharacterController controller = player.GetComponent<CharacterController>();
        Vector3 firingDirection = controller.velocity.normalized;

        // Get Rigidbody and apply velocity
        Rigidbody rb = GetComponent<Rigidbody>();
        
        
            rb.velocity = firingDirection * speed;
       
        
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Example: Destroy the obstacle
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Obstacle") || (collision.gameObject.CompareTag("Wall"))) { 
                // Destroy the bullet as well
                Destroy(gameObject);
        }
    }
}
