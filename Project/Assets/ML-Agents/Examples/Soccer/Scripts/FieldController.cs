using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _wallMeshRenderers;

    private SoccerSettings m_SoccerSettings;


    #region Privates

    private void Start()
    {
        m_SoccerSettings = FindAnyObjectByType<SoccerSettings>();
    }

    #endregion


    #region Publics

    public IEnumerator ColorizeFieldWalls(Team scoredTeam, float duration)
    {
        Material[] initialMaterials = _wallMeshRenderers.Select(it => it.material).ToArray();
        Material targetedMaterial = scoredTeam == Team.Blue ? m_SoccerSettings.blueMaterial : m_SoccerSettings.purpleMaterial;

        foreach (MeshRenderer meshRenderer in _wallMeshRenderers)
            meshRenderer.material = targetedMaterial;

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < _wallMeshRenderers.Count; i++)
        {
            MeshRenderer meshRenderer = _wallMeshRenderers[i];
            meshRenderer.material = initialMaterials[i];
        }
    }

    #endregion
}
