using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _blueScore;
    [SerializeField] private TextMeshProUGUI _purpleScore;
    [SerializeField] private TextMeshProUGUI cinemaText;

    private Color _blueColor = new(32 / 255f, 147 / 255f, 237 / 255f);
    private Color _purpleColor = new(139 / 255f, 107 / 255f, 195 / 255f);

    #region Publics

    public void TeamScored(Team scoredTeam)
    {
        if (_blueScore == null || _purpleScore == null) // no scoreboard
            return;
        UpdateScore(scoredTeam, out string newScore);

        if (cinemaText != null)
            StartCoroutine(DisplayCinemaForSeconds(newScore, scoredTeam, 0.5f));

        //StartCoroutine(SlowMotionEffect(1f, 0f));
    }

    #endregion

    #region Privates

    private void UpdateScore(Team scoredTeam, out string newScore)
    {
        TextMeshProUGUI targetedScore = scoredTeam == Team.Blue ? _blueScore : _purpleScore;
        newScore = (int.Parse(targetedScore.text) + 1).ToString();
        targetedScore.text = newScore;
    }

    private IEnumerator DisplayCinemaForSeconds(string message, Team scoredTeam, float duration)
    {
        cinemaText.color = scoredTeam == Team.Blue ? _blueColor : _purpleColor;
        cinemaText.text = message;
        cinemaText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        cinemaText.gameObject.SetActive(false);
    }

    private IEnumerator SlowMotionEffect(float duration, float slowFactor)
    {
        Time.timeScale = slowFactor;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    #endregion
}
