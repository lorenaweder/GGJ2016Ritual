using UnityEngine;
using System.Collections;

public class WaterShield : Shield {

	void Awake()
	{
		this.element = Elements.WATER;
	}

	public override void TakeDamage(Elements type, float attack){}
}