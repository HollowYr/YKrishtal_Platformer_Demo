/***
 * Sets Screen orientation on Awake.
 ***/
using UnityEngine;

public class OrientationSetter : MonoBehaviour
{
    public enum Orientation
    {
        Any,
        Portrait,
        PortraitFixed,
        Landscape,
        LandscapeFixed
    }

    public Orientation ScreenOrientation;

    private void Awake()
    {
        switch (ScreenOrientation)
        {
            case Orientation.Any:
                Screen.orientation = UnityEngine.ScreenOrientation.AutoRotation;

                Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
                break;

            case Orientation.Portrait:
                // Force screen to orient right, then switch to Auto
                Screen.orientation = UnityEngine.ScreenOrientation.Portrait;


                Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = false;
                Screen.orientation = UnityEngine.ScreenOrientation.AutoRotation;
                break;

            case Orientation.PortraitFixed:
                Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
                break;

            case Orientation.Landscape:
                // Force screen to orient right, then switch to Auto
                Screen.orientation = UnityEngine.ScreenOrientation.Landscape;


                Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
                Screen.orientation = UnityEngine.ScreenOrientation.AutoRotation;
                break;

            case Orientation.LandscapeFixed:
                Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
                break;
        }
    }
}