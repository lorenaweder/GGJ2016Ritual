using UnityEngine;
using System.Collections;

public class WaterShield : Shield {

	void Awake()
	{
		this.element = Elements.WATER;
		strong = Elements.FIRE;
		weak = Elements.EARTH;
	}
}