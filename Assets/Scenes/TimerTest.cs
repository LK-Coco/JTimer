using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //var timer = new JTimer.Timer(2f, CallBack);
        //var timer2 = new JTimer.Timer(4f, CallBack);
        //timer.Start();
        //timer2.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Time.timeScale = 2;
        }
        Debug.Log(Time.realtimeSinceStartup + "  " + Time.time );
    }

    void CallBack()
    {
        Debug.Log("Invoke!");
    }
}
