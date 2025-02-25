using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _blueScore;
    [SerializeField] private TextMeshProUGUI _purpleScore;
    [SerializeField] private TextMeshProUGUI cinemaText;

    #region Publics

    public void TeamScored(Team scoredTeam)
    {
        UpdateScore(scoredTeam, out string newScore);

        //StartCoroutine(DisplayCinemaForSeconds(newScore, scoredTeam == Team.Purple ?
        //    new Color(228, 0, 255) : new Color(0, 33, 255),
        //    1f));
        //StartCoroutine(SlowMotionEffect(1f, 0f));
    }

    #endregion

    #region Privates

    private void UpdateScore(Team scoredTeam, out string newScore)
    {
        TextMeshProUGUI targetedScore;

        if (scoredTeam == Team.Purple)
            targetedScore = _blueScore;
        else
            targetedScore = _purpleScore;

        newScore = (int.Parse(targetedScore.text) + 1).ToString();
        targetedScore.text = newScore;
    }

    private IEnumerator DisplayCinemaForSeconds(string message, Color color, float duration)
    {
        cinemaText.color = color;
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
