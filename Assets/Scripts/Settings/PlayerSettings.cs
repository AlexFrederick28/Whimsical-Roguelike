using System;
using UnityEngine;

/// <summary>
/// Stores player settings such as camera zoom speed from the settings menu
/// </summary>
/// 
[Serializable]
public class PlayerSettings 
{
    // camera
    public float cameraRotationSpeed;
    public float cameraZoomSpeed;
}
