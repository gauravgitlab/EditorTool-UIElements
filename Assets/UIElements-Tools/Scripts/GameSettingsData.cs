
using UnityEngine;
public enum eScreenMode { FullScreen, Windowed }
public class GameSettingsData
{
    public float Music = 0.5f;
    public float Sfx = 0.5f;

    public string Resolution = $"{Screen.width} x {Screen.height}";
    public eScreenMode ScreenMode = eScreenMode.FullScreen;
    public bool VSync = false;

    public bool AddSupporter = false;
    public bool CameraVfx = false;
    public bool AutoShoot = false;
}
