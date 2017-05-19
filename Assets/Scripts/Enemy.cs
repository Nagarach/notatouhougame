using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{	
  //public delegate void PathDelegate();//your guess is as good as mine lol

  public float xstart;
  public float ystart;
  public int maxHealth;
  public int scoreValue;
  public GameObject bullet;//this enemies bullet prefab
	public float fireRate;//delay between bullets in seconds
  public int pooledAmount;//how many bullets to be pooled
  public GameController.PathDelegate movePath;//defines the header of this delegate type? i think lol

	public Rect boundary;
  public GameController gameController;
  public Rigidbody2D rigidBody2D;
  public float spawnTime;
  public float xpos;
  public float ypos;
  public float t;
  public int currentHealth;
  public float nextFire;
  public List<GameObject> bulletPool;
  
  
	void Start()
	{
    //get a reference to the singleton GameController object
    gameController = GameController.instance;
    boundary = gameController.getBoundary();
    rigidBody2D = GetComponent<Rigidbody2D>();
    
    spawnTime = Time.time;
    
    xpos = xstart;
    ypos = ystart;
    
    currentHealth = maxHealth;
    
    //make them bullets
		bulletPool = new List<GameObject>();
		for(int i = 0; i < pooledAmount; i++)
		{
			GameObject obj = (GameObject)Instantiate(bullet);
			obj.SetActive(false);
			bulletPool.Add(obj);
		}
	}
  
  void Update()
  {    
    
    movePath(this);
    
    //fires a pooled bullet
    if(Time.time > nextFire)//if(time to fire)
    {
      //search for a bullet to recycle
      for(int i = 0; i < bulletPool.Count; i++)
      {
        if(!bulletPool[i].activeInHierarchy)
        {
          //found bullet to recycle
          nextFire = Time.time + fireRate;//update next fire time
          //reset bullets position
          bulletPool[i].transform.position = transform.position;
          bulletPool[i].transform.rotation = transform.rotation;
          bulletPool[i].SetActive(true);//BANG
          break;
        }
      }
    }
    
    
    //check to see if object is far outside the play area
    if(xpos > boundary.xMax + 5 || 
       ypos > boundary.yMax + 5 ||
       xpos < boundary.xMin - 5 ||
       ypos < boundary.yMin - 5)
    {
     gameObject.SetActive(false);//kys
    }
  }
  
  void OnTriggerEnter2D(Collider2D other)
  {
    if(other.tag == "PlayerShot")
    {
      other.gameObject.SetActive(false);//deactivate the bullet
      currentHealth--;
      if(currentHealth <= 0)//if(you dead)
      {
        gameObject.SetActive(false);//DIE
        gameController.AddScore(scoreValue);
      }
    }
  }
}