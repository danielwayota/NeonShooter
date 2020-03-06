using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider healthBar;

    public Text killStreakLabel;

    public int killStreakMax = 64;
    public Gradient killStreakGradient;


    public void UpdateHealth(float percent)
    {
        this.healthBar.value = percent;
    }

    public void UpdateKillStreak(int amount)
    {
        this.killStreakLabel.text = amount.ToString();

        amount = Mathf.Clamp(amount, 0, this.killStreakMax);

        float gradIndex = amount / (float) this.killStreakMax;

        Color color = this.killStreakGradient.Evaluate(gradIndex);

        this.killStreakLabel.color = color;
    }
}
