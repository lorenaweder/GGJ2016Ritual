using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour { 

	public BoxCollider2D collider;
	public SpriteRenderer sprite;
	public float maxHealth;
	protected float health;
	public Elements element {get; protected set;}

	void Start()
	{
		ResetShield();
	}

	public virtual void RiseShield()
	{
		Debug.Log("Shield UP: " + element);
		sprite.enabled = true;
		collider.enabled = true;
	}

	public void ResetShield()
	{
		Debug.Log("Reset shield");
		sprite.enabled = false;	
		collider.enabled = false;
	}

	public virtual void DestroyShield()
	{
		Debug.Log("Shield DOWN: " + element);
		ResetShield();
	}

	public virtual void TakeDamage(Elements type, float attack){}
	public void UpdateGraphics()
	{
		float opacity = health * 100 / maxHealth;
	}
}