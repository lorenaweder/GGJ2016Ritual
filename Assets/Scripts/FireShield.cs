using UnityEngine;
using System.Collections;

public class FireShield : Shield {

	void Awake()
	{
		this.element = Elements.FIRE;
	}

	public override void TakeDamage(Elements type, float attack){}
}