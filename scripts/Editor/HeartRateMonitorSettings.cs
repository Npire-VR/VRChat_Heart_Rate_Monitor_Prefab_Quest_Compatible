#if UNITY_EDITOR

using UnityEngine;

public class HeartRateMonitorSettings : MonoBehaviour
{

    // & platform
    public PlatformOption platform = PlatformOption.PC;
    public enum PlatformOption
    {
        PC,
        Quest
    }
    
    // & color fields
    public Color color = Color.white;
    public Color colorOutline = Color.white;
    public Color colorMonitor = Color.white;
    public Color colorMonitorOff = new Color(0.5f, 0.5f, 0.5f);
    
    public Gradient gradient = new Gradient();
    public Gradient gradientOutline = new Gradient();
    public Gradient gradientMonitor = new Gradient();
    public Gradient gradientMonitorOff = new Gradient();
    
    public Gradient cyclingGradient = new Gradient();
    public Gradient cyclingGradientOutline = new Gradient();
    public Gradient cyclingGradientMonitor = new Gradient();
    public Gradient cyclingGradientMonitorOff = new Gradient();
    
    // & cycle speed for cycling
    public int cycleSpeed = 60;
    public int cycleSpeedOutline = 60;
    public int cycleSpeedMonitor = 60;
    public int cycleSpeedMonitorOff = 60;

    // & monitor style options
    public StyleOption style = StyleOption.Text;
    public FontOption font = FontOption.Default;
    public ColorOption colorType = ColorOption.GradientByHeartrate;
    public ColorOptionOutline colorTypeOutline = ColorOptionOutline.SolidColor;
    public ColorOptionMonitor colorTypeMonitor = ColorOptionMonitor.SolidColor;
    public ColorOptionMonitorOff colorTypeMonitorOff = ColorOptionMonitorOff.SolidColor;

    public enum FontOption
    {
        Default,
        Sans,
        Serif,
        Square,
        SevenSegment,
        Minecraft,
        Barcode
    }
    public enum StyleOption
    {
        Text,
        OutlinedText,
        Heart,
        ScreenHeart,
        ScreenSquare,
        HUD
    }
    public enum ColorOption
    {
        SolidColor,
        GradientByHeartrate,
        CyclingGradient
    }
    public enum ColorOptionOutline
    {
        SolidColor,
        GradientByHeartrate,
        CyclingGradient
    }
    public enum ColorOptionMonitor
    {
        SolidColor,
        GradientByHeartrate,
        CyclingGradient,
    }
    
    public enum ColorOptionMonitorOff
    {
        SolidColor,
        GradientByHeartrate,
        CyclingGradient,
        HideMonitor
    }
    
    // & toggle checkboxes
    public bool autoTurnOff = true;
    public bool toggleOnOff = false;
    public bool toggleAct = false;
    public bool toggleHUD = false;
    
    void Reset()
    {
        
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1f, 0f);
        alphaKeys[1] = new GradientAlphaKey(1f, 1f);
        
        // & default gradient
        GradientColorKey[] colorKeys = new GradientColorKey[3];
        colorKeys[0] = new GradientColorKey(Color.white, 0.394f);
        colorKeys[1] = new GradientColorKey(new Color(1f, 1f, 0.333f), 0.433f);
        colorKeys[2] = new GradientColorKey(new Color(1f, 0.298f, 0.298f), 0.587f);
        
        gradient.SetKeys(colorKeys, alphaKeys);
        gradientOutline.SetKeys(colorKeys, alphaKeys);
        gradientMonitor.SetKeys(colorKeys, alphaKeys);
        gradientMonitorOff.SetKeys(colorKeys, alphaKeys);
        
