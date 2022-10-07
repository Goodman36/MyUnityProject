using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum State { Start,Playing,Pause,Dead,Finish}
public class Rocket : MonoBehaviour
{
    public float speed = 10f;
    public float speedRotate = 10f;
    Rigidbody rb;
    AudioSource audioS;
    public AudioClip countdown;
    public AudioClip rocketBoom;
    public AudioClip finishPlatform;
    public AudioClip rocketFly;
    [SerializeField] ParticleSystem flyParticle;
    [SerializeField] ParticleSystem boomParticle;
    [SerializeField] ParticleSystem finishParticle;


    State state = State.Start;
    void Start()
    {
        state = State.Start;
        audioS = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(state == State.Start && Input.GetKeyDown(KeyCode.Space))
        {
            //audioS.PlayOneShot(countdown);
            //state = State.Pause;
            Invoke("PlayCountdown", 0.1f);
        }
        if (state == State.Playing)
        {
            Rotates();
            Lounch();
        }      
    }
    void PlayCountdown()
    {    
        state = State.Playing;   
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Playing)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Start":

                break;
            case "Energy":
                print("Energy");
                break;
            case "Finish":
                Finish();
                break;
            default:
                Dead();
                break;
                           
        }
    }
    void Dead()
    {
        flyParticle.Stop();
        boomParticle.Play();
        audioS.Stop();
        audioS.PlayOneShot(rocketBoom);
        Invoke("LoadFirstLevel", 4f);
        state = State.Dead;
        
    }
    void Finish()
    {
        finishParticle.Play();
        audioS.Stop();
        audioS.PlayOneShot(finishPlatform);
        Invoke("LoadNextLevel", 4f);
        state = State.Finish;
    }
    void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    void Lounch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            flyParticle.Play();
            rb.AddRelativeForce(Vector3.up * speed);
            if (!audioS.isPlaying)
            {
                audioS.PlayOneShot(rocketFly);
            }
            
        }
        else
        {
            audioS.Pause();
            flyParticle.Stop();
        }
    }
    void Rotates()
    {
        rb.freezeRotation = false;
       
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * speedRotate * Time.deltaTime);
        }
        rb.freezeRotation = true;
    }
}
