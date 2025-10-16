using System.Collections;
using System.Collections.Generic;

using UnityEngine;

//We use pooling in order to reduce the computational power needed to run the game, Instead of creating objects we recycle them whenver we need them saving the time needed to allocate memory
public class PoolScript : MonoBehaviour {


	public List<GameObject> poolObjects = new List<GameObject>();
	public GameObject enemy;//variable used to instantiate enemies when the pool is almost empty
	public bool isSpawnTime;
	
	void Awake(){
		foreach (Transform t in this.transform) {
			poolObjects.Add (t.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		isSpawnTime = true;
	}

	void Update(){
		

	}


	//Get an object from the pool, if the pool is empty then the object has to be created
	public GameObject GetPoolObject(int life,int damage){
		
		if (poolObjects.Count == 0) {
			//Debug.Log("No object to give");
			GameObject go = Instantiate (enemy, this.transform.position, this.transform.rotation);
			go.GetComponent<EnemyAttack>().setLifeAndDamage(life, damage);
			return go;

		} else {
			int rand = Random.Range (0, poolObjects.Count);
			//Debug.Log("object avaialable");
			GameObject temp = poolObjects [rand];
			poolObjects.RemoveAt (rand);
			//Debug.Log("object name"+temp.name);
			temp.GetComponent<EnemyAttack>().setLifeAndDamage(life, damage);
			return temp;
		}
	}
	//Disable and object and place it back to the pool
	public void recyclePool(GameObject rpo){
		Debug.Log("Recycling");
		poolObjects.Add(rpo);
		rpo.transform.SetParent (this.transform);
		rpo.transform.position = Vector3.zero;
		rpo.SetActive (false);
	}
		

}