
using UnityEngine.UI;
using System.Globalization;
using UnityEngine;
using System.Threading;

//Game Maanger
public class EnemyManager : MonoBehaviour
{
    public PlayerScript playerHealth;

	public float spawnTime;
    public Transform[] spawnPoints;
	public Transform[] pool;
	public int count;
	 public int enemyLimit;
	public int enemiesCount;
	public int startLife;
	public int startDamage;
	public int life;
	public int startNumberOfenemies;
	public int damage;
	public Text EnemiesCountText;
	public int score;
	public GameObject scoreText;
	public GameObject waveText;




	//function to reduce the number of active enemies
	public void removeEnemy(){
		enemiesCount -= 1;
		updateEnemiesCount();
		if (enemiesCount == 0)
		{
			WaveManager.current.RoundEnded();
			//Debug.Log("wave ended");
		}

		score += 100;
		scoreText.GetComponent<Text>().text = "Score " + score;

	}
	//Reset the counters of the game
	public void resetLevel(){
		count = 0;
		enemyLimit += 1;
		enemiesCount = enemyLimit;
	}

    void Start ()
    {

		WaveManager.current.roundEnded += UpdateWave;
		life = startLife;
		damage = startDamage;
		score = 0;
		count = 0;
		enemiesCount = enemyLimit;
		updateEnemiesCount();
	Spawn();
	 InvokeRepeating ("Spawn", spawnTime, spawnTime); //Create method called after specific time which is responsible of spawning enemies
	}


    void Spawn ()
    {
        if(playerHealth.currentHealth <= 0f)//stop spawning enemies when player is dead
		{
			//Debug.Log("player health 0" );
			return;
        }
		if (count == enemyLimit) {//stop spawning enemies when the needed number per wave is reached
			//Debug.Log("max enemies reached");
			return;
		}

		count++;
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		int poolIndex = Random.Range(0, pool.Length);
		//Debug.Log("testing" + spawnPointIndex);

		//Debug.Log("length of array" + spawnPoints.Length);
		//Get the enemy from a random pool and spawn it in a random locaiton
		GameObject pO = pool[poolIndex].gameObject.GetComponent<PoolScript> ().GetPoolObject (life, damage);
		pO.transform.parent = this.gameObject.transform;
		pO.transform.position = spawnPoints[spawnPointIndex].position;
		pO.gameObject.SetActive (true);
		pO.GetComponent<EnemyAttack>().reset();
		//Debug.Log("object made active");

	}
	//Increase the enemies health and damage as well as number each wave
	private void UpdateWave()
	{
		count = 0;
	    enemyLimit+=1;
		life += 10;
		damage += 5;
		enemiesCount = enemyLimit;
		updateEnemiesCount();
	}
	//Change the text specifiying enemy count
	void updateEnemiesCount()
    {
		EnemiesCountText.text = "Number of Enemies: " + enemiesCount + "/" + enemyLimit;

	}

	//Restart the game by reseting the paraemters: enemy number limit ,damage and life
	public void Restart()
    {
		life = startLife;
		damage = startDamage;
		enemyLimit= startNumberOfenemies;
		while (this.transform.childCount!=0)//Recycle all the enemies in the game
		{
			GameObject Go = this.transform.GetChild(0).gameObject;
			Go.GetComponent<EnemyAttack>().isDead = true;
			Go.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
			Go.GetComponent<EnemyAttack>().isSinking = false;
			if(Go.GetComponent<EnemyAttack>().isRanged==false)
				pool[0].gameObject.GetComponent<PoolScript>().recyclePool(Go.gameObject);
			else
				pool[1].gameObject.GetComponent<PoolScript>().recyclePool(Go.gameObject);

		}

		score = 0;
		count = 0;
		enemiesCount = enemyLimit;
		updateEnemiesCount();
		scoreText.GetComponent<Text>().text = "Score " + score;
		waveText.GetComponent<WaveWriter>().restart();
	}

}
