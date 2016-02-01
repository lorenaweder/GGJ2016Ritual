using UnityEngine;
using System.Collections;

public class EarthShield : Shield {

	void Awake()
	{
		this.element = Elements.EARTH;
		strong = Elements.WATER;
		weak = Elements.AIR;
	}
}