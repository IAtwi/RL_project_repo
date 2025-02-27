using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
//using Unity.MLAgents.Sensors;
using UnityEngine;

public enum Team
{
    Blue = 0,
    Purple = 1
}

public class AgentSoccer : Agent
{
    // Note that that the detectable tags are different for the blue and purple teams. The order is
    // * ball
    // * own goal
    // * opposing goal
    // * wall
    // * own teammate
    // * opposing player

    public enum Position
    {
        Striker,
        Goalie,
        Generic
    }

    [HideInInspector]
    public Team team;
    private float m_KickPower;
    private readonly float _rotationSpeed = 1.4f;
    // The coefficient for the reward for colliding with a ball. Set using curriculum.
    private float m_BallTouch;
    public Position position;

    private const float k_Power = 2000f;
    private float m_Existential;
    private float m_LateralSpeed;
    private float m_ForwardSpeed;


    [HideInInspector]
    public Rigidbody agentRb;
    private SoccerSettings m_SoccerSettings;
    private BehaviorParameters m_BehaviorParameters;
    public Vector3 initialPos;
    public float rotSign;
    private Vector3 previousBallPosition;
    private SoccerEnvController envController;

    private EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        envController = GetComponentInParent<SoccerEnvController>();
        previousBallPosition = envController.ball.transform.position;  // Initialize ball position
        if (envController != null)
        {
            m_Existential = 1f / envController.MaxEnvironmentSteps;

            if (envController.ball == null)
            {
                Debug.LogError("SoccerEnvController: ball is not set.");
            }

            //if (envController.m_BlueAgentGroup == null)
            //{
            //    Debug.LogError("SoccerEnvController: m_BlueAgentGroup is not set.");
            //}

            //if (envController.m_PurpleAgentGroup == null)
            //{
            //    Debug.LogError("SoccerEnvController: m_PurpleAgentGroup is not set.");
            //}
        }
        else
        {
            Debug.LogError("SoccerEnvController is not found in parent.");
            m_Existential = 1f / MaxStep;
        }

        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
            initialPos = new Vector3(transform.position.x - 5f, .5f, transform.position.z);
            rotSign = 1f;
        }
        else
        {
            team = Team.Purple;
            initialPos = new Vector3(transform.position.x + 5f, .5f, transform.position.z);
            rotSign = -1f;
        }

        m_LateralSpeed = 0.5f;
        m_ForwardSpeed = 0.8f;
        if (position == Position.Goalie)
        {
            m_LateralSpeed = 0.8f;
        }
        else if (position == Position.Striker)
        {
            m_ForwardSpeed = 1.1f;
        }

        m_SoccerSettings = FindAnyObjectByType<SoccerSettings>();
        if (m_SoccerSettings == null)
        {
            Debug.LogError("SoccerSettings is not found.");
        }

        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;

        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        m_KickPower = 0f;

        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                m_KickPower = 1f;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f * _rotationSpeed);
        agentRb.AddForce(dirToGo * m_SoccerSettings.agentRunSpeed,
            ForceMode.VelocityChange);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (position == Position.Goalie)
        {
            // Existential bonus for Goalies.
            AddReward(m_Existential);
        }
        else if (position == Position.Striker)
        {
            // Existential penalty for Strikers
            AddReward(-m_Existential);
        }

        MoveAgent(actionBuffers.DiscreteActions);

        // Reward passing to teammates
        if (HasPassedToTeammate())
        {
            AddReward(0.2f);  // Small reward for successful pass
        }

        // Encourage spreading out
        if (IsProperlySpaced())
        {
            AddReward(0.1f);
        }

        // Save the current ball position for next step
        previousBallPosition = envController.ball.transform.position;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
    }
    /// <summary>
    /// Used to provide a "kick" to the ball.
    /// </summary>
    private void OnCollisionEnter(Collision c)
    {
        var force = k_Power * m_KickPower;
        if (position == Position.Goalie)
        {
            force = k_Power;
        }
        if (c.gameObject.CompareTag("ball"))
        {
            AddReward(.2f * m_BallTouch);
            var dir = c.contacts[0].point - transform.position;
            dir = dir.normalized;
            c.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
        }
    }

    public override void OnEpisodeBegin()
    {
        m_BallTouch = m_ResetParams.GetWithDefault("ball_touch", 0);
    }

    //public override void CollectObservations(VectorSensor sensor)
    //{
    //    // Local awareness (via RayPerception)
    //    // The RayPerceptionSensor should already add observations automatically

    //    // Add agent's own position
    //    sensor.AddObservation(transform.position.x);
    //    sensor.AddObservation(transform.position.z);

    //    // Add ball position (global info)
    //    SoccerEnvController envController = GetComponentInParent<SoccerEnvController>();
    //    if (envController != null && envController.ball != null)
    //    {
    //        sensor.AddObservation(envController.ball.transform.position.x);
    //        sensor.AddObservation(envController.ball.transform.position.z);
    //    }
    //    else
    //    {
    //        // Add default values if envController or ball is null
    //        sensor.AddObservation(0f);
    //        sensor.AddObservation(0f);
    //    }

    //    // Add teammate positions (shared observation)
    //    var agentGroup = team == Team.Blue ? envController?.m_BlueAgentGroup?.GetRegisteredAgents() : envController?.m_PurpleAgentGroup?.GetRegisteredAgents();
    //    if (agentGroup != null)
    //    {
    //        foreach (var agent in agentGroup)
    //        {
    //            if (agent != this)  // Exclude self
    //            {
    //                sensor.AddObservation(agent.transform.position.x);
    //                sensor.AddObservation(agent.transform.position.z);
    //            }
    //        }
    //    }
    //}

    private bool HasPassedToTeammate()
    {
        var teamGroup = team == Team.Blue ? envController.m_BlueAgentGroup : envController.m_PurpleAgentGroup;

        foreach (var teammate in teamGroup.GetRegisteredAgents())
        {
            if (teammate != this)
            {
                float distanceBefore = Vector3.Distance(previousBallPosition, teammate.transform.position);
                float distanceAfter = Vector3.Distance(envController.ball.transform.position, teammate.transform.position);

                // If ball got closer to a teammate, it's a pass
                if (distanceAfter < distanceBefore)
                {
                    //Debug.Log((distanceBefore - distanceAfter) + " Passing reward: True");
                    return true;
                } else
                {
                    //Debug.Log((distanceBefore - distanceAfter) + " Passing reward: False");
                }
            }
        }
        return false;
    }

    private bool IsProperlySpaced()
    {
        float minDistance = 2.0f; // Minimum distance between teammates

        var teamGroup = team == Team.Blue ? envController.m_BlueAgentGroup : envController.m_PurpleAgentGroup;
        foreach (var teammate in teamGroup.GetRegisteredAgents())
        {
            if (teammate != this)
            {
                float distance = Vector3.Distance(transform.position, teammate.transform.position);
                if (distance < minDistance)
                {
                    //Debug.Log(distance + " Spacing reward: False");
                    return false;
                }
                else
                {
                    //Debug.Log(distance + " Spacing reward: True");
                }
            }
        }
        return true;
    }
}
