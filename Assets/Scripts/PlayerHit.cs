using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {
	public Actor enemy;
	public Actor player;

	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log("HIT HIT HIT");
		enemy.HideProjectile();
		player.TakeHit(player.hitInfo);
	}
}
