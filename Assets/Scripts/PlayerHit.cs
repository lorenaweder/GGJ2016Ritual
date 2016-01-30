using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {
	public Actor enemy;
	public Actor player;

	void OnCollisionEnter2D(Collision2D coll)
	{
		enemy.HideProjectile();
		player.TakeHit(player.hitInfo);
	}
}
