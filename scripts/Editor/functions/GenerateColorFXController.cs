#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class GenerateColorFXController
{
    
    private static string MotionToPayload(dynamic generatedAnimationColor, dynamic generatedAnimationOff)
    {
        string colorName = generatedAnimationColor != null ? generatedAnimationColor.name : "null";
        string offName = generatedAnimationOff != null ? generatedAnimationOff.name : "null";
        return $"{colorName}|{offName}";
    }

    // & function to generate fx controller for color
    public static AnimatorController Main(string fileNamePrefix, string controllerPath, dynamic generatedAnimationColor, dynamic generatedAnimationOff)
    {
        string generatedFileName = BuildHashName.FromPayload(MotionToPayload(generatedAnimationColor, generatedAnimationOff));

        // ~ folder to controllers in
        string generatedFXControllerPath = Config.generatedPath + fileNamePrefix + generatedFileName + ".controller";

        // ~ reuse if it already exists
        AnimatorController existingController =
            AssetDatabase.LoadAssetAtPath<AnimatorController>(generatedFXControllerPath);
        if (existingController != null)
        {
            return existingController;
        }

        // ~ duplicate the controller
        AssetDatabase.CopyAsset(
            controllerPath,
            generatedFXControllerPath);
        AssetDatabase.Refresh();
        
        // ~ replace color state in controller
        // ? load the generated controller
        AnimatorController generatedController =
            AssetDatabase.LoadAssetAtPath<AnimatorController>(generatedFXControllerPath);
        
        // ? get the first layer of the controller to edit
        AnimatorControllerLayer firstLayer = generatedController.layers[0];
            
        // ? go threw all the states to find the right one
        foreach (var state in firstLayer.stateMachine.states)
        {
            // ^ replace the two animations
            switch (state.state.name)
            {
                case "custom":
                    state.state.motion = generatedAnimationColor;
                    break;
                case "off":
                    state.state.motion = generatedAnimationOff;
                    break;
            }
        }

        // ~ return completed FX controller
        return generatedController;

    }
    
}

#endif
