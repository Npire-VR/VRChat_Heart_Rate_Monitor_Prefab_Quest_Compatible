#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using System.Text;
public class GenerateCyclingColorAnimation
{
    private static string GradientToPayload(dynamic gradient, int cycleSpeed, bool questAndHUD = false)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("speed=").Append(cycleSpeed).Append('|');
        foreach (GradientColorKey colorKey in gradient)
        {
            builder.AppendFormat(
                "{0:F6},{1:F6},{2:F6},{3:F6}|",
                colorKey.color.r, colorKey.color.g, colorKey.color.b, colorKey.color.a);
            builder.AppendFormat("{0:F6}|", colorKey.time);
        }
        if (questAndHUD) return "qh_" + builder;
        return builder.ToString();
    }

    // & function to generate animation
    public static AnimationClip Main(dynamic gradient, int cycleSpeed, bool questAndHUD = false) 
    {
        gradient = GradientSetEdges.Main(gradient);

        string generatedFileName = BuildHashName.FromPayload(GradientToPayload(gradient, cycleSpeed, questAndHUD));

        // ~ folder to save anims ins
        string generatedAnimationPath = Config.generatedPath + generatedFileName + ".anim";
        
        // ? reuse if it already exists
        AnimationClip existingAnimation =
            AssetDatabase.LoadAssetAtPath<AnimationClip>(generatedAnimationPath);
        if (existingAnimation != null)
        {
            return existingAnimation;
        }
        
        // ? duplicate the anim
        string animationPath = Config.customAnimationPath;
        if (questAndHUD)
        {
            animationPath = Config.customQuestHUDAnimationPath;
        }
        AssetDatabase.CopyAsset(
            Config.customAnimationPath,
            generatedAnimationPath);
        AssetDatabase.Refresh();

        // ? get newly created anim
        AnimationClip generatedColorAnimation =
            AssetDatabase.LoadAssetAtPath<AnimationClip>(generatedAnimationPath);
        
        // ? get animation curves of clip
        var bindings = AnimationUtility.GetCurveBindings(generatedColorAnimation);
        
        // ? track if first loop
        int first = 0;

        // ? loop threw the gradient keys
        foreach (GradientColorKey colorKey in gradient)
        {
            
            // ^ loop threw and set them to new values for the start of the curve
            foreach (var binding in bindings)
            {
                
                // * get curve/keyframes
                AnimationCurve curve = AnimationUtility.GetEditorCurve(generatedColorAnimation, binding);
                Keyframe[] keys = curve.keys;
                
                // * set relevant color value for key
                if (first == 0)
                {
                    switch (binding.propertyName)
                    {
                        case "material._Color.r":
                            keys[0].value = colorKey.color.r;
                            break;
                        case "material._Color.g":
                            keys[0].value = colorKey.color.g;
                            break;
                        case "material._Color.b":
                            keys[0].value = colorKey.color.b;
                            break;
                        case "material._Color.a":
                            keys[0].value = colorKey.color.a;
                            break;
                    }
                    
                    // * set curve to new value
                    curve.keys = keys;
                    
                }
                else
                {
                    // * create new key if not first
                    Keyframe newKey = keys[0];
                    newKey.time = (float)(Math.Round(colorKey.time * cycleSpeed));

                    // * set key value 
                    switch (binding.propertyName)
                    {
                        case "material._Color.r":
                            newKey.value = colorKey.color.r;
                            break;
                        case "material._Color.g":
                            newKey.value = colorKey.color.g;
                            break;
                        case "material._Color.b":
                            newKey.value = colorKey.color.b;
                            break;
                        case "material._Color.a":
                            newKey.value = colorKey.color.a;
                            break;
                        case "material._EmissionColor.r":
                            newKey.value = colorKey.color.r;
                            break;
                        case "material._EmissionColor.g":
                            newKey.value = colorKey.color.g;
                            break;
                        case "material._EmissionColor.b":
                            newKey.value = colorKey.color.b;
                            break;
                        case "material._EmissionColor.a":
                            newKey.value = colorKey.color.a;
                            break;
                    }

                    // * add key to curve
                    curve.AddKey(newKey);
                    
                }
                
                // ^ save curve
                AnimationUtility.SetEditorCurve(generatedColorAnimation, binding, curve);
                
            }

            // ^ exit first loop
            first += 1;
            
        }

        // ~ save changes 
        EditorUtility.SetDirty(generatedColorAnimation);
        AssetDatabase.SaveAssets();

        // ~ return animation
        return generatedColorAnimation;

    }
    
}

#endif