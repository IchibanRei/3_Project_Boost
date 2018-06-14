using System;
using UnityEngine;

public class Rocket : MonoBehaviour {
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
            rigidBody.AddRelativeForce(Vector3.up);
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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward);
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
}
