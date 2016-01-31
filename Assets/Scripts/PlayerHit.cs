using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {
	public Actor enemy;
	public Actor player;

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(Game.gameOver)
			return;
		Debug.Log("Hit");
		enemy.HideProjectile();
		player.TakeHit(enemy.hitInfo);
		enemy.CleanHitInfo();
	}
}
