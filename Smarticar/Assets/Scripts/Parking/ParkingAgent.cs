using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

/// <summary>
/// The parking agent class.
/// For the ML-Agents Agent documentation-
/// <see href="https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Learning-Environment-Design-Agents.md"/>
/// </summary>
public class ParkingAgent : Agent
{
    /// <summary>
    /// True if the agent object corresponds to the user.
    /// </summary>
    public bool isUser;

    // Adds a space of 10 pixels in the inspector
    [Space(10)]

    /// <summary>
    /// The RigidBody component of the agent.
    /// </summary>
    public Rigidbody rb;

    /// <summary>
    /// The agent's driving speed.
    /// </summary>
    public float drivingSpeed;

    /// <summary>
    /// The degree the agent rotates in a single turn.
    /// </summary>
    public float turnAmount;

    [Space(10)]

    /// <summary>
    /// The parking lot the agent drives in.
    /// </summary>
    public ParkingLot parkingLot;

    /// <summary>
    /// A reference to the UI manager.
    /// </summary>
    public ParkingUIManager UIManager;

    /// <summary>
    /// The score of the agent.
    /// </summary>
    [HideInInspector]
    public int Score { get; set; } = 0;

    /// <summary>
    /// The selected parking spot
    /// </summary>
    ParkingSpot parkingSpot;

    /// <summary>
    /// Collects the agent vector observations in order to feed the neural network policy.
    /// The agent calculate it's distance from the parking spot, the direction to the parking spot,
    /// and the direction the car is facing.
    /// </summary>
    /// <param name="sensor">A <see cref="VectorSensor"/> object for adding observations.</param>
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 parkingSpotPosition = parkingSpot.transform.position;


        // The distance from the parking spot
        sensor.AddObservation(Vector3.Distance(transform.position, parkingSpotPosition));

        // The direction to the parking spot
        sensor.AddObservation((parkingSpotPosition - transform.position).normalized);

        // The direction the car is facing
        sensor.AddObservation(transform.forward);
    }

    /// <summary>
    /// Specifies the agent behavior when being reset (due to crashing or falling off the platform).
    /// </summary>
    public override void OnEpisodeBegin()
    {
        // Reset the parking lot based on the current level of the curricula.
        // Used when training using curriculum training
        parkingLot.ResetParkingLot(Academy.Instance.FloatProperties.GetPropertyWithDefault("level", 9f));

        // Store the chosen random parking spot
        parkingSpot = parkingLot.GetChosenParkingSpot();
    }

    /// <summary>
    /// Specifies the agent behavior at every step based on the provided action.
    /// In this case, whether or not the agent shall turn and in which direction,
    /// and whether or not the agent should drive and in which direction.
    /// </summary>
    /// <param name="vectorAction">The action array.</param>
    public override void OnActionReceived(float[] vectorAction)
    {
        // Drive according to the neural network decision
        Drive(vectorAction[0]);
        Steer(vectorAction[1]);

        // The agent fell off the platform,
        // ending the episode and assigning a negative reward for falling
        if (transform.position.y < -1f)
        {
            AddReward(-1.0f);
            EndEpisode();
        }

        // Add a -1/maxStep reward to the agent for not parking in order to speed up the parking process.
        // If the agent finished the episode without parking, it will get a maximum total negative reward of -(maxStep/maxStep) = -1
        AddReward(-1 / maxStep);
    }

    /// <summary>
    /// When the Agent uses Heuristics, it will call this method every time it needs an action,
    /// which is based on keyboard input instead of a neural network (lets the user control the agent).
    /// Since Discrete vector action space is being used, using 2 branch of size 3,
    /// the method shall return a float array of size 2 each slot containing either 0, 1 or 2.
    /// </summary>
    /// <returns> 
    /// A float array corresponding to the next action of the Agent
    /// </returns>
    public override float[] Heuristic()
    {
        // By default, the agent will stay in place
        float drivingDirection = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
            drivingDirection = 1f;

        else if (Input.GetKey(KeyCode.DownArrow))
            drivingDirection = 2f;

        // By default, the agent will not turn
        float steeringDirection = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
            steeringDirection = 1f;

        else if (Input.GetKey(KeyCode.LeftArrow))
            steeringDirection = 2f;

        // Return the action array
        return new float[] { drivingDirection, steeringDirection };
    }

    /// <summary>
    /// Drive in the given direction.
    /// </summary>
    /// <param name="direction">The direction to drive in.</param>
    public void Drive(float direction)
    {
        // Stay in place
        float d = 0f;

        // Drive forward
        if (direction == 1f)
            d = 1f;

        // Go in reverse
        else if (direction == 2f)
            d = -1f;

        // Move the agent in the given direction
        rb.MovePosition(transform.position + transform.forward * d * drivingSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Rotate in the given direction.
    /// </summary>
    /// <param name="direction">The rotation direction.</param>
    public void Steer(float direction)
    {
        // Do not rotate
        float d = 0f;

        // Steer left
        if (direction == 1f)
            d = 1f;

        // Steer right
        else if (direction == 2f)
            d = -1f;

        // Rotate in the given direction
        transform.Rotate(transform.up * d * turnAmount * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Handles the agent collisions; when the agent crashes or parks.
    /// </summary>
    /// <param name="collision">The collision info.</param>
    void OnCollisionEnter(Collision collision)
    {
        // The agent crashed- add a negative reward of 1f for crashing and end the episode
        if (collision.collider.tag == "Obstacle") 
        {
            AddReward(-1.0f);
            EndEpisode();
        }

        // The agent parked
        if (collision.collider.tag == "Parking")
        {
            // Increment the agent score
            Score++;
            UIManager.UpdateScore(this);

            // Add a positive reward of 1f for parking and end the episode
            AddReward(1f);
            EndEpisode();
        }
    }
}
