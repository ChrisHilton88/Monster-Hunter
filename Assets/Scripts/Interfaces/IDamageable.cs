// IDamageable looks after all game objects that are able to receive damage

using System;

public interface IDamageable 
{
    void ReceiveDamage(int damageReceived);
}
