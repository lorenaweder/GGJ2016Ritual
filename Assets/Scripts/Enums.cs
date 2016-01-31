[System.Serializable]
public class ADParams
{
	public Elements element;
	public float recoveryTime = 0f;
	public float connectTime = 0f;
	public float animationTime = 0f;
	public float projectileSpeed = 0f;
}

public struct ActionType
{
	public Actions action {get; private set;}
	public Elements element {get; private set;}

	public ActionType(Actions action, Elements element)
	{
		this.action = action;
		this.element = element;
	}
}

public class AttackHitInfo
{
	public AttackHitInfo(Elements e, float a)
	{
		element = e;
		attack = a;
	}
	public Elements element;
	public float attack;
}

public enum Buttons
{
	NONE,
	UP,
	DOWN,
	RIGHT,
	LEFT
}

public enum Elements
{
	NONE 	= 0,
	FIRE 	= 1,
	WATER 	= 2,
	EARTH 	= 3,
	AIR		= 4
}

public enum Actions
{
	NONE 	= 0,
	ATTACK 	= 10,
	DEFEND 	= 20,
	BUFF 	= 30,
	DEBUFF 	= 40
}

public enum Letters
{
	NONE 	= 0,
	FIRE 	= 1,
	WATER 	= 2,
	EARTH 	= 3,
	AIR 	= 4,
	ATTACK 	= 10,
	DEFEND 	= 20,
	BUFF 	= 30,
	DEBUFF 	= 40
}

public enum ActorStates
{
	IDLE,
	SHOWING,
	ATTACKING,
	RECOVERY
}