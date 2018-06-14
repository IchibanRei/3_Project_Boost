using System;
using UnityEngine;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Thrust();
        Rotate();
	}

        private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        { 
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            PlayThrusterSound(false);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayThrusterSound(true);
        }
    }

    private void Rotate()
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

    private void PlayThrusterSound(bool isThrustPlaying)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (isThrustPlaying)
        {
            audioSource.Stop();
        }
           
    }

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");//remove
                break;
            case "Fuel":
                print("refueled");
                break;
            default:
                print("You Died");
                break;

        }
    }
}
