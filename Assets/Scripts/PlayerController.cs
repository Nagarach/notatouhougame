using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour
{
	public float playerSpeed;
	public Boundary boundary;
	
	//public GameObject shot;
	//public float fireRate;
	
	//private float nextFire;
	
	void Update()
	{
	/*	if(Input.GetButton("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, transform.position, transform.rotation);
		}
		*/
	}
	
	void FixedUpdate()
	{
		//gets player input
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		
		//moves player
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		Vector3 heading = new Vector3(moveHorizontal, moveVertical, 0.0f);
		rb.velocity = heading * playerSpeed;
		
		//sets play area
		rb.position = new Vector3
		(
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
			0.0f
		);
	}
}
