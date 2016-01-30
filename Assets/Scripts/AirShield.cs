using UnityEngine;
using System.Collections;

public class AirShield : Shield {

	void Awake()
	{
		this.element = Elements.AIR;
	}

	public override void TakeDamage(Elements type, float attack){}
}