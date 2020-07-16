using System.Collections.Generic;
using UnityEngine;
using MLAgents;

/// <summary>
/// Used for handling a parking lot and set it states based on the current level of the curriculum
/// (the parking agent were trained using curriculum training).
/// </summary>
public class ParkingLot : MonoBehaviour
{
    /// <summary>
    /// A list of all of the parking lot parking spots.
    /// </summary>
    public List<ParkingSpot> parkingSpots;

    /// <summary>
    /// The agent of the parking lot.
    /// </summary>
    public ParkingAgent parkingAgent;

    /// <summary>
    /// The chosen random parking spot.
    /// </summary>
    ParkingSpot randomParkingSpot;

    /// <summary>
    /// Reset the parking lot based on the current level of the curriculum.
    /// </summary>
    /// <param name="level">The current level of the curriculum.</param>
    public void ResetParkingLot(float level)
    {
        ResetAllParkingSpots();

        // An empty parking lot, chosen parking spots are only from the left side
        if (level == 0f)
        {
            randomParkingSpot = parkingSpots[Random.Range(0, parkingSpots.Count/2)];
            randomParkingSpot.SetAsChosenParkingSpot();

            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                if (parkingSpot != randomParkingSpot)
                    parkingSpot.ActivateCarModel(false);
            }
        }

