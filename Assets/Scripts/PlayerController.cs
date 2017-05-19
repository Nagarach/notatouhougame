using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  public int StartLives;//lives the player starts with
  public Text livesText;
	public float PlayerSpeedUnFocused;//how fast the player is unfocused
  public float PlayerSpeedFocused;//HOLD SHIFT FOR FOCUSED MOVEMENT
	public GameObject shot;//player bullet prefab
	public float fireRate;//delay between player shots in seconds
  public int pooledAmount;//how many bullets to be pooled
  
  private GameController _gameController;
	private Rect _boundary;//playable area of the screen
  private int _currentLives;
  private float _nextFire;
  private Rigidbody2D _rigidBody2D;
  private List<GameObject> _shotPool;
	
  void Start()
  {
    //get a reference to the singleton GameController object
     _gameController = GameController.instance;
    
    //set instance variables
    _boundary = _gameController.getBoundary();
    _rigidBody2D = GetComponent<Rigidbody2D>();
    
    _currentLives = StartLives;
    livesText.text = "Lives: " + _currentLives;
    
    //make them bullets
		_shotPool = new List<GameObject>();
		for(int i = 0; i < pooledAmount; i++)
		{
			GameObject obj = (GameObject)Instantiate(shot);
			obj.SetActive(false);
			_shotPool.Add(obj);
		}
  }
	
	void Update()
	{
    //fires a pooled bullet
    if(Input.GetButton("Fire1") && Time.time > _nextFire)//if(time to fire)
    {
      //search for a bullet to recycle
      for(int i = 0; i < _shotPool.Count; i++)
      {
        if(!_shotPool[i].activeInHierarchy)
        {
          //found bullet to recycle
          _nextFire = Time.time + fireRate;//update next fire time
          //reset bullets position
          _shotPool[i].transform.position = transform.position;
          _shotPool[i].transform.rotation = transform.rotation;
          _shotPool[i].SetActive(true);//BANG
          break;
        }
      }
    }
    
		//gets player input for movement
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		
		//moves player
		Vector3 heading = new Vector3(moveHorizontal, moveVertical, 0.0f);
    if(Input.GetButton("Focus"))
    {
      _rigidBody2D.velocity = heading * PlayerSpeedFocused;
    }
    else
    {
      _rigidBody2D.velocity = heading * PlayerSpeedUnFocused;
    }      
		
		//enforces play area
		_rigidBody2D.position = new Vector3
		(
			Mathf.Clamp(_rigidBody2D.position.x, _boundary.xMin, _boundary.xMax),
			Mathf.Clamp(_rigidBody2D.position.y, _boundary.yMin, _boundary.yMax),
			0.0f
		);
	}
  
  void OnTriggerEnter2D(Collider2D other)
  {
    if(other.tag == "EnemyBullet")
    {
      other.gameObject.SetActive(false);//deactivate the bullet
      _currentLives--;
      livesText.text = "Lives: " + _currentLives;
      if(_currentLives <= 0)//if(you dead)
      {
        gameObject.SetActive(false);//DIE
        _gameController.GameOver();
      }
    }
  }
}
