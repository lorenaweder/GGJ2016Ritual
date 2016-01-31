using UnityEngine;
using System.Collections;

public class AirShield : Shield {

	void Awake()
	{
		this.element = Elements.AIR;
		strong = Elements.EARTH;
		weak = Elements.FIRE;
		equal = Elements.WATER;
	}

}