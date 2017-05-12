using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

  public Text scoreText;
  public Text loseText;

  [SerializeField]
  private Rect Boundary;//the play area

  
	[System.Serializable]
    public class Enemy {
        [SerializeField]
        public GameObject EnemyPrefab;
        public float DelayTime;
        public Vector3 SpawnPosition;
    }

    [System.Serializable]
    public class Wave
    {
        [SerializeField]
        public List<Enemy> Enemies;
		public bool IsEvent;
		public float WaveDelay;
    }

    [SerializeField]
    List<Wave> Waves;
	
  //globally accesible bookmark
	public static GameController instance = null;
  
  private int _score;
	
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
		//start
		StartCoroutine(RunWaves());
    
    loseText.text = "";
    _score = 0;
    scoreText.text = "Score: " + _score.ToString();
	}
	
	IEnumerator RunWaves()
	{
		//iterates through the waves
		for(int n = 0; n < Waves.Count; n++)
		{
			//iterates through enemies in a single wave
			for(int i = 0; i < Waves[n].Enemies.Count; i++)
			{
				Enemy _enemy = ((Waves[n]).Enemies[i]);
				//waits between spawning each enemy
				yield return new WaitForSeconds(_enemy.DelayTime);
				Instantiate(_enemy.EnemyPrefab,_enemy.SpawnPosition,Quaternion.identity);
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
}