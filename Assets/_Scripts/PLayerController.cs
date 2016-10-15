using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    // PRIVATE INSTANCE VARIABLES
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private float _move;

    public float Velocity = 10f;
    public Camera camera;
    public Transform SpawnPoint;

    [Header("Sound Clips")]
    public AudioSource movingSound;
    public AudioSource Engine_idleSound;

	// Use this for initialization
	void Start () {
        this._initialize();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //if press to move play sound
        if(Input.GetKeyDown(KeyCode.D))
        {
            Engine_idleSound.loop = false;
            Engine_idleSound.Stop();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            if (movingSound.isPlaying)
            { }
            else
            {
                movingSound.Play();
            }
        }
        else if(Input.GetKeyUp(KeyCode.D))
        {
            movingSound.Stop();
            Engine_idleSound.loop = true;
            Engine_idleSound.Play();
        }

        this._move = Input.GetAxis("Horizontal");
        this._rigidbody.AddForce(new Vector2(this._move * this.Velocity,0f),ForceMode2D.Force);
        this.camera.transform.position = new Vector3(
            this._transform.position.x,
            this._transform.position.y,
            -10f);

        if (Input.GetKeyUp(KeyCode.R))
        {
            _Respawn();
        }
	
	}
    // PRIVATE METHODS
    /**
     * This method respawns the car
     */
     private void _Respawn()
    {
        this._transform.position = this.SpawnPoint.position;
    }
    // PRIVATE METHODS
    /**
     * This method initializes variables and object when called
     */
     private void _initialize()
    {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._move = 0f;
    }
}
