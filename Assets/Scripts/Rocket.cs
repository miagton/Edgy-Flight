
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float speed = 28f;

    AudioSource audioSource;
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * speed);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();

            }
        }
        else 
        {
            audioSource.Stop();
        }
       // else audioSource.Stop();
        
        if (Input.GetAxis("Horizontal") != 0)
        {
            float input = Input.GetAxis("Horizontal");
            ShipRotation(input);
        }
        
    }

    void ShipRotation(float multiplier)
    {
        transform.Rotate(-Vector3.forward*multiplier);
    }
}
