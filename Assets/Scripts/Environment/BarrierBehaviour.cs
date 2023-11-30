using UnityEngine;

// Responsible for the environmental barrier game object behaviours
public class BarrierBehaviour : MonoBehaviour, IDamageable
{
    private int _health = 500;


    public void ReceiveDamage(int damageReceived)
    {
        _health -= damageReceived;
    }
}
