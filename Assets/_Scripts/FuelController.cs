using UnityEngine;
using System.Collections;

public class FuelController : MonoBehaviour {

    private GameObject _gameControllerObject;
    private GameController _gameController;

    // Use this for initialization
    void Start () {

        this._gameControllerObject = GameObject.Find("GameController");

        this._gameController = this._gameControllerObject.GetComponent<GameController>() as GameController;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_gameController.IsGameover)
        this._gameController.FuelValue += 300;

        Destroy(gameObject);
    }
}
