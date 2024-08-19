using TMPro;
using UnityEngine;

public class endGameScript : MonoBehaviour
{
    public TMP_Text gameStatus;
    public TMP_Text endGameDes;

    void Start()
    {
        switch (PlayerPrefs.GetInt("endGameStatus")) {
            case 1:
                gameStatus.text = "Congratulations, you have completed the experiment.";
                endGameDes.text = "Your research propelled humanity into a fully digitalized future. Human consciousness was integrated into digital systems, leading to incredible advancements. Society no longer needed the physical body. \n\n However, this progress had a devastating cost. A powerful virus infiltrated these systems, wiping out most of the population. What began as a step toward evolution turned into near-extinction, leaving a few survivors trapped in the digital realm. \n\n Your work changed the world, but at an unimaginable price. The future of humanity now hangs by a thread.";
                return;
            case 2:
                gameStatus.text = "Raid started!";
                endGameDes.text = "Subjects of the test started a big raid, which destroyed your experiment.";
                return;
            case 3:
                gameStatus.text = "Goverment no longer interested.";
                endGameDes.text = "The experiment had no results for too long, so goverment is no longer interested and stopped funding.";
                return;
        }
    }
}
