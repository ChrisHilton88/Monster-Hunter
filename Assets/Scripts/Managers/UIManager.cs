using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public void EnemyDisplay(int count)
    {
        string _displayEnemyCount = count.ToString();
        Debug.Log(_displayEnemyCount);
    }
}
