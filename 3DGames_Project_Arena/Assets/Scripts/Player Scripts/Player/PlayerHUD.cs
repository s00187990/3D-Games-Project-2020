
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public float imageDamageTime = 0.5f;
    public Image TakingDamageImage;
    public Health playerHealth;
    public TextMeshProUGUI KeillsTextMesh;
    public TextMeshProUGUI WeaponNameTextMesh;
    public TextMeshProUGUI WeaponDurabilityTextMesh;
    public Image weaponImage;
    Weapon CurrentWeapon;
    int kills;

    private void OnEnable()
    {
        LevelManager.OnEnemyDied += PlayerHUD_OnEnemyDied;
        PlayerAttack.OnPlayerFired += PlayerAttack_OnPlayerFired;
        PlayerAttack.OnPlayerWeaponDroped += PlayerAttack_OnPlayerWeaponDroped;
        playerHealth.OnTakingDamage += PlayerHUD_OnTakingDamage;
        Pickup.OnPickup += PlayerPickup_OnPickup;
        //playerHealth.OnDeath += PlayerHealth_OnDeath;
    }

    private void PlayerAttack_OnPlayerWeaponDroped()
    {
        WeaponNameTextMesh.text = "Hand";
        WeaponDurabilityTextMesh.text = "Hits: -";
        weaponImage.sprite = null;
    }

    private void PlayerAttack_OnPlayerFired(int hitCounter)
    {
        WeaponDurabilityTextMesh.text = (( CurrentWeapon.Durability - hitCounter ) <= 0) ? "Hits: -" : "Hits: " + (CurrentWeapon.Durability - hitCounter).ToString();
    }

    private void PlayerPickup_OnPickup(GameObject pickedfUpWeapon)
    {
        CurrentWeapon = pickedfUpWeapon.GetComponent<Weapon>();
        WeaponNameTextMesh.text = CurrentWeapon.Name;
        WeaponDurabilityTextMesh.text = "Hits: " + CurrentWeapon.Durability.ToString();
        weaponImage.sprite = CurrentWeapon.Image;
    }

    private void PlayerHUD_OnTakingDamage(float amount)
    {
        CancelInvoke("DisableDamageImage");
        TakingDamageImage.enabled = true;
        Invoke("DisableDamageImage", imageDamageTime * amount);
        //print("Player Taking damag : " + amount);

    }

    private void PlayerHUD_OnEnemyDied(GameObject gameObject)
    {
        kills++;
        KeillsTextMesh.text = "Kills: " + kills;
    }
    void DisableDamageImage()
    {
        TakingDamageImage.enabled = false;
    }


    private void OnDisable()
    {
        LevelManager.OnEnemyDied -= PlayerHUD_OnEnemyDied;
        PlayerAttack.OnPlayerFired -= PlayerAttack_OnPlayerFired;
        PlayerAttack.OnPlayerWeaponDroped -= PlayerAttack_OnPlayerWeaponDroped;
        playerHealth.OnTakingDamage -= PlayerHUD_OnTakingDamage;
        Pickup.OnPickup -= PlayerPickup_OnPickup;
    }

}
