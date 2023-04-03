// Handles a collection of strings globally as one instance that can be referenced.
using System.Collections.Generic;

public class StringManager : MonoSingleton<StringManager>
{
    // Tags
    public const string _wallTag = "Wall";
    public const string _floorTag = "Floor";
    public const string _columnTag = "Column";
    public const string _enemy = "Enemy";
    public const string _player = "Player";
    public const string _bullet = "Bullet";

    

    //public List<string> _tagList = new List<string> ();


    //void Start()
    //{
    //    CreateStringList();
    //}


    //private void CreateStringList()
    //{
    //    _tagList.Add(_wallTag); 
    //    _tagList.Add(_floorTag); 
    //    _tagList.Add(_columnTag); 
    //    _tagList.Add(_enemy);    
    //    _tagList.Add (_player); 
    //}
}
