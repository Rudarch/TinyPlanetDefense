using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseRotationSpeedUpgrade", menuName = "Upgrades/IncreaseRotationSpeed")]
public class IncreaseRotationSpeedUpgrade : CannonUpgrade
{
    public float speedBoost = 90f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);
        var controller = cannon.GetComponent<CannonController>();
        if (controller != null)
        {
            controller.rotationSpeed += speedBoost;
        }
    }

    public override string GetEffectText()
    {
        return $"+{speedBoost}°/s Rotation Speed";
    }
}