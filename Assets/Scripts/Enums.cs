public struct ActionType
{
	public Letters action {get; private set;}
	public Letters element {get; private set;}

	public ActionType(Letters action, Letters element)
	{
		this.action = action;
		this.element = element;
	}
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
}