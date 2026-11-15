using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ProjectRimba.Player;

namespace ProjectRimba.Systems
{
    public class StageFlowManager : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private GoalZone goalZone;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private Text resultTitleText;
        [SerializeField] private Button retryButton;

        private void Awake()
        {
            Time.timeScale = 1f;
            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if (playerHealth != null) playerHealth.OnDied += HandlePlayerDied;
            if (goalZone != null) goalZone.OnGoalReached += HandleGoalReached;
            if (retryButton != null) retryButton.onClick.AddListener(RetryStage);
        }

        private void OnDisable()
        {
            if (playerHealth != null) playerHealth.OnDied -= HandlePlayerDied;
            if (goalZone != null) goalZone.OnGoalReached -= HandleGoalReached;
            if (retryButton != null) retryButton.onClick.RemoveListener(RetryStage);
        }

        private void HandlePlayerDied()
        {
            ShowResult("ORANG UTAN TUMBANG");
        }

        private void HandleGoalReached()
        {
            ShowResult("STAGE CLEAR!");
        }

        private void ShowResult(string title)
        {
            if (resultTitleText != null) resultTitleText.text = title;
            if (resultPanel != null) resultPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void RetryStage()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
