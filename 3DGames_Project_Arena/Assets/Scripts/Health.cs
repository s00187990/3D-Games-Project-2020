using UnityEngine;

public delegate void Death();
public delegate void TakingDamage(float amount);
public class Health : MonoBehaviour
{
    public float FullHealth;
    [SerializeField]
    public float currentHealth { get; private set; }
    public event Death OnDeath;
    public event TakingDamage OnTakingDamage ;
    bool Alive = true;
    private void Start()
    {
        currentHealth = FullHealth;
    }
    public void TakeDamage(float amount)
    {
        if (!Alive) return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, -0.1f, FullHealth);
        if (OnTakingDamage != null)
            OnTakingDamage(amount);
        if (currentHealth <= 0)
        {
            Alive = false;
            if (OnDeath != null)
                OnDeath();
        }
    }



}
