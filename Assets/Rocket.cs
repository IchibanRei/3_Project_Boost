using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float deathTime = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explodeSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem explodeParticles;
    [SerializeField] ParticleSystem successParticles;

    [Range(2, 5)] [SerializeField] int startingLives;

    [SerializeField] bool isDebugMode = false;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if(Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
        Exit();
	}

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Advance level for debug
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C)) // disable collision for debug
        {
            isDebugMode = true;
            print("Debug mode enabled");
        }
    }

    private void Exit()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        { 
            ApplyThrust();            
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }
    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();

    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; // resume physics control of rotation
    }


    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || isDebugMode) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");//remove
                break;
            case "Finish":
                SuccessProcess();
                break;
            default:
                DeathProcess();
                break;
        }
    }

    private void SuccessProcess()
    {
        state = State.Transcending;
        audioSource.Stop();
        print("Load audio file " + successSound.name);
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void DeathProcess()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(explodeSound, 100f);
        mainEngineParticles.Stop();
        explodeParticles.Play();
        Invoke("LoadFirstLevel", deathTime);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
