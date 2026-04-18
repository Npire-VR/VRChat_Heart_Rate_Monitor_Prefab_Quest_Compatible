#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
public class GenerateColorAnimation
{
    private static string ColorToPayload(Color color, bool questAndHUD = false)
    {
        if (questAndHUD) return $"qh_{color.r:F6}|{color.g:F6}|{color.b:F6}|{color.a:F6}";
        return $"{color.r:F6}|{color.g:F6}|{color.b:F6}|{color.a:F6}";
    }

    // & function to generate animation
    public static AnimationClip Main(Color animationColor, bool questAndHUD = false) 
    {
        string generatedFileName = BuildHashName.FromPayload(ColorToPayload(animationColor, questAndHUD));

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
            animationPath,
            generatedAnimationPath);
        AssetDatabase.Refresh();

        // ? get newly created anim
        AnimationClip generatedColorAnimation =
            AssetDatabase.LoadAssetAtPath<AnimationClip>(generatedAnimationPath);

        // ? get animation curves of clip
        var bindings = AnimationUtility.GetCurveBindings(generatedColorAnimation);
        
        // ? loop threw and set them to new values
        foreach (var binding in bindings)
        {

            // ^ get curve/keyframes
            AnimationCurve curve = AnimationUtility.GetEditorCurve(generatedColorAnimation, binding);
            Keyframe[] keys = curve.keys;

            // ^ set relevant value for key
            switch(binding.propertyName) 
            {   
                case "material._Color.r":
                    keys[0].value = animationColor.r;
                    break;
                case "material._Color.g":
                    keys[0].value = animationColor.g;
                    break;
                case "material._Color.b":
                    keys[0].value = animationColor.b;
                    break;
                case "material._Color.a":
                    keys[0].value = animationColor.a;
                    break;
                case "material._EmissionColor.r":
                    keys[0].value = animationColor.r;
                    break;
                case "material._EmissionColor.g":
                    keys[0].value = animationColor.g;
                    break;
                case "material._EmissionColor.b":
                    keys[0].value = animationColor.b;
                    break;
                case "material._EmissionColor.a":
                    keys[0].value = animationColor.a;
                    break;
            }
            
            // ^ set curve to new value
            curve.keys = keys;
            AnimationUtility.SetEditorCurve(generatedColorAnimation, binding, curve);

        }

        // ? save changes 
        EditorUtility.SetDirty(generatedColorAnimation);
        AssetDatabase.SaveAssets();

        // ~ return animation
        return generatedColorAnimation;

    }
    
}

#endif