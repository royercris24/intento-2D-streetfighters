using System.Collections;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public string playerInput;
    // variables
    // los movimientos que puede hacer el character
    float horizontal;
    float vertical;
    bool attack1;
    bool attack2;
    bool attack3;

    StateManager states;
    // Use this for initialization
    void Start()
    {
        states = GetComponent<StateManager>();
    }

    // los movimientos que estaran en el input
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal" + playerInput);
        vertical = Input.GetAxis("Vertical" + playerInput);
        attack1 = Input.GetButton("Fire1" + playerInput);
        attack2 = Input.GetButton("Fire2" + playerInput);
        attack3 = Input.GetButton("Fire3" + playerInput);

        states.horizontal = horizontal;
        states.vertical = vertical;
        states.attack1 = attack1;
        states.attack2 = attack2;
        states.attack3 = attack3;
    }
}
