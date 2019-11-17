using System.Collections;
using UnityEngine;

public class HandleMovementCollider : StateMachineBehaviour {

	StateManager states;

	public int index;

	// es llamdo cuando una transmision empieza y la maquina de estados empieza a evaluar
	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
		if(states == null){
			states = animator.transform.GetComponentInParent<StateManager>();	
		}
		states.CloseMovementCollider(index);
	}

	// es llamado cuando la transmision se termina y la maquina de estados termina de evaluar
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		if(states == null){
			states = animator.transform.GetComponentInParent<StateManager>();
		}
		states.OpenMovementCollider(index);
	}
	
}
