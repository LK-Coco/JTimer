using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    private int num = 0;
    // Start is called before the first frame update
    void Start()
    {
        var timer = new JTimer.Timer(2,true,false,0,null, CallBack);
        //var timer2 = new JTimer.Timer(4f, CallBack);
        timer.Start();
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            //JTimer.Timer.Delay(2f, true, () => Debug.Log($"Invoke{num}")).Start();
            JTimer.Timer.Repeat(2, true, 3, () => Debug.Log($"Invoke{num}")).OnUpdated(t => Debug.Log($"upddate{t}")).Start(); 
            num++;
        }
        //Debug.Log(Time.realtimeSinceStartup + "  " + Time.time );
    }

    void CallBack()
    {
        Debug.Log("Invoke!");
    }
}
