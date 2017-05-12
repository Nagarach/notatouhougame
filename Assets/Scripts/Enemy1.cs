using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{	
  public int maxHealth;
  public int scoreValue;
  public GameObject bullet;//this enemies bullet prefab
	public float fireRate;//delay between bullets in seconds
  public int pooledAmount;//how many bullets to be pooled

	private Rect _boundary;
  private GameController _gameController;
  private Rigidbody2D _rigidBody2D;
  private float _spawnTime;
  private float _xstart;
  private float _ystart;
  private float _xpos;
  private float _ypos;
  private float _t;
  private int _currentHealth;
  private float flavorNumber;//for silliness
  private float _nextFire;
  private List<GameObject> _bulletPool;
  
  
	void Start()
	{
    //get a reference to the singleton GameController object
    _gameController = GameController.instance;
    _boundary = _gameController.getBoundary();
    _rigidBody2D = GetComponent<Rigidbody2D>();
    
    _spawnTime = Time.time;
    
    _xstart = _rigidBody2D.position.x;
    _ystart = _rigidBody2D.position.y;
    _xpos = _xstart;
    _ypos = _ystart;
    
    _currentHealth = maxHealth;
    
    //make them bullets
		_bulletPool = new List<GameObject>();
		for(int i = 0; i < pooledAmount; i++)
		{
			GameObject obj = (GameObject)Instantiate(bullet);
			obj.SetActive(false);
			_bulletPool.Add(obj);
		}
    
    flavorNumber = 3 * Random.value;
	}
  
  void Update()
  {    
    //how long the enemy has been alive
    _t = Time.time - _spawnTime;
    
    //preset path
		_xpos = _xstart + 2 * Mathf.Pow(_t, 2);
    _ypos = _ystart + 1 * Mathf.Sin(_t * Mathf.PI * flavorNumber);
   
   //moves enemy acoording to path
    _rigidBody2D.MovePosition(new Vector2(_xpos, _ypos));
    
    //fires a pooled bullet
    if(Time.time > _nextFire)//if(time to fire)
    {
      //search for a bullet to recycle
      for(int i = 0; i < _bulletPool.Count; i++)
      {
        if(!_bulletPool[i].activeInHierarchy)
        {
          //found bullet to recycle
          _nextFire = Time.time + fireRate;//update next fire time
          //reset bullets position
          _bulletPool[i].transform.position = transform.position;
          _bulletPool[i].transform.rotation = transform.rotation;
          _bulletPool[i].SetActive(true);//BANG
          break;
        }
      }
    }
    
    //check to see if object is far outside the play area
    if(_xpos > _boundary.xMax + 5 || 
       _ypos > _boundary.yMax + 5 ||
       _xpos < _boundary.xMin - 5 ||
       _ypos < _boundary.yMin - 5)
    {
     gameObject.SetActive(false);//kys
    }
  }
  
  void OnTriggerEnter2D(Collider2D other)
  {
    if(other.tag == "PlayerShot")
    {
      other.gameObject.SetActive(false);//deactivate the bullet
      _currentHealth--;
      if(_currentHealth <= 0)//if(you dead)
      {
        gameObject.SetActive(false);//DIE
        _gameController.AddScore(scoreValue);
      }
    }
  }
}