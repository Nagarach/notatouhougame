using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

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
	
	void Start()
	{
		//start
		StartCoroutine(RunWaves());
	}
	
	IEnumerator RunWaves()
	{
		//iterates through the waves
		for(int n = 0; n < Waves.Count; n++)
		{
			//iterates through enemies in a single wave
			for(int i = 0; i < Waves[n].Enemies.Count; i++)
			{
				Enemy e = ((Waves[n]).Enemies[i]);
				//waits between spawning each enemy
				yield return new WaitForSeconds(e.DelayTime);
				Instantiate(e.EnemyPrefab,e.SpawnPosition,Quaternion.identity);
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
	
}