using UnityEngine;

public class MainMenuBusController : MonoBehaviour
{
    public static bool IsGameAudioLoaded { get; set; } = false;
    public static bool IsGameAudioLoadingStarted { get; set; } = false;

    public static bool IsGameAudioChanged { get; set; } = false;

    public static readonly string NO_AUDIO_IMPORTED = "No audio\n imported";

    public static readonly string AUDIO_IMPORTED = "Audio imported:\n";
}
