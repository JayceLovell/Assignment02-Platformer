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
    private bool _isGameWin;
    private int _score;

    public AudioSource GameOverSound;

    [Header("UI Objects")]
    public Text TimeLabel;
    public Text FuelLabel;
    public Text GameOverLabel;
    public Text ScoreLabel;

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
            if (this._timeValue <= 0 || IsGamewin)
            {
                IsGameover = true;
            }
            else
            {
                this.TimeLabel.text = "Time Left: " + Mathf.Round(this._timeValue);
            }
        }
    }
    public int ScoreValue
    {
        get
        {
            return this._score;
        }
        set
        {
            this._score = value;
            this.ScoreLabel.text = "Score: " + this._score;
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
                this.GameOverLabel.text = "GameOver";
                GameOverSound.Play();
            }
        }
    }
    public bool IsGamewin
    {
        get
        {
            return this._isGameWin;
        }
        set
        {
            this._isGameWin = value;
            if(IsGamewin)
            {
                this.GameOverLabel.gameObject.SetActive(true);
                this.GameOverLabel.text = "You Win";
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        this.FuelValue = 1000;
        this.TimeValue = 300.0f;
        this.IsGameover = false;
        this.IsGamewin = false;
        this.GameOverLabel.gameObject.SetActive(false);
        this.ScoreValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TimeValue -= Time.deltaTime;
    }
}