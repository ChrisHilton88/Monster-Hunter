// Responsible for the environmental barrier game object behaviours
public class BarrierBehaviour : DamageableBase
{
    // Sealed modifier - Allows for no further changes to that method if inheriting this class
    // Barrier specific initialisation values
    private int _barrierMaxHealth = 500;            

    private float _barrierIdleRepairTimer = 5f;            
    private float _barrierRepairMultiplier = 10f;



    #region Initialisation
    protected sealed override void Start()
    {
        base.Start(); 
        Initialisation();
    }

    protected sealed override void Initialisation()
    {
        MaxHealth = _barrierMaxHealth;
        IdleRepairTimer = _barrierIdleRepairTimer;
        RepairMultiplier = _barrierRepairMultiplier;
    }
    #endregion
}
