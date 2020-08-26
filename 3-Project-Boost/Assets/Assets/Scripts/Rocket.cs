using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Rocket : MonoBehaviour
{
    [SerializeField] float mainThrust = 300f;
    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    Rigidbody rigidbody;
    AudioSource audioSource;

    bool collisionsDisabled = false;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = .1f;
        rigidbody.drag = 0.2f;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) // Can trust while rotating.
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            thrustParticles.Stop();
        }
    }

    void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        thrustParticles.Play();
    }

    void RespondToRotateInput()
    {
        rigidbody.freezeRotation = true;
        float rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rigidbody.freezeRotation = false;
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            StartSuccessSequence();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly object detected");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        Invoke("ReloadScene", levelLoadDelay);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

}
