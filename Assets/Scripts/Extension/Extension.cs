using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extension {
    
	public static string ConvertToDigitalClockFormat(int time)
    {
        int mm = 0;
        int ss = 0;
        if (time >= 60)
        {
            mm = time / 60;
            ss = time % 60;
        }
        else
        {
            ss = time;
        }
        return string.Format("{0}:{1}", FillZero(mm), FillZero(ss));
    }

    private static string FillZero(int time)
    {
        return string.Format("{0}{1}", (time.ToString().Length < 2) ? "0" : "", time);
    }

}
