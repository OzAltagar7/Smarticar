using UnityEngine;

/// <summary>
/// A class representing a single parking spot
/// </summary>
public class ParkingSpot : MonoBehaviour
{
    /// <summary>
    /// The collider of the parking spot used to check if the agent parked.
    /// </summary>
    public BoxCollider parkingCollider;

    /// <summary>
    /// The trigger collider of the parking spot used to determine 
    /// if the user entered the parking spot in order to color it in green.
    /// </summary>
    public BoxCollider parkingTrigger;

    /// <summary>
    /// The car model of the parking spot
    /// </summary>
    public GameObject carModel;

    /// <summary>
    /// The lines of the parking spot
    /// </summary>
    public MeshRenderer[] parkingSpotLines;

    /// <summary>
    /// The side of the parking spot in the parking lot
    /// </summary>
    public string side;

    /// <summary>
    /// Whether or not the parking spot was chosen as the random parking spot
    /// </summary>
    public bool isChosen;

    /// <summary>
    /// The parking lines colors-
    /// standart = white, marked = red and successful = green.  
    /// </summary>
    public Material standartParkingSpot;
    public Material markedParkingSpot;
    public Material successfulParkingSpot;
    
    /// <summary>
    /// Set the activation status of the parking spot car model.
    /// </summary>
    /// <param name="activationStatus">true=activate, false=deactivate.</param>
    public void ActivateCarModel(bool activationStatus)
    {
        carModel.SetActive(activationStatus);
    }

    /// <summary>
    /// Set the activation status of the parking spot colliders.
    /// </summary>
    /// <param name="activationStatus">true=activate, false=deactivate.</param>
    public void ActivateParkingColliders(bool activationStatus)
    {
        parkingCollider.enabled = activationStatus;
        parkingTrigger.enabled = activationStatus;
    }

    /// <summary>
    /// Reset the parking spot to it's default state-
    /// car model is activated, colliders are deactivated and the lines are colored white.
    /// </summary>
    public void ResetParkingSpot()
    {
        ActivateCarModel(true);
        ActivateParkingColliders(false);
        isChosen = false;
        parkingSpotLines[0].enabled = false;
        ColorParkingSpot(standartParkingSpot);
    }

    /// <summary>
    /// Color the parking spot lines in a given color
    /// </summary>
    /// <param name="parkingSpotColor">the color to change to</param>
    public void ColorParkingSpot(Material parkingSpotColor)
    {
        foreach(MeshRenderer parkingSpotLine in parkingSpotLines)
            parkingSpotLine.material = parkingSpotColor;
    }

    /// <summary>
    /// Set the parking spot as chosen-
    /// car model is deactivated, colliders are activated and the lines are colored red.
    /// </summary>
    public void SetAsChosenParkingSpot()
    {
        isChosen = true;
        ActivateCarModel(false);
        ActivateParkingColliders(true);
        parkingSpotLines[0].enabled = true;
        ColorParkingSpot(markedParkingSpot);
    }

    /// <summary>
    /// When the agent enters the chosen parking spot, color it green.
    /// </summary>
    /// <param name="other">The agent collider</param>
    public void OnTriggerStay(Collider other)
    {
        if (isChosen)
            ColorParkingSpot(successfulParkingSpot);
    }
}
