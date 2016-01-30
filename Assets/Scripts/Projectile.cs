using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public new Transform transform;
	public Rigidbody2D rigidBody;
	Vector3 originalPos;
	public SpriteRenderer spriteRenderer;

	void Awake()
	{
		originalPos = transform.position;
		spriteRenderer = GetComponent<SpriteRenderer>();
		Hide();
	}

	public void Hide()
	{
		transform.position = originalPos;
		rigidBody.velocity = Vector2.zero;
		spriteRenderer.enabled = false;
	}

	public void Show(Elements element)
	{
		transform.position = originalPos;
		rigidBody.velocity = Vector2.zero;
		spriteRenderer.sprite = Game.attackByType[element];
		spriteRenderer.enabled = true;
	}
}
