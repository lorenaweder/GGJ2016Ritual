using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour { 

	public TextMesh healthText;
	public new BoxCollider2D collider;
	public SpriteRenderer sprite;
	public float maxHealth = 10;
	protected float health = 10;
	public Elements element {get; protected set;}

	protected Elements strong, weak;

	void Start()
	{
		ResetShield();
	}

	public virtual void RiseShield()
	{
		healthText.text = element.ToString() + " " + health.ToString();
		sprite.enabled = true;
		collider.enabled = true;
	}

	public void ResetShield()
	{
		health = maxHealth;
		sprite.enabled = false;	
		collider.enabled = false;
		healthText.text = "";
	}

	public virtual float TakeDamage(Elements type, out bool shieldDied)
	{
		if(type == strong)
		{
			health -= 1;
			shieldDied = IsShieldDestroyed();
			return 0.5f;
		}

		if(type == weak)
		{
			health -= 10;
			shieldDied = IsShieldDestroyed();
			return 1.5f;
		}

		health -= 5;
		shieldDied = IsShieldDestroyed();
		return 1;	
	}

	bool IsShieldDestroyed()
	{
		healthText.text = element.ToString() + " " + health.ToString();
		if(health <= 0)
		{
			ResetShield();
			return true;
		}
		return false;
	}
}