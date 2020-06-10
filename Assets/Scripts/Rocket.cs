
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
    bool collisionsDisabled = false;

    bool isTransitioning = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();

        }
        if (Debug.isDebugBuild)//if debug build is on obviously
        {
        RespondToDebug();

        }
    }

    private void RespondToDebug()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
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
            StopApplyThrust();
        }
    }

    private void StopApplyThrust()
    {
        audioSource.Stop();
        engineTrail.Stop();
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
        rigidBody.angularVelocity = Vector3.zero;//stoping rotation via physics
        float rotationThisFrame = rotationThrust * Time.deltaTime;
      
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
      if(isTransitioning || collisionsDisabled) { return;}

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
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        engineTrail.Stop();
        winParticles.Play();
        Invoke("LoadNextScene", lvlLoadDelay);
    }
    private void StartDeathScenario()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(explode);
        engineTrail.Stop();
        deathBlow.Play();
        Invoke("RestartLvL", lvlLoadDelay);
    }

   

    void RestartLvL()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScneneIndex ;
        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings-1)
        {
            nextScneneIndex = 0;
        }
        else nextScneneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextScneneIndex);
       
    }
}
