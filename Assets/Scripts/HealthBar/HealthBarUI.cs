using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarForegroundImage;

    public void UpdateHealthBar(float persentaseHP)
    {
        if (healthBarForegroundImage != null)
        {
            healthBarForegroundImage.fillAmount = persentaseHP;
        }
    }
}

