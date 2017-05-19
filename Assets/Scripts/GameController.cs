using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

  public Text scoreText;
  public Text loseText;
  
  public delegate void PathDelegate(Enemy enemy);//defines the header of this delegate type? i think lol

  [SerializeField]
  private Rect Boundary;//the play area

  
	[System.Serializable]
    public class Enemystruct {
      [SerializeField]
      public GameObject EnemyPrefab;
      public float DelayTime;
      public Vector3 SpawnPosition;
      public int maxHealth;
      public int scoreValue;
      public GameObject bullet;
      public float fireRate;
      public int pooledAmount;
      public Enemy enemyObject;
      public PathDelegate path;//hopefully this holds each enemystructs path?
    }

    [System.Serializable]
    public class Wave
    {
      [SerializeField]
      public List<Enemystruct> Enemies;
      public bool IsEvent;
      public float WaveDelay;
    }

    [SerializeField]
    List<Wave> Waves;
	
  //globally accesible bookmark
	public static GameController instance = null;
  
  private int _score;
  //private List<Enemy> _enemies;//i dont think i need this anymore
	
  //enforces this as a singleton, highlander style
	void Awake()
	{
    if(instance == null)
      instance = this;
    else if(instance != this)
      Destroy(gameObject);
	}
	
	void Start()
	{
		//pool all enemies, putting them in their places inactive
    for(int n = 0; n < Waves.Count; n++)
		{
			//iterates through enemies in a single wave
			for(int i = 0; i < Waves[n].Enemies.Count; i++)
			{
				Enemystruct enemy = ((Waves[n]).Enemies[i]);
        enemy.enemyObject = (Instantiate(enemy.EnemyPrefab, enemy.SpawnPosition, Quaternion.identity)).GetComponent<Enemy>();
        enemy.enemyObject.gameObject.SetActive(false);
        //sets the enemy up with its starting values
        enemy.enemyObject.xstart = enemy.SpawnPosition.x;
        enemy.enemyObject.ystart = enemy.SpawnPosition.y;
        enemy.enemyObject.maxHealth = enemy.maxHealth;//i can feel james's judgment as i type these
        enemy.enemyObject.scoreValue = enemy.scoreValue;
        enemy.enemyObject.bullet = enemy.bullet;
        enemy.enemyObject.fireRate = enemy.fireRate;
        enemy.enemyObject.pooledAmount = enemy.pooledAmount;
        //enemy.enemyObject.movePath = enemy.path;
        enemy.enemyObject.movePath = HorizontalSineWave;//testing version to make it simpler
      }
		}
    
    //actually start the waves
		StartCoroutine(RunWaves());
    
    loseText.text = "";
    _score = 0;
    scoreText.text = "Score: " + _score.ToString();
	}
	
  //progresses through the stage
	IEnumerator RunWaves()
	{
		//iterates through the waves
		for(int n = 0; n < Waves.Count; n++)
		{
			//iterates through enemies in a single wave
			for(int i = 0; i < Waves[n].Enemies.Count; i++)
			{
				Enemystruct enemy = ((Waves[n]).Enemies[i]);
				//waits between spawning each enemy
				yield return new WaitForSeconds(enemy.DelayTime);
        enemy.enemyObject.gameObject.SetActive(true);
      }
			//if the wave is a midboss/boss
			while(Waves[n].IsEvent)
			{
				//the code loops until the boss dies before continuing
				if(!(Waves[n].Enemies[0].EnemyPrefab.activeInHierarchy))
				{
					break;
				}
			}
			yield return new WaitForSeconds(Waves[n].WaveDelay);
		}
	} 
  
  
  public Rect getBoundary() {
    return Boundary;
  }
  
  public void AddScore(int scoreToAdd)
  {
    _score += scoreToAdd;
    scoreText.text = "Score: " + _score.ToString();
  }
  
  public void GameOver()
  {
    loseText.text = ("DEAD PARROT");
  }
  
  //all the path types as delegate-ready functions
  
  //just some arbitrary movement till i get this shit to workt
  public void HorizontalSineWave(Enemy enemy) {
    
    //how long the enemy has been alive
    enemy.t = Time.time - enemy.spawnTime;
    
    //preset path
		enemy.xpos = enemy.xstart + 2 * Mathf.Pow(enemy.t, 2);
    enemy.ypos = enemy.ystart + 1 * Mathf.Sin(enemy.t * Mathf.PI * 5);
   
   //moves enemy acoording to path
    enemy.rigidBody2D.MovePosition(new Vector2(enemy.xpos, enemy.ypos));
    
  }
}