        // & default cycling gradient
        float keyframeDivider = 0.16666666f;
        GradientColorKey[] colorKeysCycling = new GradientColorKey[7];
        // colorKeysCycling[0] = new GradientColorKey(new Color(1,0,0), 0);
        colorKeysCycling[0] = new GradientColorKey(new Color(0.9666f,0.1098f,0.1098f),0);
        // colorKeysCycling[1] = new GradientColorKey(new Color(0.988f,0.207f,0.0117f),1 * keyframeDivider);
        // colorKeysCycling[2] = new GradientColorKey(new Color(0.9725f,0.4054f,0.02745f),2 * keyframeDivider);
        // colorKeysCycling[3] = new GradientColorKey(new Color(0.9529f,0.5905f,0.0470f),3 * keyframeDivider);
        // colorKeysCycling[4] = new GradientColorKey(new Color(0.9294f,0.7576f,0.0705f),4 * keyframeDivider);
        colorKeysCycling[1] = new GradientColorKey(new Color(0.9019f,0.9019f, 0.098f),keyframeDivider);
        // colorKeysCycling[6] = new GradientColorKey(new Color(0.7639f,0.9215f,0.1333f),6 * keyframeDivider);
        // colorKeysCycling[7] = new GradientColorKey(new Color(0.6266f,0.9294f,0.1725f),7 * keyframeDivider);
        // colorKeysCycling[8] = new GradientColorKey(new Color(0.5051f,0.9333f,0.2196f),8 * keyframeDivider);
        // colorKeysCycling[9] = new GradientColorKey(new Color(0.3623f,0.9333f,0.2196f),9 * keyframeDivider);
        colorKeysCycling[2] = new GradientColorKey(new Color(0.2196f,0.9333f,0.2196f),2 * keyframeDivider);
        // colorKeysCycling[11] = new GradientColorKey(new Color(0.2196f,0.9333f,0.3623f),11 * keyframeDivider);
        // colorKeysCycling[12] = new GradientColorKey(new Color(0.2196f,0.9333f,0.5051f),12 * keyframeDivider);
        // colorKeysCycling[13] = new GradientColorKey(new Color(0.2196f,0.9333f,0.6478f),13 * keyframeDivider);
        // colorKeysCycling[14] = new GradientColorKey(new Color(0.2196f,0.9333f,0.7905f),14 * keyframeDivider);
        colorKeysCycling[3] = new GradientColorKey(new Color(0.2196f,0.9333f,0.9333f),3 * keyframeDivider);
        // colorKeysCycling[16] = new GradientColorKey(new Color(0.2196f,0.7905f,0.9333f),16 * keyframeDivider);
        // colorKeysCycling[17] = new GradientColorKey(new Color(0.2196f,0.6478f,0.9333f),17 * keyframeDivider);
        // colorKeysCycling[18] = new GradientColorKey(new Color(0.2196f,0.5051f,0.9333f),18 * keyframeDivider);
        // colorKeysCycling[19] = new GradientColorKey(new Color(0.2196f,0.3623f,0.9333f),19 * keyframeDivider);
        colorKeysCycling[4] = new GradientColorKey(new Color(0.2196f,0.2196f,0.9333f),4 * keyframeDivider);
        // colorKeysCycling[21] = new GradientColorKey(new Color(0.3623f,0.2196f,0.9333f),21 * keyframeDivider);
        // colorKeysCycling[22] = new GradientColorKey(new Color(0.5051f,0.2196f,0.9333f),22 * keyframeDivider);
        // colorKeysCycling[23] = new GradientColorKey(new Color(0.6478f,0.2196f,0.9333f),23 * keyframeDivider);
        // colorKeysCycling[24] = new GradientColorKey(new Color(0.7905f,0.2196f,0.9333f),24 * keyframeDivider);
        colorKeysCycling[5] = new GradientColorKey(new Color(0.9333f,0.2196f,0.9333f),5 * keyframeDivider);
        // colorKeysCycling[26] = new GradientColorKey(new Color(0.9333f,0.2196f,0.7905f),26 * keyframeDivider);
        // colorKeysCycling[27] = new GradientColorKey(new Color(0.9333f,0.2196f,0.6478f),27 * keyframeDivider);
        // colorKeysCycling[28] = new GradientColorKey(new Color(0.9333f,0.2196f,0.5051f),28 * keyframeDivider);
        // colorKeysCycling[29] = new GradientColorKey(new Color(0.9333f,0.2196f,0.3623f),29 * keyframeDivider);
        colorKeysCycling[6] = new GradientColorKey(new Color(0.9666f,0.1098f,0.1098f),1);
        // colorKeysCycling[30] = new GradientColorKey(new Color(0.9333f,0.2196f,0.2196f),1);

        cyclingGradient.SetKeys(colorKeysCycling, alphaKeys);
        cyclingGradientOutline.SetKeys(colorKeysCycling, alphaKeys);
        cyclingGradientMonitor.SetKeys(colorKeysCycling, alphaKeys);
        cyclingGradientMonitorOff.SetKeys(colorKeysCycling, alphaKeys);
        
    }
    
}

#endif