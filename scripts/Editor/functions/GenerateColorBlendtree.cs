#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Text;

public class GenerateColorBlendtree
{
    private static string GradientToPayload(dynamic gradient, bool questAndHUD = false)
    {
        StringBuilder builder = new StringBuilder();
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
    public static BlendTree Main(dynamic gradient, string paramType, bool questAndHUD = false) 
    {
        gradient = GradientSetEdges.Main(gradient);

        string generatedFileName = BuildHashName.FromPayload(GradientToPayload(gradient, questAndHUD));

        // ~ path to save blend tree
        string generatedBlendtreePath = Config.generatedPath + generatedFileName + ".asset";
        
        // ~ return existing blendtree if present
        BlendTree existingBlendTree = AssetDatabase.LoadAssetAtPath<BlendTree>(generatedBlendtreePath);
        if (existingBlendTree != null)
        {
            return existingBlendTree;
        }
        
        // ~ duplicate the existing blendtree
        AssetDatabase.CopyAsset(
            Config.customBlendtreePath,
            generatedBlendtreePath);
        AssetDatabase.Refresh();

        // ~ get newly created blendtree
        BlendTree generatedBlendTree =
            AssetDatabase.LoadAssetAtPath<BlendTree>(generatedBlendtreePath);
        
        // ~ create animations and add them to the blend tree
        foreach (GradientColorKey colorKey in gradient) 
        {
            
            // ? create new animation clip
            string treeAnimationFileName = Guid.NewGuid().ToString("n").Substring(0, 16);
            AnimationClip treeAnimation = GenerateColorAnimation.Main(colorKey.color, questAndHUD);
            
            // ? set threshold for blendtree based on parm type
            float blendtreeThreshold = colorKey.time;
            // ^ int
            if (paramType == "int")
            {
                blendtreeThreshold *= 255;
                blendtreeThreshold = (int)blendtreeThreshold;
            }
            // ^ assume default to float
            else
            {
                // * convert from 0-1 to -1 to 1
                blendtreeThreshold = blendtreeThreshold * 2f - 1f;
            }
            
            // ? add to blendtree
            generatedBlendTree.AddChild(treeAnimation, blendtreeThreshold);

        }
           
        // ~ save asset
        EditorUtility.SetDirty(generatedBlendTree);
        AssetDatabase.SaveAssets();
        
        // ~ return blendtree
        return generatedBlendTree;
        
    }
    
}

#endif