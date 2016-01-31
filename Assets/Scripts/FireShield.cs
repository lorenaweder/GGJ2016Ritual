using UnityEngine;
using System.Collections;

public class FireShield : Shield {

	void Awake()
	{
		this.element = Elements.FIRE;
		strong = Elements.AIR;
		weak = Elements.WATER;
		equal = Elements.EARTH;
	}
}