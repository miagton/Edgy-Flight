
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float moveThrust = 100f;
    [SerializeField] float rotationThrust=100f;

    AudioSource audioSource;
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        ThrustInput();
        Rotate();
        
    }

    

    

     void ThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Thrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void Thrust()
    {
        float thrustThisFrame = moveThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * moveThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.Play();

        }
    }

    public void Rotate()
    {
        rigidBody.freezeRotation = true;//before we take manual control on rotation
        float rotationThisFrame = rotationThrust * Time.deltaTime;
      
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;//enabling physics after input
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Its our friend!");
                break;
            default:
                print("Ur dead!");
                break;
        }
    }

}
