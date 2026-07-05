#if UNITY_EDITOR

using com.vrcfury.api;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.ScriptableObjects;

[CustomEditor(typeof(HeartRateMonitorSettings))]
public class HeartRateMonitorSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        // & set script
        HeartRateMonitorSettings script = (HeartRateMonitorSettings)target;
        
        // & set fields
        // ~ Basic settings
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Basic Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        script.platform = (HeartRateMonitorSettings.PlatformOption)EditorGUILayout.EnumPopup("Platform", script.platform);
        script.style = (HeartRateMonitorSettings.StyleOption)EditorGUILayout.EnumPopup("Style", script.style);
        script.font = (HeartRateMonitorSettings.FontOption)EditorGUILayout.EnumPopup("Font", script.font);
        script.autoTurnOff = EditorGUILayout.Toggle("Turn Off Automatically", script.autoTurnOff);

        // ~ Color options
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // ? show text color option always
        script.colorType = (HeartRateMonitorSettings.ColorOption)EditorGUILayout.EnumPopup("Text Color Type", script.colorType);
        // ^ show relevant type for selected option
        switch (script.colorType)
        {
            case HeartRateMonitorSettings.ColorOption.SolidColor:
                script.color = EditorGUILayout.ColorField("Text Color", script.color);
                break;        
            case HeartRateMonitorSettings.ColorOption.GradientByHeartrate:
                script.gradient = EditorGUILayout.GradientField("Gradient By Heartrate", script.gradient);
                break;
            case HeartRateMonitorSettings.ColorOption.CyclingGradient: 
                script.cyclingGradient = EditorGUILayout.GradientField("Cycling Gradient", script.cyclingGradient);
                script.cycleSpeed = EditorGUILayout.IntSlider("Cycle Speed", script.cycleSpeed, 0, 120);
                break;
        }
        
        // ? show outline color if necessary
        if (script.style == HeartRateMonitorSettings.StyleOption.OutlinedText)
        {
            EditorGUILayout.Space();
            script.colorTypeOutline = (HeartRateMonitorSettings.ColorOptionOutline)EditorGUILayout.EnumPopup("Outline Color Type", script.colorTypeOutline);
            // ^ show relevant type for selected option
            switch (script.colorTypeOutline)
            {
                case HeartRateMonitorSettings.ColorOptionOutline.SolidColor:
                    script.colorOutline = EditorGUILayout.ColorField("Outline Color", script.colorOutline);
                    break;        
                case HeartRateMonitorSettings.ColorOptionOutline.GradientByHeartrate:
                    script.gradientOutline = EditorGUILayout.GradientField("Gradient By Heartrate", script.gradientOutline);
                    break;
                case HeartRateMonitorSettings.ColorOptionOutline.CyclingGradient: 
                    script.cyclingGradientOutline = EditorGUILayout.GradientField("Cycling Gradient", script.cyclingGradientOutline);
                    script.cycleSpeedOutline = EditorGUILayout.IntSlider("Cycle Speed", script.cycleSpeedOutline, 0, 120);
                    break;
            }
        }
        
        // ? show monitor color if necessary
        if (script.style == HeartRateMonitorSettings.StyleOption.Heart ||
            script.style.ToString().StartsWith("Screen") ||  
            script.style == HeartRateMonitorSettings.StyleOption.HUD)
        {
            // ^ monitor color while on
            EditorGUILayout.Space();
            script.colorTypeMonitor = (HeartRateMonitorSettings.ColorOptionMonitor)EditorGUILayout.EnumPopup("Monitor Color Type", script.colorTypeMonitor);
            // ^ show relevant type for selected option
            switch (script.colorTypeMonitor)
            {
                case HeartRateMonitorSettings.ColorOptionMonitor.SolidColor:
                    script.colorMonitor = EditorGUILayout.ColorField("Color", script.colorMonitor);
                    break;        
                case HeartRateMonitorSettings.ColorOptionMonitor.GradientByHeartrate:
                    script.gradientMonitor = EditorGUILayout.GradientField("Gradient By Heartrate", script.gradientMonitor);
                    break;
                case HeartRateMonitorSettings.ColorOptionMonitor.CyclingGradient: 
                    script.cyclingGradientMonitor = EditorGUILayout.GradientField("Cycling Gradient", script.cyclingGradientMonitor);
                    script.cycleSpeedMonitor = EditorGUILayout.IntSlider("Cycle Speed", script.cycleSpeedMonitor, 0, 120);
                    break;
            }
            
            // ^ monitor color while off (exclude for hud)
            if (script.style != HeartRateMonitorSettings.StyleOption.HUD)
            {
                EditorGUILayout.Space();
                script.colorTypeMonitorOff = (HeartRateMonitorSettings.ColorOptionMonitorOff)EditorGUILayout.EnumPopup("When Monitor Off Color Type", script.colorTypeMonitorOff);
                // ^ show relevant type for selected option
                switch (script.colorTypeMonitorOff)
                {
                    case HeartRateMonitorSettings.ColorOptionMonitorOff.SolidColor:
                        script.colorMonitorOff = EditorGUILayout.ColorField("Color", script.colorMonitorOff);
                        break;        
                    case HeartRateMonitorSettings.ColorOptionMonitorOff.GradientByHeartrate:
                        script.gradientMonitorOff = EditorGUILayout.GradientField("Gradient By Heartrate", script.gradientMonitorOff);
                        break;
                    case HeartRateMonitorSettings.ColorOptionMonitorOff.CyclingGradient: 
                        script.cyclingGradientMonitorOff = EditorGUILayout.GradientField("Cycling Gradient", script.cyclingGradientMonitorOff);
                        script.cycleSpeedMonitorOff = EditorGUILayout.IntSlider("Cycle Speed", script.cycleSpeedMonitorOff, 0, 120);
                        break;
                }
            }
        }
        
        // & menu toggles
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Menu toggles", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        script.toggleOnOff = EditorGUILayout.Toggle("On/off (+1 synced bits)", script.toggleOnOff);
        script.toggleAct = EditorGUILayout.Toggle("Act Dead (+1 synced bits)", script.toggleAct);
        if (script.style == HeartRateMonitorSettings.StyleOption.HUD) {
            script.toggleHUD = EditorGUILayout.Toggle("Hud (local, +0 synced bits)", script.toggleHUD);
        } else {
            script.toggleHUD = false;
        }
        
        // & button to run configuration based on settings
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Configure"))
        {
            
            // ~ call configuration code
            ConfigureButton();
            
        }
        
    }
    
    // & function for the configuration button
    private void ConfigureButton()
    {
        
        // ~ set script
        HeartRateMonitorSettings script = (HeartRateMonitorSettings)target;
        
        // ~ get default off animation
        AnimationClip defaultOffAnimation =
            AssetDatabase.LoadAssetAtPath<AnimationClip>(Config.offAnimationPath);
        
        // ~ create empty game object for monitor
        GameObject containerObject = ObjectFactory.CreateGameObject("Heartrate Monitor");
        GameObjectUtility.SetParentAndAlign(containerObject, Selection.activeGameObject);
        
        // ~ create the FX/animation layers for the different objects
        // ? set hud path addons
        string extraHUDFXname = "";
        if (script.style == HeartRateMonitorSettings.StyleOption.HUD)
        {
            extraHUDFXname = "_hud";

        }
        
        // ? set prefix for auto off and HUD if needed
        string fileNamePrefix = "";
        if (script.autoTurnOff)
        {
            fileNamePrefix = "ao";
        }

        fileNamePrefix += extraHUDFXname;
        
        if (script.style == HeartRateMonitorSettings.StyleOption.HUD &&
            script.platform == HeartRateMonitorSettings.PlatformOption.Quest)
        {
            fileNamePrefix += "_quest";
        }

        fileNamePrefix += "_";
        
        // ? set quest + hud if needed for animation
        bool questAndHUD = false;
        if (script.platform == HeartRateMonitorSettings.PlatformOption.Quest &&
            script.style == HeartRateMonitorSettings.StyleOption.HUD)
        {
            questAndHUD = true;
        }
        
        // ? set FX copy path
        string fxControllerToCopyPath;
        string numberControllerPath;
        if (script.autoTurnOff)
        {
            fxControllerToCopyPath = Config.customFXControllerAutoOffPath;
            numberControllerPath = Config.numberControllerAutoOffPath;
        }
        else
        {
            fxControllerToCopyPath = Config.customFXControllerPath;
            numberControllerPath = Config.numberControllerPath;
        }

        fxControllerToCopyPath += extraHUDFXname + ".controller";
        numberControllerPath += extraHUDFXname + ".controller";
        
        // ? generate for text
        // why does the IDE have to yell at me if I leave it empty, it's fine... it's never going to escape the switch case *sigh*
        dynamic generatedTextMotion = new AnimationClip();
        switch(script.colorType)
        {
            case HeartRateMonitorSettings.ColorOption.SolidColor:
                generatedTextMotion = GenerateColorAnimation.Main(script.color, questAndHUD);
                break;
            case HeartRateMonitorSettings.ColorOption.GradientByHeartrate:
                generatedTextMotion = GenerateColorBlendtree.Main(script.gradient, "float", questAndHUD);
                break;
            case HeartRateMonitorSettings.ColorOption.CyclingGradient:
                generatedTextMotion = GenerateCyclingColorAnimation.Main(script.cyclingGradient, script.cycleSpeed, questAndHUD);
                break;
        }
        AnimatorController generatedTextFXController = GenerateColorFXController.Main(fileNamePrefix, fxControllerToCopyPath, generatedTextMotion, defaultOffAnimation);

        // ? generate for outline if needed
        AnimatorController generatedOutlineTextFXController = new AnimatorController();
        if (script.style == HeartRateMonitorSettings.StyleOption.OutlinedText)
        {
            dynamic generatedOutlineTextMotion = new AnimationClip();
            switch(script.colorTypeOutline)
            {
                case HeartRateMonitorSettings.ColorOptionOutline.SolidColor:
                    generatedOutlineTextMotion = GenerateColorAnimation.Main(script.colorOutline, questAndHUD);
                    break;
                case HeartRateMonitorSettings.ColorOptionOutline.GradientByHeartrate:
                    generatedOutlineTextMotion = GenerateColorBlendtree.Main(script.gradientOutline, "float", questAndHUD);
                    break;
                case HeartRateMonitorSettings.ColorOptionOutline.CyclingGradient:
                    generatedOutlineTextMotion = GenerateCyclingColorAnimation.Main(script.cyclingGradientOutline, script.cycleSpeedOutline, questAndHUD);
                    break;
            }
            generatedOutlineTextFXController = GenerateColorFXController.Main(fileNamePrefix, fxControllerToCopyPath, generatedOutlineTextMotion, defaultOffAnimation);
        }
        
        // ? generate for Monitor if needed
        AnimatorController generatedMonitorFXController = new AnimatorController();
        if (script.style == HeartRateMonitorSettings.StyleOption.Heart ||
            script.style.ToString().StartsWith("Screen") ||
            script.style == HeartRateMonitorSettings.StyleOption.HUD)
        {
            dynamic generatedMonitorMotion = new AnimationClip();
            switch(script.colorTypeMonitor)
            {
                case HeartRateMonitorSettings.ColorOptionMonitor.SolidColor:
                    generatedMonitorMotion = GenerateColorAnimation.Main(script.colorMonitor, questAndHUD);
                    break;
                case HeartRateMonitorSettings.ColorOptionMonitor.GradientByHeartrate:
                    generatedMonitorMotion = GenerateColorBlendtree.Main(script.gradientMonitor, "float", questAndHUD);
                    break;
                case HeartRateMonitorSettings.ColorOptionMonitor.CyclingGradient:
                    generatedMonitorMotion = GenerateCyclingColorAnimation.Main(script.cyclingGradientMonitor, script.cycleSpeedMonitor, questAndHUD);
                    break;
            }
            
            // ? generate for monitor off too
            dynamic generatedMonitorOffMotion = defaultOffAnimation;
            switch (script.colorTypeMonitorOff)
            {
                case HeartRateMonitorSettings.ColorOptionMonitorOff.SolidColor:
                    generatedMonitorOffMotion = GenerateColorAnimation.Main(script.colorMonitorOff, questAndHUD);
                    break;
                case HeartRateMonitorSettings.ColorOptionMonitorOff.GradientByHeartrate:
                    generatedMonitorOffMotion =
                        GenerateColorBlendtree.Main(script.gradientMonitorOff, "float", questAndHUD);
                    break;
                case HeartRateMonitorSettings.ColorOptionMonitorOff.CyclingGradient:
                    generatedMonitorOffMotion = GenerateCyclingColorAnimation.Main(
                        script.cyclingGradientMonitorOff, script.cycleSpeedMonitorOff, questAndHUD);
                    break;
            }
            
            // ^ if HUD then set as default off
            if (script.style == HeartRateMonitorSettings.StyleOption.HUD)
            {
                generatedMonitorOffMotion = defaultOffAnimation;
            }

            // ^ generate
            generatedMonitorFXController = GenerateColorFXController.Main(fileNamePrefix, fxControllerToCopyPath, generatedMonitorMotion, generatedMonitorOffMotion);

        }
        
        // ~ load number FX controller and heartrate params
        AnimatorController numberController =
            AssetDatabase.LoadAssetAtPath<AnimatorController>(numberControllerPath);
        VRCExpressionParameters heartRateParameter =
            AssetDatabase.LoadAssetAtPath<VRCExpressionParameters>(Config.heartrateParameterPath);

        // ~ get selected text prefab
        GameObject selectedPrefabText = (GameObject)Resources.Load( 
            Config.platformPaths[script.platform.ToString()] +
            Config.textTypePaths[script.style.ToString()] +
            Config.textPrefabNames[script.font.ToString()]);
        
        // ~ create instance of text prefab
        GameObject textObject = Instantiate(selectedPrefabText, containerObject.transform);
        
        // ~ apply FX according to whether outlined or not
        if (script.style != HeartRateMonitorSettings.StyleOption.OutlinedText)
        {
            if (script.style == HeartRateMonitorSettings.StyleOption.HUD)
            {
                textObject.name = "hrm_ht_" + script.font.ToString().ToLower();
            }
            else
            {
                textObject.name = "hrm_t_" + script.font.ToString().ToLower();
            }

            // ? add controller/params to normal text
            dynamic textObjectController = FuryComponents.CreateFullController(textObject);
            textObjectController.AddController(generatedTextFXController);
            textObjectController.AddController(numberController);
            textObjectController.AddParams(heartRateParameter);
            textObjectController.AddGlobalParam("*");
        }
        else
        {
            textObject.name = "hrm_ot_" + script.font.ToString().ToLower();
            // ? add controller to outlined text
            GameObject textObjectOutline = textObject.transform.GetChild(0).gameObject;
            dynamic textObjectOutlineController = FuryComponents.CreateFullController(textObjectOutline);
            textObjectOutlineController.AddController(generatedOutlineTextFXController);
            GameObject textObjectText = textObject.transform.GetChild(1).gameObject;
            dynamic textObjectTextController = FuryComponents.CreateFullController(textObjectText);
            textObjectTextController.AddController(generatedTextFXController);
            
            // ? add number controlling FX
            textObjectOutlineController.AddController(numberController);
            textObjectTextController.AddController(numberController);
            
            // ? add params
            textObjectOutlineController.AddParams(heartRateParameter);
            textObjectTextController.AddParams(heartRateParameter);
            textObjectOutlineController.AddGlobalParam("*");
            textObjectTextController.AddGlobalParam("*");
        }
        
        // ~ load monitor body if need be
        if (script.style == HeartRateMonitorSettings.StyleOption.Heart ||
            script.style.ToString().StartsWith("Screen") ||
            script.style == HeartRateMonitorSettings.StyleOption.HUD)
        {

            GameObject selectedPrefabMonitor =
                (GameObject)Resources.Load(
                    Config.platformPaths[script.platform.ToString()] +
                    Config.monitorPath +
                    Config.monitorPrefabPaths[script.style.ToString()]);
            GameObject monitorObject = Instantiate(selectedPrefabMonitor, containerObject.transform);
            monitorObject.name = "hrm_m_" + script.style.ToString().ToLower();

            // ? add color controller/params to monitor 
            dynamic monitorObjectController = FuryComponents.CreateFullController(monitorObject);
            monitorObjectController.AddController(generatedMonitorFXController);
            monitorObjectController.AddParams(heartRateParameter);
            monitorObjectController.AddGlobalParam("*");
            
            // ? add hud params and contact if necessary
            if (script.style == HeartRateMonitorSettings.StyleOption.HUD)
            {
                VRCExpressionParameters hudParameter =
                    AssetDatabase.LoadAssetAtPath<VRCExpressionParameters>(Config.parameterHUDPath);
                monitorObjectController.AddParams(hudParameter);

                AnimatorController hudController =
                    AssetDatabase.LoadAssetAtPath<AnimatorController>(Config.FXControllerHUDPath);
                monitorObjectController.AddController(hudController);
            }
            
            // ? load extra fx for screen if needed
            if (script.style.ToString().StartsWith("Screen"))
            {
                AnimatorController screenFX;
                if (script.autoTurnOff)
                {
                    screenFX =
                        AssetDatabase.LoadAssetAtPath<AnimatorController>(Config.screenFXAutoOffPath);
                }
                else
                {
                    screenFX =
                        AssetDatabase.LoadAssetAtPath<AnimatorController>(Config.screenFXPath);
                }
                monitorObjectController.AddController(screenFX);
            }
            
        }
        
        // ~ add menus if need be
        if (script.toggleOnOff ||
            script.toggleAct ||
            (script.toggleHUD && script.style == HeartRateMonitorSettings.StyleOption.HUD))
        {
            // ? load menu container and add full controller
            GameObject menuObjectPrefab =
                (GameObject)Resources.Load(Config.menuPrefabPath);
            GameObject menuObject = Instantiate(menuObjectPrefab, containerObject.transform);
            menuObject.name = "hrm_menu_items";
            GameObjectUtility.SetParentAndAlign(menuObject, containerObject);
            dynamic menuObjectController = FuryComponents.CreateFullController(menuObject);
            menuObjectController.AddGlobalParam("*");
            
            // ? add menus
            // ^ on/off menu
            if (script.toggleOnOff)
            {
                VRCExpressionParameters onOffParameter =
                    AssetDatabase.LoadAssetAtPath<VRCExpressionParameters>(Config.parameterOnOffPath);
                VRCExpressionsMenu onOffMenu =
                    AssetDatabase.LoadAssetAtPath<VRCExpressionsMenu>(Config.menuOnOffPath);
                menuObjectController.AddParams(onOffParameter);
                menuObjectController.AddMenu(onOffMenu, Config.menuPrefix);
            }
            
            // ^ act dead menu
            if (script.toggleAct)
            {
                VRCExpressionParameters actParameter =
                    AssetDatabase.LoadAssetAtPath<VRCExpressionParameters>(Config.parameterActDeadPath);
                VRCExpressionsMenu actMenu =
                    AssetDatabase.LoadAssetAtPath<VRCExpressionsMenu>(Config.menuActDeadPath);
                menuObjectController.AddParams(actParameter);
                menuObjectController.AddMenu(actMenu, Config.menuPrefix);
            }
            
            // ^ hud menu
            if (script.toggleHUD)
            {
                VRCExpressionParameters hudParameter =
                    AssetDatabase.LoadAssetAtPath<VRCExpressionParameters>(Config.parameterHUDPath);
                VRCExpressionsMenu hudMenu =
                    AssetDatabase.LoadAssetAtPath<VRCExpressionsMenu>(Config.menuHUDPath);
                menuObjectController.AddParams(hudParameter);
                menuObjectController.AddMenu(hudMenu, Config.menuPrefix);
            }

        }
        
        // ~ select the new container object for the monitor
        Selection.activeGameObject = containerObject;
        
    }
    
}

#endif