#if UNITY_EDITOR

using System;
using System.Drawing;
using UnityEngine;

public class GradientSetEdges
{
    
    // & adds 0 time and 1 time values to gradient input if it doesn't already have them
    public static Array Main(Gradient gradient)
    {

        // ~ determine how much extra length we needed
        int extraLength = 0;
        if (gradient.colorKeys[0].time != 0)
        {
            extraLength += 1;
        }

        if (gradient.colorKeys[gradient.colorKeys.Length - 1].time != 1)
        {
            extraLength += 1;
        }

        // ~ create array to hold color keys if needed
        if (extraLength > 0)
        { 
            GradientColorKey[] colorKeys = new GradientColorKey[gradient.colorKeys.Length + extraLength];   

            // ? add new keys as needed
            // ^ starting key
            int loopStart = 0;
            if (gradient.colorKeys[0].time != 0)
            {
                colorKeys[0] = gradient.colorKeys[0];
                colorKeys[0].time = 0;
                // * set loop start for if not first of array anymore
                loopStart = 1;
            }
            
            // ^ ending key
            int endOffset = 0;
            if (gradient.colorKeys[gradient.colorKeys.Length - 1].time != 1)
            {
                colorKeys[colorKeys.Length - 1] = gradient.colorKeys[gradient.colorKeys.Length - 1];
                colorKeys[colorKeys.Length - 1].time = 1;
                // * set loop start for if not first of array anymore
                endOffset = 1;
            }
            
            // ^ add original keys
            for (int i = loopStart; i < colorKeys.Length - endOffset; i++)
            {
                colorKeys[i] = gradient.colorKeys[i - loopStart];
            }
            
            // ~ return array
            return colorKeys;
            
        }
        // ~ return array if not needed 
        else
        {
            return gradient.colorKeys;
        }
        
    }
    
}

#endif