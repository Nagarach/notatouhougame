using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

	private float bulletSpeed;

  private Rigidbody2D _rigidbody2D;
  private Rect _boundary;//playable area of the screen

	void Start()//runs when the object is first created
  {
    //get a reference to the singleton GameController object
    GameController _gameController = GameController.instance;
    
    //set instance variables
    _boundary = _gameController.getBoundary();
    _rigidbody2D = GetComponent<Rigidbody2D>();
  }
  
	void Awake()//runs EACH TIME the bullet is fired/recycled
  {
		bulletSpeed = Random.value * 5 + 3;
	}
	
	void Update() {
    //moves
		_rigidbody2D.velocity = Vector2.down * bulletSpeed;
    
    //checks if the bullet is still in the play area
    float _xpos = _rigidbody2D.position.x;
    float _ypos = _rigidbody2D.position.y;
    if(_xpos > _boundary.xMax + 5 || 
       _ypos > _boundary.yMax + 5 ||
       _xpos < _boundary.xMin - 5 ||
       _ypos < _boundary.yMin - 5)
       {
         gameObject.SetActive(false);//kys
       }
	}
}
