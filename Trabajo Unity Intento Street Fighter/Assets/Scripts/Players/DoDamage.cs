using System.Collections;
using UnityEngine;

public class DoDamage : MonoBehaviour
{

    StateManager states;
    public HandleDamageCollider.DamageType damageType;
    // Use this for initialization
    void Start()
    {
        states = GetComponentInParent<StateManager>();
    }

    // el daño que se ejecutara en el character dependiendo en que collider sea afectado
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<StateManager>())
        {
            StateManager oState = other.GetComponentInParent<StateManager>();

            if (oState != states)
            {
                // el daño que puede hacer el character
                if (!oState.currentlyAttacking)
                {
                    oState.TakeDamage(10, damageType);
                }
            }
        }
    }

}
