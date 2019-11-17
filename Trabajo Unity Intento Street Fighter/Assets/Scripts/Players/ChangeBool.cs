using UnityEngine;
using System.Collections;

public class ChangeBool : StateMachineBehaviour {

    public string boolName;
    public bool status;
    public bool resetOnExit;

	// es llamdo cuando una transmision empieza y la maquina de estados empieza a evaluar
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        animator.SetBool(boolName, status);
	}

	

	// es llamado cuando la transmision se termina y la maquina de estados termina de evaluar
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
        if(resetOnExit)
            animator.SetBool(boolName, !status);
	}

}
