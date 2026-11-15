using UnityEngine;
using UnityEngine.UI;
using ProjectRimba.Player;

namespace ProjectRimba.UI
{
    public class HudController : MonoBehaviour
    {
        [Header("Data Sources")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private PlayerAdrenaline playerAdrenaline;
        [SerializeField] private OrangutanController playerController;

        [Header("UI Elements")]
        [SerializeField] private Image[] leafIcons;
        [SerializeField] private Image adrenalineFill;
        [SerializeField] private Image healProgressFill;
        [SerializeField] private Image pythonSkillIcon;

        [Header("Colors")]
        [SerializeField] private Color activeLeafColor = new Color(0.25f, 0.85f, 0.35f, 1f);
        [SerializeField] private Color lostLeafColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        [SerializeField] private Color unlockedSkillColor = new Color(0.2f, 0.95f, 0.55f, 1f);
        [SerializeField] private Color lockedSkillColor = new Color(0.2f, 0.2f, 0.2f, 0.45f);

        private void OnEnable()
        {
            if (playerHealth != null) playerHealth.OnHealthChanged += UpdateHealth;
            if (playerAdrenaline != null)
            {
                playerAdrenaline.OnAdrenalineChanged += UpdateAdrenaline;
                playerAdrenaline.OnHealProgress += UpdateHealProgress;
            }
            if (playerController != null)
            {
                playerController.OnPythonUnlockedChanged += UpdatePythonSkill;
            }
        }

        private void Start()
        {
            if (playerHealth != null)
            {
                UpdateHealth(playerHealth.CurrentSegments, playerHealth.MaxSegments);
            }
            if (playerAdrenaline != null)
            {
                UpdateAdrenaline(playerAdrenaline.CurrentAdrenaline, playerAdrenaline.MaxAdrenaline);
                UpdateHealProgress(0f);
            }
            if (playerController != null)
            {
                UpdatePythonSkill(playerController.HasPythonSkill);
            }
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged -= UpdateHealth;
            }

            if (playerAdrenaline != null)
            {
                playerAdrenaline.OnAdrenalineChanged -= UpdateAdrenaline;
                playerAdrenaline.OnHealProgress -= UpdateHealProgress;
            }
            if (playerController != null)
            {
                playerController.OnPythonUnlockedChanged -= UpdatePythonSkill;
            }
        }

        private void UpdateHealth(int current, int max)
        {
            if (leafIcons == null) return;

            for (int i = 0; i < leafIcons.Length; i++)
            {
                if (leafIcons[i] != null)
                {
                    leafIcons[i].color = i < current ? activeLeafColor : lostLeafColor;
                }
            }
        }

        private void UpdateAdrenaline(float current, float max)
        {
            if (adrenalineFill != null && max > 0f)
            {
                adrenalineFill.fillAmount = Mathf.Clamp01(current / max);
            }
        }

        private void UpdateHealProgress(float progress)
        {
            if (healProgressFill != null)
            {
                healProgressFill.fillAmount = progress;
            }
        }

        private void UpdatePythonSkill(bool unlocked)
        {
            if (pythonSkillIcon != null)
            {
                pythonSkillIcon.color = unlocked ? unlockedSkillColor : lockedSkillColor;
            }
        }
    }
}
