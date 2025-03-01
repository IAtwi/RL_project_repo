using UnityEngine;

public class SoccerBallController : MonoBehaviour
{
    public GameObject area;

    private SoccerEnvController envController;
    private SoccerSettings m_SoccerSettings;

    void Start()
    {
        envController = area.GetComponent<SoccerEnvController>();
        m_SoccerSettings = FindAnyObjectByType<SoccerSettings>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(m_SoccerSettings.purpleGoalTag)) //ball touched purple goal
            envController.GoalTouched(Team.Blue);
        else if (col.gameObject.CompareTag(m_SoccerSettings.blueGoalTag)) //ball touched blue goal
            envController.GoalTouched(Team.Purple);
    }
}
