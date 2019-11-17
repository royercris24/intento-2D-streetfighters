using System.Collections;
using UnityEngine;

public class HandleAnimations : MonoBehaviour
{
    // animaciones
    public Animator anim;
    StateManager states;

    // la velocidad de ataque
    public float attackRate = 0.3f;
    public AttackBase[] attacks = new AttackBase[2];
    // Referencias
    void Start()
    {
        states = GetComponent<StateManager>();
        anim = GetComponentInChildren<Animator>();
    }

    // esto controla las animaciones del character
    void FixedUpdate()
    {
        states.dontMove = anim.GetBool("DontMove");

        anim.SetBool("TakesHit", states.gettingHit);
        anim.SetBool("OnAir", !states.onGround);
        anim.SetBool("Crouch", states.crouch);

        float movement = Mathf.Abs(states.horizontal);
        anim.SetFloat("Movement", movement);

        if (states.vertical < 0)
        {
            states.crouch = true;
        }
        else
        {
            states.crouch = false;
        }
        HandleAttacks();
    }

    void HandleAttacks()
    {
        // este comprueba si el character puede atacar
        if (states.canAttack)
        {
            // si cuando atacas haces el primer ataque, lo ejecuta
            if (states.attack1)
            {
                attacks[0].attack = true;
                attacks[0].attackTimer = 0;
                attacks[0].timesPressed++;
            }

            // si el character esta atacando
            if (attacks[0].attack)
            {
                // el ataque ira por tiempo
                attacks[0].attackTimer += Time.deltaTime;

                // si el ataque por tiempo supera la cantidad de ataques que se pueden hacer en ese minimo de tiempo,
                // no saldra el ataque
                if (attacks[0].attackTimer > attackRate || attacks[0].timesPressed >= 3)
                {
                    attacks[0].attack = false;
                    attacks[0].attackTimer = 0;
                    attacks[0].timesPressed = 0;
                }
            }

            // esta parte es la misma que la de arriba
            // si cuando atacas haces el primer ataque, lo ejecuta
            if (states.attack2)
            {

                attacks[1].attack = true;
                attacks[1].attackTimer = 0;
                attacks[1].timesPressed++;
            }

            // si el character esta atacando
            if (attacks[1].attack)
            {
                // el ataque ira por tiempo
                attacks[1].attackTimer += Time.deltaTime;

                // esta parte es la misma que la de arriba
                // si cuando atacas haces el primer ataque, lo ejecuta
                if (attacks[1].attackTimer > attackRate || attacks[0].timesPressed >= 3)
                {
                    attacks[1].attack = false;
                    attacks[1].attackTimer = 0;
                    attacks[1].timesPressed = 0;
                }
            }
        }

        // animator
        anim.SetBool("Attack1", attacks[0].attack);
        anim.SetBool("Attack2", attacks[1].attack);
    }

    // en el salto el character no podra atacar
    public void JumpAnim()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Jump", true);
        StartCoroutine(CloseBoolInAnim("Jump"));
    }

    IEnumerator CloseBoolInAnim(string name)
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool(name, false);
    }
}

[System.Serializable]
public class AttackBase
{
    // el ataque
    public bool attack;
    // el tiempo del ataque
    public float attackTimer;
    // cuantas veces pulsamos
    public int timesPressed;
}
