using UnityEngine;
using System.Collections;

public class EarthShield : Shield {

	void Awake()
	{
		this.element = Elements.EARTH;
	}

	public override void TakeDamage(Elements type, float attack){}
}