using UnityEngine;
using System.Collections;

public class WheelBehavuour : MonoBehaviour {

    public Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("WheelState", 1);
        }
        else
        {
            animator.SetInteger("WheelState", 0);
        }
        Debug.Log(animator.GetInteger("Wheelstate"));
    }
}
