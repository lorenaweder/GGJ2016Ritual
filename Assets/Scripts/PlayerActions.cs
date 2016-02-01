using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerActions {

	public Dictionary<int, ActionType> available;

	public PlayerActions()
	{
		available = new Dictionary<int, ActionType>(16);
		available.Add(Letters.ATTACK.GetHashCode() + Letters.FIRE.GetHashCode(), new ActionType(Actions.ATTACK, Elements.FIRE));
		available.Add(Letters.DEFEND.GetHashCode() + Letters.FIRE.GetHashCode(), new ActionType(Actions.DEFEND, Elements.FIRE));

		available.Add(Letters.ATTACK.GetHashCode() + Letters.WATER.GetHashCode(), new ActionType(Actions.ATTACK, Elements.WATER));
		available.Add(Letters.DEFEND.GetHashCode() + Letters.WATER.GetHashCode(), new ActionType(Actions.DEFEND, Elements.WATER));

		available.Add(Letters.ATTACK.GetHashCode() + Letters.AIR.GetHashCode(), new ActionType(Actions.ATTACK, Elements.AIR));
		available.Add(Letters.DEFEND.GetHashCode() + Letters.AIR.GetHashCode(), new ActionType(Actions.DEFEND, Elements.AIR));

		available.Add(Letters.ATTACK.GetHashCode() + Letters.EARTH.GetHashCode(), new ActionType(Actions.ATTACK, Elements.EARTH));
		available.Add(Letters.DEFEND.GetHashCode() + Letters.EARTH.GetHashCode(), new ActionType(Actions.DEFEND, Elements.EARTH));
	}
}
