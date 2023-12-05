using UnityEngine;

public class HealthView : View
{
    public void OnHealthChanged(float health)
    {
        ChangeText(health.ToString());
    }
}