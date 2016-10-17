using UnityEngine;
using System.Collections;

// reference to the UI namespace
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Private INSTANCE VARIABLES ++++++++++++
    private float _timeValue;
    private int _fuelValue;
    private bool _isGameOver;

    public AudioSource GameOverSound;

    [Header("UI Objects")]
    public Text TimeLabel;
    public Text FuelLabel;
    public Text GameOverLabel;

    // PUBLIC PROPERTIES ++++++++++++++++++++
    public int FuelValue
    {
        get
        {
            return this._fuelValue;
        }
        set
        {
            this._fuelValue = value;
            if (this._fuelValue <= 0)
            {
                IsGameover = true;
            }
            else
            {
                this.FuelLabel.text = "Fuel: " + this._fuelValue;
            }
        }
    }
    public float TimeValue
    {

        get
        {
            return this._timeValue;
        }
        set
        {
            this._timeValue = value;
            if (this._timeValue <= 0)
            {
                IsGameover = true;
            }
            else
            {
                this.TimeLabel.text = "Time Left: " + Mathf.Round(this._timeValue);
            }
        }
    }
    public bool IsGameover
    {
        get
        {
            return this._isGameOver;
        }
        set
        {
            this._isGameOver = value;
            if(IsGameover)
            {
                this.GameOverLabel.gameObject.SetActive(true);
                GameOverSound.Play();
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        this.FuelValue = 1000;
        this.TimeValue = 300.0f;
        this.IsGameover = false;
        this.GameOverLabel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TimeValue -= Time.deltaTime;
    }
}