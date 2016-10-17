using UnityEngine;
using System.Collections;
using System;

public class CarMovement : MonoBehaviour
{

    //private variables
    private float _dir = 0f;
    private float _torqueDir = 0f;
    private float _maxFwdSpeed = -5000;
    private float _maxBwdSpeed = 2000f;
    private float _accelerationRate = 500;
    private float _decelerationRate = -100;
    private float _brakeSpeed = 2500f;
    private float _gravity = 9.81f;
    private float _slope = 0;
    private WheelJoint2D[] _wheelJoints;
    private JointMotor2D _motorBack;
    private Rigidbody2D _rigidbody;
    private GameObject _spawnPoint;
    private GameObject _camera;
    private GameObject _gameControllerObject;
    private GameController _gameController;
    private Transform _transform;


    //public variables
    public Transform CenterOfMass;
    public Transform RearWheel;
    public Transform FrontWheel;
    [Header("Sound Clips")]
    public AudioSource movingSound;
    public AudioSource Engine_idleSound;
    public AudioSource Tires_Screech;
    public AudioSource Reverse;

    // Use this for initialization
    void Start()
    {
        this._initialize();
    }
    // PRIVATE METHODS
    /**
     * This method initializes variables and object when called
     */
    private void _initialize()
    {
        this._camera = GameObject.FindWithTag("MainCamera");

        this._spawnPoint = GameObject.FindWithTag("SpawnPoint");

        this._gameControllerObject = GameObject.Find("GameController");

        this._gameController = this._gameControllerObject.GetComponent<GameController>() as GameController;

        this._transform = GetComponent<Transform>();

        this._rigidbody = GetComponent<Rigidbody2D>();

        //set the center of mass of the car
        _rigidbody.centerOfMass = CenterOfMass.transform.localPosition;

        //get the wheeljoin components
        _wheelJoints = gameObject.GetComponents<WheelJoint2D>();

        //get the reference to the motor of rear wheels joint
        _motorBack = _wheelJoints[0].motor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            this._transform.position = this._spawnPoint.transform.position;
        }
        //Decrease fuel and turn off idle sound
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Engine_idleSound.loop = false;
            Engine_idleSound.Stop();
        }
        //if player is going reverse Play Sound
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Reverse.isPlaying)
            { }
            else
            {
                Reverse.Play();
            }
        }
        //If player is going foward play sound
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (movingSound.isPlaying)
            { }
            else
            {
                movingSound.Play();
            }
        }
        //add ability to rotate the car around its axis
        _torqueDir = Input.GetAxis("Horizontal");
        if (_torqueDir != 0)
        {
            this._gameController.FuelValue -= 1;
            _rigidbody.AddTorque(3 * Mathf.PI * _torqueDir, ForceMode2D.Force);
        }
        else
        {
            _rigidbody.AddTorque(0);
            movingSound.Stop();
            Engine_idleSound.loop = true;
            Engine_idleSound.Play();
        }

        //determine the cars angle
        _slope = transform.localEulerAngles.z;

        //if slope is more than 180 add more power
        if (_slope >= 180)
        {
            _slope = _slope - 360;
        }
        _dir = Input.GetAxis("Horizontal");

        //check input
        if (_dir != 0)
        {
            //add speed
            _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed - (_dir * _accelerationRate - _gravity * Mathf.Sin((_slope * Mathf.PI) / 180) * 80) * Time.deltaTime, _maxFwdSpeed, _maxBwdSpeed);
        }
        //if no input and vehicle moving forward
        if ((_dir == 0 && _motorBack.motorSpeed < 0) || (_dir == 0 && _motorBack.motorSpeed == 0 && _slope < 0))
        {
            //decelerate the car while adding the speed if the car is on an slope
            _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed - (_decelerationRate - _gravity * Mathf.Sin((_slope * Mathf.PI) / 180) * 80) * Time.deltaTime, _maxFwdSpeed, 0);
        }
        else if ((_dir == 0 && _motorBack.motorSpeed > 0) || (_dir == 0 && _motorBack.motorSpeed == 0 && _slope > 0))
        {
            _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed - (-_decelerationRate - _gravity * Mathf.Sin((_slope * Mathf.PI) / 180) * 80) * Time.deltaTime, 0, _maxBwdSpeed);
        }
        //apply brakes to the car
        if (Input.GetKey(KeyCode.Space) && _motorBack.motorSpeed > 0)
        {
            _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed - _brakeSpeed * Time.deltaTime, 0, _maxBwdSpeed);
        }
        else if (Input.GetKey(KeyCode.Space) && _motorBack.motorSpeed < 0)
        {
            _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed + _brakeSpeed * Time.deltaTime, _maxFwdSpeed, 0);
            if (!Tires_Screech.isPlaying)
            {
                Tires_Screech.Play();
            }
        }
        //connect the motor to the joint
        _wheelJoints[0].motor = _motorBack;
        //moves camera to player
        this._camera.transform.position = new Vector3(
           this._transform.position.x,
           this._transform.position.y,
           -10f);
    }
}