using System.Collections;
using UnityEngine;

public class OpenDamageCollider : StateMachineBehaviour {

	StateManager states;
	public HandleDamageCollider.DamageType damageType;
	public HandleDamageCollider.DCtype dcType;
	public float delay;

	// es llamdo cuando una transmision empieza y la maquina de estados empieza a evaluar
	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(states == null){
			states = animator.transform.GetComponentInParent<StateManager>();
		}
		states.handleDC.OpenCollider(dcType, delay, damageType);
	}
	
	// es llamado cuando la transmision se termina y la maquina de estados termina de evaluar
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(states == null){
			states = animator.transform.GetComponentInParent<StateManager>();
		}
		states.handleDC.CloseColliders();
	}
}
