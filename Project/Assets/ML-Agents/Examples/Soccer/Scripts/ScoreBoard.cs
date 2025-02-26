using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _blueScore;
    [SerializeField] private TextMeshProUGUI _purpleScore;

    #region Publics

    public void TeamScored(Team scoredTeam)
    {
        TextMeshProUGUI targetedScore = scoredTeam == Team.Blue ? _blueScore : _purpleScore;
        targetedScore.text = (int.Parse(targetedScore.text) + 1).ToString();
    }

    public void ResetScores()
    {
        _purpleScore.text = "0";
        _blueScore.text = "0";
    }

    #endregion

}
