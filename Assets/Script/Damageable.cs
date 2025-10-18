using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    
    [SerializeField] private int _health = 100;        
    [SerializeField] private bool _isAlive = true;   
    [SerializeField] private int defence = 0;          

    
    public int Health
    {
        get
        {
            return _health;  // Return current health when accessed
        }
        set
        {
            _health = value; // Set new health value

            
            if (_health <= 0)
            {
                _health = 0;         
                IsAlive = false;     // Update living state to dead
                Destroy(gameObject); 
            }
        }
    }

    
    public bool IsAlive
    {
        get
        {
            return _isAlive; // Return current alive status
        }
        set
        {
            _isAlive = value; 
        }
    }

    
    public void Hit(int damage)
    {
        
        if (IsAlive)
        {
            
            // Mathf.Max ensures damage never goes below 0
            int actualDamage = Mathf.Max(damage - defence, 0);

           
            
            Health -= actualDamage;
        }
    }

    
    private void Start()
    {
        _health = Health; 
    }
}