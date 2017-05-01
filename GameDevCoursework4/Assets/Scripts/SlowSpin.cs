using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowSpin : MonoBehaviour
{

    private float _rotateSpeed = 1;

    private WorldTime _worldTime;

    public bool OscillateX = false;
    public float minX = 1000;
    public float maxX = 2000;


	// Use this for initialization
	void Start ()
	{
	    _worldTime = GameObject.Find("Time").GetComponent<WorldTime>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _rotateSpeed = _worldTime.Speed;
        transform.Rotate(0,Time.deltaTime * _rotateSpeed, 0);

	    if (OscillateX)
	    {
	        // Vector3 position = transform.localPosition;
	        // position.x = (Mathf.PingPong(_worldTime.TimeSinceCreation, maxX + minX) + minX);
	        // transform.localPosition = position;
	    }
	}
}
