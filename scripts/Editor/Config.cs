#if UNITY_EDITOR

using System.Collections.Generic;

public class Config
{
    
    // & FX generation
    public static string resourcesPath = "Assets/32294/heartrate_monitor/scripts/Resources/";
    public static string generatedPath = resourcesPath + "generated/";
    
    public static string screenFXPath = "Assets/32294/heartrate_monitor/animations/monitor/screen_on.controller";
    public static string screenFXAutoOffPath = "Assets/32294/heartrate_monitor/animations/monitor/screen_on_autooff.controller";
    
    public static string offAnimationPath = "Assets/32294/heartrate_monitor/animations/monitor/off.anim";
    public static string customAnimationPath = "Assets/32294/heartrate_monitor/animations/color/custom.anim";
    public static string customQuestHUDAnimationPath = "Assets/32294/heartrate_monitor/animations/color/custom_quest_hud.anim";
    public static string customBlendtreePath = "Assets/32294/heartrate_monitor/animations/color/custom.asset";
    
    public static string numberControllerPath = "Assets/32294/heartrate_monitor/animations/number/number";
    public static string numberControllerAutoOffPath = "Assets/32294/heartrate_monitor/animations/number/number_autooff";
    public static string customFXControllerPath = "Assets/32294/heartrate_monitor/animations/color/custom";
    public static string customFXControllerAutoOffPath = "Assets/32294/heartrate_monitor/animations/color/custom_autooff";

    public static string FXControllerHUDPath = "Assets/32294/heartrate_monitor/animations/monitor/hud_contact.controller";
    
    // & parameters
    public static string heartrateParameterPath = "Assets/32294/heartrate_monitor/menu/parameters.asset";
    public static string parameterOnOffPath = "Assets/32294/heartrate_monitor/menu/parameters_toggle_on_off.asset";
    public static string parameterActDeadPath = "Assets/32294/heartrate_monitor/menu/parameters_act_dead.asset";
    public static string parameterHUDPath = "Assets/32294/heartrate_monitor/menu/parameters_hud.asset";
    
    // & menus
    public static string menuPrefabPath = "hrm_menu_items";
    public static string menuOnOffPath = "Assets/32294/heartrate_monitor/menu/toggle_on_off.asset";
    public static string menuActDeadPath = "Assets/32294/heartrate_monitor/menu/toggle_act_dead.asset";
    public static string menuHUDPath = "Assets/32294/heartrate_monitor/menu/toggle_hud.asset";
    public static string menuPrefix = "HR Monitor";
    
    // & prefab locations
    // ~ folder path prepends
    public static string monitorPath = "monitor/";
    
    // ~ text types
    public static Dictionary<string, string> textTypePaths = new Dictionary<string, string>
    {
        {"Text", "text/hrm_t_"},
        {"OutlinedText", "outlined_text/hrm_ot_"},
        {"Heart", "text/hrm_t_"},
        {"ScreenHeart", "text/hrm_t_"},
        {"ScreenSquare", "text/hrm_t_"},
        // {"ScreenFish", "text/hrm_t_"},
        {"HUD", "hud_text/hrm_ht_"},
    };    

    
    // ~ platforms
    public static Dictionary<string, string> platformPaths = new Dictionary<string, string>
    {
        {"PC", "PC/"},
        {"Quest", "Quest/"}
    };    
    
    // ~ monitor types
    public static Dictionary<string, string> monitorPrefabPaths = new Dictionary<string, string>
    {
        {"Heart", "hrm_m_heart"},
        {"ScreenHeart", "hrm_m_screen_heart"},
        {"ScreenSquare", "hrm_m_screen_square"},
        // {"ScreenFish", "hrm_m_screen_fish"},
        {"HUD", "hrm_m_hud"},
    };    
    
    // ~ font prefab names
    public static Dictionary<string, string> textPrefabNames = new Dictionary<string, string>
    {
        {"Default", "default"},
        {"Sans", "sans"},
        {"Serif", "serif"},
        {"Square", "square"},
        {"SevenSegment", "seven_segment"},
        {"Minecraft", "minecraft"},
        {"Barcode", "barcode"},
    };
    
}


#endif