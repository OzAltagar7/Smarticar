using UnityEngine;
using MLAgents;

/// <summary>
/// The racetrack agent class.
/// For the ML-Agents Agent documentation-
/// <see href="https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Learning-Environment-Design-Agents.md"/>
/// </summary>
public class CarAgent : Agent
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
    public Rigidbody rbComponent;

    /// <summary>
    /// The agent's forward speed.
    /// </summary>
    public float forwardSpeed;

    /// <summary>
    /// The degree the agent rotates in a single turn.
    /// </summary>
    public float turnAmount;

    /// <summary>
    /// The reset point for the agent when crashing.
    /// </summary>
    public Vector3 resetPoint;

    [Space(10)]

    /// <summary>
    /// A reference to the race manager.
    /// </summary>
    public RaceManager raceManager;

    /// <summary>
    /// A reference to the UI manager.
    /// </summary>
    public RacetrackUIManager UIManager;

    /// <summary>
    /// The number of laps the agent made in the race.
    /// Will reset when crashing.
    /// </summary>
    [HideInInspector]
    public int LapsMade { get; set; } = 0;

    /// <summary>
    /// Whether or not the agent has passed the checkpoint.
    /// Used to determine if the agent has completed the course when passing through the starting line.
    /// </summary>
    bool passedCheckpoint;

    /// <summary>
    /// True if the agent stopped.
    /// </summary>
    bool stopped;

    /// <summary>
    /// Specifies the agent behavior when being reset (due to crashing).
    /// </summary>
    public override void OnEpisodeBegin()
    {
        // Spawn the agent back at the reset point
        Spawn();
    }

    /// <summary>
    /// Specifies the agent behavior at every step based on the provided action.
    /// In this case, whether or not the agent shall turn and in which direction.
    /// </summary>
    /// <param name="vectorAction">The action array.</param>
    public override void OnActionReceived(float[] vectorAction)
    {
        if (!stopped)
        {
            // The agent moves forward automatically
            MoveForward();

            // Turn based on the neural network output
            Turn(vectorAction[0]);
        }

        // Add a 1/maxStep reward to the agent for not crashing.
        // If the agent finished the episode without crashing, it will get a maximum total reward of maxStep/maxStep = 1
        AddReward(1 / maxStep);
    }

    /// <summary>
    /// When the Agent uses Heuristics, it will call this method every time it needs an action,
    /// which is based on keyboard input instead of a neural network (lets the user control the agent).
    /// Since Discrete vector action space is being used, using 1 branch of size 3,
    /// the method shall return a float array of size 1 containing either 0, 1 or 2.
    /// </summary>
    /// <returns> 
    /// A float array corresponding to the next action of the Agent
    /// </returns>
    public override float[] Heuristic()
    {
        // By default, the agent will not turn
        float turnValue = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            turnValue = 1f;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turnValue = 2f;
        }

        // Return the action array
        return new float[] { turnValue };
    }

    /// <summary>
    /// Move the agent forward at a speed of <see cref="forwardSpeed"/>
    /// </summary>
    public void MoveForward()
    {
        rbComponent.velocity = transform.forward * forwardSpeed;
    }

    /// <summary>
    /// Turn <see cref="turnAmount"/> degrees in the given direction.
    /// </summary>
    /// <param name="direction">The direction of the rotation.</param>
    public void Turn(float direction)
    {
        if (direction == 1)
        {
            // Turn right
            transform.Rotate(0, turnAmount, 0);
        }

        else if (direction == 2)
        {
            // Turn left
            transform.Rotate(0, -turnAmount, 0);
        }
    }

    /// <summary>
    /// Stop the car by setting <see cref="stopped"/> true.
    /// </summary>
    public void Stop()
    {
        stopped = true;
    }

    /// <summary>
    /// Spawn the agent back at the <see cref="resetPoint"/> facing forward.
    /// </summary>
    public void Spawn()
    {
        transform.localPosition = resetPoint;
        transform.localRotation = Quaternion.Euler(transform.forward);
    }

    /// <summary>
    /// Handles the agent trigger collisions- passing the checkpoint or going through the starting line.
    /// </summary>
    /// <param name="other">The object the agent collided with.</param>
    void OnTriggerEnter(Collider other)
    {
        // The agent passed the checkpoint
        if (other.CompareTag("LapCheckpoint"))
        {
            passedCheckpoint = true;
        }

        // The agent went through the starting line
        if (other.CompareTag("StartingLine"))
        {
            // If the agent didn't pass the checkpoint,
            // it means that the agent is starting a new lap after a crash
            if (passedCheckpoint)
            {
                // Update the lap counter
                LapsMade++;
                UIManager.UpdateLapCount(this);

                // If the agent won, the game
                if (LapsMade == 2)
                {
                    raceManager.EndGame();
                }
            }

            // Reset passedCheckpoint status
            passedCheckpoint = false;
        }
    }

    /// <summary>
    /// Handles the agent collisions; when the agent crashes.
    /// </summary>
    /// <param name="collision">The collision info.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            // Reset the lap counter
            LapsMade = 0;
            UIManager.UpdateLapCount(this);

            // Add a negative reward of 1f to the agent for crashing and end the episode
            AddReward(-1.0f);
            EndEpisode();
        }
    }
}
