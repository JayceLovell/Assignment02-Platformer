using UnityEngine;
using System.Collections;

// reference to the UI namespace
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    // Private INSTANCE VARIABLES ++++++++++++
    private float _timeValue;
    private int _fuelValue;

    [Header("UI Objects")]
    public Text TimeLabel;
    public Text FuelLabel;

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
            if(this._fuelValue <= 0)
            {

            } else
            {
                this.FuelLabel.text = "Fuel: " + this._fuelValue;
            }
        }
    }
    public float TimeValue {

        get
        {
            return this._timeValue;
        }
        set
        {
            this._timeValue = value;
            if(this._timeValue <=0)
            {

            } else
            {
                this.TimeLabel.text = "Time Left: " + Mathf.Round(this._timeValue);
            }
        }
    }
	// Use this for initialization
	void Start () {
        this.FuelValue = 1000;
        this.TimeValue = 300.0f;

    }
	
	// Update is called once per frame
	void Update () {
        TimeValue -= Time.deltaTime;
        if (TimeValue < 0)
        {
            Debug.Log("Out of time");
        }
    }
}