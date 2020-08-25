using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float mainThrust = 300f;
    [SerializeField] float rcsThrust = 25;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip winSound;
    public Rigidbody rigidbody;
    public AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = 10f;
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
        }
    }

    void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
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

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }

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
        Invoke("LoadNextScene", 1f);
    }

    void StartCrashSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        Invoke("ReloadScene", 1f);
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
