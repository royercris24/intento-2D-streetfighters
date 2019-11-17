using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // la vida
    public int health = 150;

    // controles
    public float horizontal;
    public float vertical;
    public bool attack1;
    public bool attack2;
    public bool attack3;
    public bool crouch;

    // si podemos atacar
    public bool canAttack;

    // si somos golpeados
    public bool gettingHit;

    // si en este momento estamos a tacando
    public bool currentlyAttacking;

    // si no nos movemos
    public bool dontMove;

    // si estamos en el suelo
    public bool onGround;

    // si miramos a la derecha
    public bool lookRight;

    // la barra de vida hecha en el canvas
    public Slider healthSlider;
    SpriteRenderer sRenderer;

    [HideInInspector]
    public HandleDamageCollider handleDC;
    [HideInInspector]
    public HandleAnimations handleAnim;
    [HideInInspector]
    public HandleMovement HandleMovement;

    public GameObject[] movementColliders;

    // referencias
    void Start()
    {
        handleDC = GetComponent<HandleDamageCollider>();
        handleAnim = GetComponent<HandleAnimations>();
        HandleMovement = GetComponent<HandleMovement>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // controla donde mira el character
        sRenderer.flipX = lookRight;

        onGround = isOnGround();

        if (healthSlider != null)
        {
            healthSlider.value = Mathf.InverseLerp(0f, LevelManager.PlayerHealth, health);
        }
        // si la vida llega a cero, el character esta muerto
        if (health <= 0)
        {
            if (LevelManager.GetInstance().countdown)
            {
                LevelManager.GetInstance().EndTurnFunction();

                handleAnim.anim.Play("Dead");
            }
        }
    }

    bool isOnGround()
    {
        bool retVal = false;

        LayerMask layer = ~(1 << gameObject.layer | 1 << 3);
        retVal = Physics2D.Raycast(transform.position, -Vector2.up, 0.1f, layer);

        return retVal;
    }

    public void ResetStateInputs()
    {
        horizontal = 0;
        vertical = 0;
        attack1 = false;
        attack2 = false;
        attack3 = false;
        crouch = false;
        gettingHit = false;
        currentlyAttacking = false;
        dontMove = false;
    }

    public void CloseMovementCollider(int index)
    {
        movementColliders[index].SetActive(false);
    }
    public void OpenMovementCollider(int index)
    {
        movementColliders[index].SetActive(true);
    }

    // este sirve para calcular el daño, dependiendo de que tipo
    public void TakeDamage(int damage, HandleDamageCollider.DamageType damageType)
    {

        if (!gettingHit)
        {
            switch (damageType)
            {
                // este seria un ataque normal y el segundo un ataque potente
                case HandleDamageCollider.DamageType.light:
                    StartCoroutine(CloseImmortality(0.3f));
                    break;
                case HandleDamageCollider.DamageType.heavy:
                    HandleMovement.AddVelocityOnCharacter(
                        ((!lookRight) ? Vector3.right * 1 : Vector3.right * -1) + Vector3.up, 0.5f);
                    StartCoroutine(CloseImmortality(1));
                    break;
            }
            // el daño que recibe se lo resta a la vida
            health -= damage;
            gettingHit = true;
        }
    }


    IEnumerator CloseImmortality(float timer)
    {
        yield return new WaitForSeconds(timer);
        gettingHit = false;
    }

}
