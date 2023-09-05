using UnityEngine;

public class Cannibal : EnemyBase
{
    private void Start()
    {
        Debug.Log(_enemyHealth);
    }

    protected override void Movement()
    {
        throw new System.NotImplementedException();
    }
}
