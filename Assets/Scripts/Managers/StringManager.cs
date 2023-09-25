// Handles a collection of strings globally as one instance that can be referenced.
using UnityEngine;

public class StringManager : MonoSingleton<StringManager>
{
    // Tags
    public const string columnTag = "Column";
    public const string consoleTag = "Console";
    public const string floorTag = "Floor";
    public const string wallTag = "Wall";

    public const string cannibal = "Cannibal";
    public const string golem = "Golem";
    public const string ghoul = "Ghoul";
    public const string tank = "Tank";
    public const string warlock = "Warlock";

    public const string bullet = "Bullet";
    public const string enemy = "Enemy";
    public const string player = "Player";


    // Methods to search through tags
    public void SwitchThroughTags(RaycastHit hitInfo)
    {
        switch (hitInfo.collider.tag)
        {
            case columnTag:
            case consoleTag:
            case floorTag:
            case wallTag:
                break;

            case cannibal:
                DebugHitTarget(cannibal);
                break;
            case golem:
                DebugHitTarget(golem);
                break;
            case ghoul:
                DebugHitTarget(ghoul);
                break;
            case tank:
                DebugHitTarget(tank);
                break;
            case warlock:
                DebugHitTarget(warlock);
                break;

            case bullet:
                break;
            case enemy:
                break;
            case player:
                break;
            default:
                break;
        }
    }

    private void DebugHitTarget(string target)
    {
        Debug.Log("You hit a: " +  target);
    }
}
