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
    private GameObject _camera;
    private GameObject _gameControllerObject;
    private GameController _gameController;
    private Transform _transform;
    private WheelJoint2D _connectiontowheelforsound;
    private float _lowPitch = 0.5f;
    private float _highPitch = 5f;
    private float _reductionFactor = 0.001f;


    //public variables
    public Transform CenterOfMass;
    public Transform RearWheel;
    public Transform FrontWheel;
    [Header("Sound Clips")]
    public AudioSource Tires_Screech;
    public AudioSource Engine_Sound;

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

        this._gameControllerObject = GameObject.Find("GameController");

        this._gameController = this._gameControllerObject.GetComponent<GameController>() as GameController;

        this._transform = GetComponent<Transform>();

        this._rigidbody = GetComponent<Rigidbody2D>();

        //set the center of mass of the car
        _rigidbody.centerOfMass = CenterOfMass.transform.localPosition;

        //get the wheeljoin components
        _wheelJoints = gameObject.GetComponents<WheelJoint2D>();
        _connectiontowheelforsound = GetComponent<WheelJoint2D>();

        //get the reference to the motor of rear wheels joint
        _motorBack = _wheelJoints[0].motor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_gameController.IsGameover && !_gameController.IsGamewin)
        {
            if(_rigidbody.velocity.magnitude>2)
            {
                //increase score
                this._gameController.ScoreValue++;
            }
            //add ability to rotate the car around its axis
            _torqueDir = Input.GetAxis("Horizontal");

            if (_torqueDir != 0)
            {
                _rigidbody.AddTorque(3 * Mathf.PI * _torqueDir, ForceMode2D.Force);
            }else
            {
                _rigidbody.AddTorque(0);
            }

            //determine the cars angle
            _slope = transform.localEulerAngles.z;

            //Give the vehicle a wheele barrel effect
            if(_rigidbody.rotation > 60)
            {
                _rigidbody.rotation -= 15;
            }
            //if slope is more than 180 add more power
            if (_slope >= 180)
            {
                _slope = _slope - 360;
            }
            _dir = Input.GetAxis("Horizontal");

            //check input
            if (_dir != 0)
            {
                //decrease fuel
                this._gameController.FuelValue -= 1;

                //add speed
                _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed - (_dir * _accelerationRate - _gravity * Mathf.Sin((_slope * Mathf.PI) / 180) * 80) * Time.deltaTime, _maxFwdSpeed, _maxBwdSpeed);
            }

            //if no input and vehicle moving forward
            if ((_dir == 0 && _motorBack.motorSpeed < 0) || (_dir == 0 && _motorBack.motorSpeed == 0 && _slope < 0))
            {
                //decelerate the car while adding the speed if the car is on an slope
                _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed - (_decelerationRate - _gravity * Mathf.Sin((_slope * Mathf.PI) / 180) * 80) * Time.deltaTime, _maxFwdSpeed, 0);
            } else if ((_dir == 0 && _motorBack.motorSpeed > 0) || (_dir == 0 && _motorBack.motorSpeed == 0 && _slope > 0))
            {
                _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed - (-_decelerationRate - _gravity * Mathf.Sin((_slope * Mathf.PI) / 180) * 80) * Time.deltaTime, 0, _maxBwdSpeed);
            }

            //apply brakes to the car
            if (Input.GetKey(KeyCode.Space) && _motorBack.motorSpeed > 0)
            {
                _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed - _brakeSpeed * Time.deltaTime, 0, _maxBwdSpeed);
            } else if (Input.GetKey(KeyCode.Space) && _motorBack.motorSpeed < 0)
            {
                _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed + _brakeSpeed * Time.deltaTime, _maxFwdSpeed, 0);
                if (!Tires_Screech.isPlaying)
                {
                    Tires_Screech.Play();
                }
            }
            //connect the motor to the joint
            _wheelJoints[0].motor = _motorBack;

            //get the absolute value of joinSpeed
            float forwardSpeed = Mathf.Abs(_connectiontowheelforsound.jointSpeed);

            //calculated pitch added to audio source
            float pitchFactor = Mathf.Abs(forwardSpeed * _reductionFactor * _torqueDir);

            //clamp the calculated pitch 
            Engine_Sound.pitch = Mathf.Clamp(pitchFactor, _lowPitch, _highPitch);
        }
        else
        {
            _dir = 0;
            _torqueDir = 0;
            Engine_Sound.loop = false;
            Engine_Sound.Stop();
            //apply brakes to the car
            if (_motorBack.motorSpeed > 0)
            {
                _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed - _brakeSpeed * Time.deltaTime, 0, _maxBwdSpeed);
            }
            else if ( _motorBack.motorSpeed < 0)
            {
                _motorBack.motorSpeed = Mathf.Clamp(_motorBack.motorSpeed + _brakeSpeed * Time.deltaTime, _maxFwdSpeed, 0);
                if (!Tires_Screech.isPlaying)
                {
                    Tires_Screech.Play();
                }
            }
        }
        if (!_gameController.IsGamewin)
        {
            //moves camera to player
            this._camera.transform.position = new Vector3(
               this._transform.position.x,
               this._transform.position.y,
               -10f);
        }
    }
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("End"))
        {
            _gameController.IsGamewin = true;
        }
    }
}