using System.Collections;
using UnityEngine;

public class HandleMovement : MonoBehaviour
{

    Rigidbody2D rb;
    StateManager states;
    HandleAnimations anim;

    // los movimientos
    public float acceleration = 30;
    public float airAcceleration = 15;
    public float maxSpeed = 60;
    public float jumpSpeed = 5;
    public float jumpDuration = 5;
    float actualSpeed;
    float jumpTimer;
    bool canVariableJump;
    bool justJumped;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<StateManager>();
        anim = GetComponent<HandleAnimations>();
        rb.freezeRotation = true;
    }

    // el movimiento y el salto
    void FixedUpdate()
    {
        if (!states.dontMove)
        {
            HorizontalMovement();
            Jump();
        }
    }

    // este es el movimiento del personaje 
    void HorizontalMovement()
    {
        actualSpeed = this.maxSpeed;

        if (states.onGround && !states.currentlyAttacking)
        {
            rb.AddForce(new Vector2((states.horizontal * actualSpeed) - rb.velocity.x * this.acceleration, 0));

            //por si se desliza el personaje
            if (states.horizontal == 0 && states.onGround)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    void Jump()
    {
        if (states.vertical > 0)
        {
            // con este se salta
            if (!justJumped)
            {
                justJumped = true;

                // si el character esta en el suelo, empieza la animacion del salto
                if (states.onGround)
                {
                    anim.JumpAnim();

                    // dependiendo de la velocidad que tenga el character al andar, saltara mas lejos
                    rb.velocity = new Vector3(rb.velocity.x, this.jumpSpeed);
                    jumpTimer = 0;
                    canVariableJump = true;
                }
                else
                {
                    // esto pone la fuerza de salto
                    if (canVariableJump)
                    {
                        jumpTimer += Time.deltaTime;

                        if (jumpTimer < this.jumpDuration / 1000)
                        {
                            rb.velocity = new Vector3(rb.velocity.x, this.jumpSpeed);
                        }
                    }
                }
            }
            else
            {
                justJumped = false;
            }
        }
    }

    public void AddVelocityOnCharacter(Vector3 direction, float timer)
    {
        StartCoroutine(AddVelocity(timer, direction));
    }

    // hace que cuanto mas tiempos estas andando, mas velocidad tienes(hasta cierto limite)
    IEnumerator AddVelocity(float timer, Vector3 direction)
    {
        float t = 0;

        while (t < timer)
        {
            t += Time.deltaTime;

            rb.AddForce(direction * 2);
            yield return null;
        }
    }
}
