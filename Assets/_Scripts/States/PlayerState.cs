using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerStatee {

	void PlayerState();
	void UpdateState (PlayerController player);
	void HandleInput();
}
