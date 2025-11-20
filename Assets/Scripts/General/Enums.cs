using System;

public class Enums
{
    #region ANIMATION
    public enum AnimationType
    {
        MOVE_HORIZONTAL,
        MOVE_VERTICAL,
        SCALE,
        SCROLL_HORIZONTAL,
        SCROLL_VERTICAL,
        FADE_IMAGE,
        FADE_TEXT,
        FADE_CANVASGROUP,
        ROTATE_CLOCKWISE,
        ROTATE_COUNTERCLOCKWISE
    }
    #endregion

    #region SCENE
    public enum SceneType
    {
        MAIN_MENU,
        GAMEPLAY,
        GARDEN
    }
    #endregion

    #region AUDIO
    public enum BGMType
    {
        NONE,
        DEFAULT
    }
    public enum AmbienceType
    {
        NONE,
        MAIN_MENU,
        GAMEPLAY
    }
    public enum SFXType
    {
        BUTTON_CLICK,
        CARD_DROP,
        CARD_PICK,
        FAIL,
        ROULETTE,
        SUCCESS,
        CARD_MATCH,
        POINT_1,
        POINT_6
    }
    #endregion

    #region VFX
    public enum VFXTrigger
    {
        NONE,
        PLANT_UNLOCKED,
        SMOKE_EFFECT,
    }
    #endregion

    #region GAMEPLAY
    public enum CardType
    {
        WATER = 1,
        SOIL = 2,
        SEED = 3,
        SUN = 4,
        AIR = 5,
        COMPOST = 6,
        HUMUS = 7,
        PEST = 8
    }
    public enum GameState
    {
        FIRST_ROUND, //Move 2 cards from bottom to middle
        SECOND_ROUND, //Move 2 cards from top to middle
        THIRD_ROUND //Reveal last card
    }
    #endregion

    #region Tutorial
    
    public enum TutorialStep
    {
        NORMAL_TUTORIAL,
        SECOND_ROUND,
        THIRD_ROUND,
        RESUME_TIME,
        STOP_TIME
    }
    
    #endregion

    #region GARDEN
    public enum PlantColor
    {
        GREEN,
        YELLOW,
        RED,
        GOLDEN
    }
    #endregion
}
