using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

  public static BulletController instance = null;
  public int BulletResevoirSize;
  public GameObject Bullet;

  private List<GameObject> _bulletResevoir;
  
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
    //creates a vast resevoir of bullets
		_bulletResevoir = new List<GameObject>();
		for(int i = 0; i < BulletResevoirSize; i++)
		{
			GameObject obj = (GameObject)Instantiate(Bullet);
			obj.SetActive(false);
			_bulletResevoir.Add(obj);
		}
  }
  
	void Update()
  {
		/*
    for(iterates through all active bullets)
      if(bullets parent is a certain kind of enemy)
        bullet(i).heading = someshit
        bullet(i).speed = someothershit
      else if(bullets parent is some other type of enemy)
        ....
      else...
    */
	}
}