using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, yMin, yMax;
}

public class Mover : MonoBehaviour
{
	public float speed;
	
	
	void Start()
	{
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		Vector3 heading = new Vector3(5, 0.0f, 0.0f);
		rb.velocity = heading;
		
		rb.position = new Vector3
		(
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
			0.0f
		);
	}
}
