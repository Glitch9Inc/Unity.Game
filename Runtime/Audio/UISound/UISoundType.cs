namespace Glitch9
{
    public enum UISoundType
    {
        None,

        // Button Sounds
        Click,
        Confirm,
        Cancel,
        ToggleOn,  // ItemSelect and ToggleOn are the same sound
        ToggleOff, // ItemDeselect and ToggleOff are the same sound

        // Transition Sounds
        MenuOpen,
        MenuClose,
        MenuOpenSubtle,
        MenuCloseSubtle,
        PopUpOpen,
        PopUpClose,
        SwipeLeft,  // also refers to SlideLeft
        SwipeRight, // also refers to SlideRight
        DropdownOpen,   // rarely used
        DropdownClose,  // rarely used

        // Alert Sounds
        AlertInfo,
        AlertWarning,
        AlertError,
        AlertSuccess,
        AlertFailure,

        // Game
        LevelUp,
        Victory,
        GameOver,
        RewardCascade,

        // Extra
        LoadingStart,
        LoadingEnd,
    }
}