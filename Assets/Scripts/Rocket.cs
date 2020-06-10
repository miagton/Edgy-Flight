
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //moving values
    [SerializeField] float moveThrust = 100f;
    [SerializeField] float rotationThrust=100f;

    [SerializeField] float lvlLoadDelay = 2f;

    //audioclips
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explode;
    [SerializeField] AudioClip winSound;
    //patricle systems
    [SerializeField] ParticleSystem engineTrail;
    [SerializeField] ParticleSystem deathBlow;
    [SerializeField] ParticleSystem winParticles;

    AudioSource audioSource;
    Rigidbody rigidBody;

    enum State { Alive,Dying,Transcending}
    State state = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (state == State.Alive)
        {
        RespondToThrustInput();
        RespondToRotateInput();

        }
        
    }

    

    

     void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            engineTrail.Stop();
        }
    }

    public void ApplyThrust()
    {
        float thrustThisFrame = moveThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);

        }
        engineTrail.Play();
    }

    public void RespondToRotateInput()
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
      if(state!= State.Alive) { return;}

        switch (collision.gameObject.tag)
        {
            case "Friendly":
               //do nothing
                break;
            case "Finish":
                StartWinScenario();

                break;
            default:
                StartDeathScenario();
                break;
        }
    }

    private void StartWinScenario()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        engineTrail.Stop();
        winParticles.Play();
        Invoke("LoadNextScene", lvlLoadDelay);
    }
    private void StartDeathScenario()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(explode);
        engineTrail.Stop();
        deathBlow.Play();
        Invoke("RestartLvL", lvlLoadDelay);
    }

   

    void RestartLvL()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
