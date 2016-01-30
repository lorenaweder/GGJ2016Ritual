using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour { 

	public SpriteRenderer sprite;
	public float maxHealth;
	protected float health;
	public Elements element {get; protected set;}

	public virtual void RiseShield()
	{
		Debug.Log("Shield UP: " + element);
		sprite.enabled = true;
	}

	public void ResetShield()
	{
		sprite.enabled = false;	
	}

	public virtual void DestroyShield()
	{
		Debug.Log("Shield DOWN: " + element);
	}

	public virtual void TakeDamage(Elements type, float attack){}
	public void UpdateGraphics()
	{
		float opacity = health * 100 / maxHealth;
	}
}