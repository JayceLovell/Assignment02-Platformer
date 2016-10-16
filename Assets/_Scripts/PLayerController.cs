using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    // PRIVATE INSTANCE VARIABLES
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private float _move;
    private GameObject _spawnPoint;
    private GameObject _camera;
    private GameObject _gameControllerObject;
    private GameController _gameController;

    public float Velocity = 20f;

    [Header("Sound Clips")]
    public AudioSource movingSound;
    public AudioSource Engine_idleSound;

	// Use this for initialization
	void Start () {
        this._initialize();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        this._move = Input.GetAxis("Horizontal");

        //if press to move play sound
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this._gameController.FuelValue -= 1;
            Engine_idleSound.loop = false;
            Engine_idleSound.Stop();
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (movingSound.isPlaying)
            { }
            else
            {
                movingSound.Play();
            }
        }
        else if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            movingSound.Stop();
            Engine_idleSound.loop = true;
            Engine_idleSound.Play();
        }

        this._rigidbody.AddForce(new Vector2(this._move * this.Velocity,0f),ForceMode2D.Force);

        this._camera.transform.position = new Vector3(
            this._transform.position.x,
            this._transform.position.y,
            -10f);
        if(Input.GetKeyDown(KeyCode.R))
        {
            this._transform.position = this._spawnPoint.transform.position;
            this._rigidbody.AddForce(new Vector2(this._move * 0, 0f), ForceMode2D.Force);
        }
	}
    // PRIVATE METHODS
    /**
     * This method initializes variables and object when called
     */
     private void _initialize()
    {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._camera = GameObject.FindWithTag("MainCamera");
        this._move = 0f;
        this._spawnPoint = GameObject.FindWithTag ("SpawnPoint");

        this._gameControllerObject = GameObject.Find ("GameController");
        this._gameController = this._gameControllerObject.GetComponent<GameController> () as GameController;
    }
}
