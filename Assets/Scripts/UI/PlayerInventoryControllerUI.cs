using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryControllerUI : MonoBehaviour
{
    [Header("WEAPON INFO")]
    [SerializeField] private Image _weaponImage;
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private TextMeshProUGUI _weaponLevel;

    [SerializeField] private TextMeshProUGUI _weaponDamage;
    [SerializeField] private TextMeshProUGUI _weaponRange;

    public void Initialize(IPlayer player)
    {
        _weaponImage.sprite = player.GetWeapon().GetSprite();
        _weaponName.text = player.GetWeapon().GetName();
        _weaponLevel.text = $"Lv {player.GetWeapon().GetLevel()}";
        _weaponRange.text = player.GetWeapon().GetAttackRange().ToString();
        _weaponDamage.text = player.GetWeapon().GetAttackDamage().ToString();
    }
}
