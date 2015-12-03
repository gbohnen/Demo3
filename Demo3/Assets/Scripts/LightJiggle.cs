using UnityEngine;
using System.Collections;

public class LightJiggle : MonoBehaviour {

	public int _layerIndex = 0;

	// starts the object at a random frame in its animation
	void Start () {
		Animator anim = GetComponent<Animator>();
		AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(_layerIndex);
		anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
	}
}