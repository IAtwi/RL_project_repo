using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] private Material _wallsDefaultMaterial;
    [SerializeField] private List<MeshRenderer> _wallMeshRenderers;

    private SoccerSettings m_SoccerSettings;
    //private BoxCollider _fieldBoxCollider;

    #region Privates

    private void Start()
    {
        m_SoccerSettings = FindAnyObjectByType<SoccerSettings>();
        //_fieldBoxCollider = GetComponent<BoxCollider>();
    }

    private void ResetTeam(List<PlayerInfo> teamPlayers, bool isXPositive)
    {
        int xMultiplier = isXPositive ? 1 : -1;
        float minOffsetFromHalfwayLine = 3.4f;
        float distanceBetweenHorizontalLines = 3;
        int playerIndex = 0, playersCount = teamPlayers.Count;
        int horizontalLineIndex = 0;

        while (playerIndex < playersCount)
        {
            float horizontalLineOffsetFromHalfwayLine = xMultiplier * (minOffsetFromHalfwayLine + horizontalLineIndex++ * distanceBetweenHorizontalLines);
            int playersLeft = playersCount - playerIndex;
            float z = 0;

            if (playersLeft == 1)
            {
                ResetPlayer(teamPlayers[playerIndex++], new Vector3(horizontalLineOffsetFromHalfwayLine, 0.5f, z));
            }
            else
            {
                z = Random.Range(-4.5f, -1f);
                ResetPlayer(teamPlayers[playerIndex++], new Vector3(horizontalLineOffsetFromHalfwayLine, 0.5f, z));
                ResetPlayer(teamPlayers[playerIndex++], new Vector3(horizontalLineOffsetFromHalfwayLine, 0.5f, -z));
            }
        }
    }

    private void ResetPlayer(PlayerInfo playerInfo, Vector3 position)
    {
        Transform playerTransform = playerInfo.Agent.transform;
        playerTransform.localPosition = position;
        Vector3 direction = (Vector3.zero - position).normalized;
        // Calculate the rotation that only affects the Y-axis
        direction.y = 0; // Set the Y component to 0 to ignore vertical rotation
        // Check if the direction is not zero to avoid invalid LookRotation
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction); // Only rotates on the Y-axis
            playerTransform.rotation = targetRotation; // Apply the rotation
        }

        Rigidbody playerRigidbody = playerInfo.Rb;
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
    }

    // to be fixed an used
    //private Vector3 GetRandomPositionOnHorizontalLine(int horizontalLineIndex, int xMultiplier)
    //{
    //    Bounds bounds = _fieldBoxCollider.bounds; // Get world-space bounds

    //    float fieldHalfLength = bounds.extents.x; // Half the field length
    //    float fieldHalfWidth = bounds.extents.z;  // Half the field width

    //    float fixedX = xMultiplier * (bounds.center.x + (fieldHalfLength * 0.15f) * (horizontalLineIndex + 1));
    //    float minZ = bounds.center.z + (fieldHalfWidth * 0.10f); // 10% of half-width
    //    float maxZ = bounds.center.z + (fieldHalfWidth * 0.35f); // 35% of half-width

    //    float randomZ = Random.Range(minZ, maxZ); // Random Z in the given range
    //    float y = 0.5f; // Fixed Y value

    //    return new Vector3(fixedX, y, randomZ);
    //}

    #endregion


    #region Publics

    public IEnumerator ColorizeFieldWalls(Team scoredTeam, float duration)
    {
        Material targetedMaterial = scoredTeam == Team.Blue ? m_SoccerSettings.blueMaterial : m_SoccerSettings.purpleMaterial;

        foreach (MeshRenderer meshRenderer in _wallMeshRenderers)
            meshRenderer.sharedMaterial = targetedMaterial;

        yield return new WaitForSeconds(duration);

        foreach (MeshRenderer meshRenderer in _wallMeshRenderers)
            meshRenderer.sharedMaterial = _wallsDefaultMaterial;
    }

    public void ResetAgents(IEnumerable<PlayerInfo> agentsList)
    {
        List<PlayerInfo>[] teams = new List<PlayerInfo>[2];

        foreach (PlayerInfo player in agentsList)
        {
            int teamId = (int)player.Agent.team;
            if (teams[teamId] == null)
                teams[teamId] = new List<PlayerInfo>();

            teams[teamId].Add(player);
        }

        ResetTeam(teams[0], false);
        ResetTeam(teams[1], true);
    }

    #endregion

}
