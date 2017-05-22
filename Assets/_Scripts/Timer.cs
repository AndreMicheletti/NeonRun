using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Timer {

	public int wait;
	private int timer;

	public Timer(int w) {
		this.wait = w;
		this.timer = 0;
	}

	public void Update () {
		if (timer < wait) {
			timer++;
		} else {
			doAction ();
			reset ();
		}
	}

	public void reset() {
		timer = 0;
	}

	protected abstract void doAction();

}
