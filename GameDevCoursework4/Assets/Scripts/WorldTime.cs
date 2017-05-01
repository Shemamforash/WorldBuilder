using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldTime : MonoBehaviour
{

    public float Speed;
    public float TimeSinceCreation = 0;
    public float TimePlayed = 0;

    private Slider _timeSlider;
    private Text _timeText;

    // Use this for initialization
    void Start () {
        _timeSlider = GameObject.Find("TimeSlider").GetComponent<Slider>();
        _timeText = GameObject.Find("TimeText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        Speed = _timeSlider.value;
        _timeText.text = "Time " + Mathf.Round(Speed) + "x";
	    TimeSinceCreation += Time.deltaTime * Speed;
	    TimePlayed += Time.deltaTime;
	}

    public void ResetTime()
    {
        TimeSinceCreation = 0;
    }
}
