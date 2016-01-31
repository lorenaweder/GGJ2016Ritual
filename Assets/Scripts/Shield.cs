using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour { 

	public TextMesh healthText;
	public BoxCollider2D collider;
	public SpriteRenderer sprite;
	public float maxHealth = 10;
	protected float health = 10;
	public Elements element {get; protected set;}

	protected Elements strong, weak, equal;

	void Start()
	{
		ResetShield();
	}

	public virtual void RiseShield()
	{
		healthText.text = health.ToString();
		Debug.Log("Shield UP: " + element);
		sprite.enabled = true;
		collider.enabled = true;
	}

	public void ResetShield()
	{
		Debug.Log("Reset shield");
		sprite.enabled = false;	
		collider.enabled = false;
		healthText.text = "";
	}

	public virtual void DestroyShield()
	{
		Debug.Log("Shield DOWN: " + element);
		ResetShield();
	}

	public virtual float TakeDamage(Elements type)
	{
		if(type == strong)
		{
			health -= 1;
			CheckLife();
			return 0.5f;
		}

		if(type == weak)
		{
			health -= 10;
			CheckLife();
			return 1.5f;
		}

		health -= 5;
		CheckLife();
		return 1;	
	}

	void CheckLife()
	{
		healthText.text = health.ToString();
		if(health <= 0)
			DestroyShield();
	}

	public void UpdateGraphics()
	{
		float opacity = health * 100 / maxHealth;
	}
}