        // An empty parking lot, chosen parking spots are only from the right side
        else if (level == 1f)
        {
            randomParkingSpot = parkingSpots[Random.Range(parkingSpots.Count/2, parkingSpots.Count)];
            randomParkingSpot.SetAsChosenParkingSpot();

            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                if (parkingSpot != randomParkingSpot)
                    parkingSpot.ActivateCarModel(false);
            }
        }

        // An empty parking lot, chosen parking spots are from both sides
        else if (level == 2f)
        {
            // Select a random parking spot at the parking lot
            randomParkingSpot = parkingSpots[Random.Range(0, parkingSpots.Count)];
            randomParkingSpot.SetAsChosenParkingSpot();

            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                if (parkingSpot != randomParkingSpot)
                    parkingSpot.ActivateCarModel(false);
            }
        }

        // Chosen parking spots are only from the left side and are surrounded with cars 
        else if (level == 3f)
        {
            randomParkingSpot = parkingSpots[Random.Range(0, parkingSpots.Count / 2)];
            randomParkingSpot.SetAsChosenParkingSpot();

            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                if (parkingSpot != randomParkingSpot)
                    parkingSpot.ActivateCarModel(false);
            }

            SurroundWithCars(randomParkingSpot);
        }

        // Chosen parking spots are only from the right side and are surrounded with cars
        else if (level == 4f)
        {
            randomParkingSpot = parkingSpots[Random.Range(parkingSpots.Count / 2, parkingSpots.Count)];
            randomParkingSpot.SetAsChosenParkingSpot();

            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                if (parkingSpot != randomParkingSpot)
                    parkingSpot.ActivateCarModel(false);
            }

            SurroundWithCars(randomParkingSpot);
        }

        // Chosen parking spots are from both sides and are surrounded with cars
        else if (level == 5f)
        {
            randomParkingSpot = parkingSpots[Random.Range(0, parkingSpots.Count)];
            randomParkingSpot.SetAsChosenParkingSpot();

            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                if (parkingSpot != randomParkingSpot)
                    parkingSpot.ActivateCarModel(false);
            }

            SurroundWithCars(randomParkingSpot);
        }

        // Chosen parking spots are only from the left side.
        // they are surrounded with cars + the other side of the parking lot is full of cars
        else if (level == 6f)
        {
            randomParkingSpot = parkingSpots[Random.Range(0, parkingSpots.Count / 2)];
            randomParkingSpot.SetAsChosenParkingSpot();

            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                if (parkingSpot.side == randomParkingSpot.side && parkingSpot != randomParkingSpot)
                    parkingSpot.ActivateCarModel(false);
            }

            SurroundWithCars(randomParkingSpot);
        }

        // Chosen parking spots are only from the right side.
        // they are surrounded with cars + the other side of the parking lot is full of cars
        else if (level == 7f)
        {
            randomParkingSpot = parkingSpots[Random.Range(parkingSpots.Count / 2, parkingSpots.Count)];
            randomParkingSpot.SetAsChosenParkingSpot();

            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                if (parkingSpot.side == randomParkingSpot.side && parkingSpot != randomParkingSpot)
                    parkingSpot.ActivateCarModel(false);
            }

            SurroundWithCars(randomParkingSpot);
        }

        // Chosen parking spots are both sides.
        // they are surrounded with cars + the other side of the parking lot is full of cars
        else if (level == 8f)
        {
            randomParkingSpot = parkingSpots[Random.Range(0, parkingSpots.Count)];
            randomParkingSpot.SetAsChosenParkingSpot();

            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                if (parkingSpot.side == randomParkingSpot.side && parkingSpot != randomParkingSpot)
                    parkingSpot.ActivateCarModel(false);
            }

            SurroundWithCars(randomParkingSpot);
        }

        // The default level 9, a full parking lot
        else
        {
            randomParkingSpot = parkingSpots[Random.Range(0, parkingSpots.Count)];
            randomParkingSpot.SetAsChosenParkingSpot();
        }

        // Spawn the agent into the parking lot
        SpawnAgent();
    }

    /// <summary>
    /// Surround the selected parking spot with cars by activating the closest parking spots car models.
    /// </summary>
    /// <param name="parkingSpot">The parking spot to surround.</param>
    public void SurroundWithCars(ParkingSpot parkingSpot)
    {
        int parkingSpotIndex = FindParkingSpotIndex(parkingSpot);

        // Handling the corner parking spots
        if (parkingSpotIndex == 0)
            parkingSpots[1].ActivateCarModel(true);

        else if (parkingSpotIndex == 5)
            parkingSpots[4].ActivateCarModel(true);

        else if (parkingSpotIndex == 6)
            parkingSpots[7].ActivateCarModel(true);

        else if (parkingSpotIndex == 11)
            parkingSpots[10].ActivateCarModel(true);

        else
        {
            parkingSpots[parkingSpotIndex + 1].ActivateCarModel(true);
            parkingSpots[parkingSpotIndex - 1].ActivateCarModel(true);
        }
    }

    /// <summary>
    /// Spawn the agent into the parking lot.
    /// </summary>
    public void SpawnAgent()
    {
        parkingAgent.rb.velocity = Vector3.zero;
        parkingAgent.rb.angularVelocity = Vector3.zero;

        parkingAgent.transform.localPosition = ChooseRandomPosition();
        parkingAgent.transform.localRotation = ChooseRandomRotation();
    }

    /// <summary>
    /// Choose a random position in the parking spot to spawn the agent in.
    /// </summary>
    /// <returns>
    /// The chosen position.
    /// </returns>
    public Vector3 ChooseRandomPosition()
    {
        float zSpawnPosition = Academy.Instance.FloatProperties.GetPropertyWithDefault("spawn_range", 1f);
        float randomXPosition = Random.Range(-2.5f, 2.5f);
        float randomZPosition = Random.Range(-zSpawnPosition, zSpawnPosition);

        return new Vector3(randomXPosition, 0.5f, randomZPosition);
    }

    /// <summary>
    /// Choose a random rotation to spawn the agent in.
    /// </summary>
    /// <returns>
    /// The chosen rotation.
    /// </returns>
    public Quaternion ChooseRandomRotation()
    {
        return Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    /// <summary>
    /// Get the chosen parking spot.
    /// </summary>
    /// <returns>
    /// The chosen parking spot
    /// </returns>
    public ParkingSpot GetChosenParkingSpot()
    {
        return randomParkingSpot;
    }

    /// <summary>
    /// Reset all parking spots into their standart state.
    /// </summary>
    void ResetAllParkingSpots()
    {
        foreach (ParkingSpot parkingSpot in parkingSpots)
            parkingSpot.ResetParkingSpot();
    }

    /// <summary>
    /// Return the index of a given parking spot in the parking spots list.
    /// </summary>
    /// <returns>
    /// The index of the parking spot.
    /// </returns>
    /// <param name="parkingSpot">The parking spot to find it's index.</param>
    int FindParkingSpotIndex(ParkingSpot parkingSpot)
    {
        for (int i =0; i < parkingSpots.Count; i++)
        {
            if (parkingSpots[i] == parkingSpot)
                return i;
        }

        // The item does not exist in the list
        return -1;
    }
}
