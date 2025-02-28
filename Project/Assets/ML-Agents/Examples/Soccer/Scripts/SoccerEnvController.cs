using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using static AgentSoccer;

public class SoccerEnvController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo
    {
        public AgentSoccer Agent;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Quaternion StartingRot;
        [HideInInspector]
        public Rigidbody Rb;
    }

    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 5000; // Max Academy steps before this platform resets
    public GameObject ball;
    [SerializeField] private ScoreBoard _scoreBoard;
    [SerializeField] private List<MeshRenderer> _wallMeshRenderers;
    [SerializeField] private List<PlayerInfo> AgentsList = new(); //List of Agents On Platform


    public SimpleMultiAgentGroup m_BlueAgentGroup;
    public SimpleMultiAgentGroup m_PurpleAgentGroup;

    private FieldController _fieldController;
    private int m_ResetTimer;
    private Rigidbody ballRb;
    private Vector3 m_BallStartingPos;
    private float GOAL_REWARD = 5.0f;

    #region Publics

    // This function returns the target position for the agent based on its role
    // I put approximate values based on the field size in the prefab
    public Vector3 GetTargetFormationPosition(AgentSoccer agent)
    {
        if (agent.team == Team.Blue)
        {
            if (agent.role == PlayerRole.Defender) return new Vector3(-13, 0, 0);
            if (agent.role == PlayerRole.Midfielder) return new Vector3(-0f, 0, 0);
            if (agent.role == PlayerRole.Striker) return new Vector3(13f, 0, 0);
        }
        else // Purple  Team
        {
            if (agent.role == PlayerRole.Defender) return new Vector3(13f, 0, 0);
            if (agent.role == PlayerRole.Midfielder) return new Vector3(0f, 0, 0);
            if (agent.role == PlayerRole.Striker) return new Vector3(-13f, 0, 0);
        }
        return Vector3.zero; // Default fallback
    }

    public void ResetBall()
    {
        var randomPosX = Random.Range(-2.5f, 2.5f);
        var randomPosZ = Random.Range(-2.5f, 2.5f);

        ball.transform.position = m_BallStartingPos + new Vector3(randomPosX, 0f, randomPosZ);
        ballRb.linearVelocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;

    }

    public void GoalTouched(Team scoredTeam)
    {
        if (scoredTeam == Team.Blue)
        {
            m_BlueAgentGroup.AddGroupReward(GOAL_REWARD - (float)m_ResetTimer / MaxEnvironmentSteps);
            m_PurpleAgentGroup.AddGroupReward(-GOAL_REWARD);
        }
        else
        {
            m_PurpleAgentGroup.AddGroupReward(GOAL_REWARD - (float)m_ResetTimer / MaxEnvironmentSteps);
            m_BlueAgentGroup.AddGroupReward(-GOAL_REWARD);
        }
        m_PurpleAgentGroup.EndGroupEpisode();
        m_BlueAgentGroup.EndGroupEpisode();
        _scoreBoard.TeamScored(scoredTeam);
        StartCoroutine(_fieldController.ColorizeFieldWalls(scoredTeam, 0.6f));
        ResetScene();
    }

    public void ResetScene()
    {
        m_ResetTimer = 0;

        //Reset Agents
        foreach (var item in AgentsList)
        {
            var randomPosX = Random.Range(-5f, 5f);
            var newStartPos = item.Agent.initialPos + new Vector3(randomPosX, 0f, 0f);
            var rot = item.Agent.rotSign * Random.Range(80.0f, 100.0f);
            var newRot = Quaternion.Euler(0, rot, 0);
            item.Agent.transform.SetPositionAndRotation(newStartPos, newRot);

            item.Rb.linearVelocity = Vector3.zero;
            item.Rb.angularVelocity = Vector3.zero;
        }

        ResetBall();
    }

    #endregion


    #region Privates

    private void Start()
    {
        _fieldController = GetComponentInChildren<FieldController>();
        // Initialize TeamManager
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_PurpleAgentGroup = new SimpleMultiAgentGroup();
        ballRb = ball.GetComponent<Rigidbody>();
        m_BallStartingPos = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z);

        ///<note-to-do>
        /// I might change this from AgentsList to Blue and Purple Agents List to find some better way to pass Agent Groups and assign rewards to them
        ///</note-to-do>
        foreach (var item in AgentsList)
        {
            item.StartingPos = item.Agent.transform.position;
            item.StartingRot = item.Agent.transform.rotation;
            item.Rb = item.Agent.GetComponent<Rigidbody>();
            if (item.Agent.team == Team.Blue)
            {
                m_BlueAgentGroup.RegisterAgent(item.Agent);
                item.Agent.InitializeAgent(m_BlueAgentGroup, m_PurpleAgentGroup);
            }
            else
            {
                m_PurpleAgentGroup.RegisterAgent(item.Agent);
                item.Agent.InitializeAgent(m_PurpleAgentGroup, m_BlueAgentGroup);
            }
        }
        _scoreBoard.ResetScores();
        ResetScene();
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_BlueAgentGroup.GroupEpisodeInterrupted();
            m_PurpleAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    #endregion
}
