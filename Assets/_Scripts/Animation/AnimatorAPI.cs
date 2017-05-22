using UnityEngine;

public static class AnimatorAPI {

	public static bool isAnimatorPlaying(Animator anim) {
		return anim.GetCurrentAnimatorStateInfo (0).length >
			anim.GetCurrentAnimatorStateInfo (0).normalizedTime;
	}

	public static bool isAnimationPlaying(Animator anim, string tag) {
		return isAnimatorPlaying (anim) &&
			anim.GetCurrentAnimatorStateInfo (0).IsTag (tag);
	}

	public static bool isAnimationStopped(Animator anim, string name) {
		return !isAnimationPlaying (anim, name);
	}

}
