
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour {

	public int lifeTime;

	private IEnumerator Start()
	{
		lifeTime = Random.Range(5,15);
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
