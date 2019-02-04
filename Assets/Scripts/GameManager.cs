using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using NetworkLibrary;
using NetworkLibrary.MessageElements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Constants
    private const string HERO_HEALTH_TAG = "HeroHealth";
    private const string ENEMY_HEALTH_TAG = "EnemyHealth";
    private const int HERO_INITIAL_HEALTH = 10;
    private const int ENEMY_INITIAL_HEALTH = 10;
    private const int ACTOR_ID = 1;
    #endregion

    #region Properties
    public int Health
    {
        get { return _health; }
        set 
        { 
            _health = value;
            _updateTextElement(HERO_HEALTH_TAG, $"Hero Health: {_health}");
        }
    }

    public int EnemyHealth
    {
        get { return _enemyHealth; }
        set
        {
            _enemyHealth = value;
            _updateTextElement(ENEMY_HEALTH_TAG, $"Enemy Health: {_enemyHealth}");
        }
    }
    #endregion

    private List<UpdateElement> _elements;
    private List<UpdateElement> _empty = new List<UpdateElement>();
    private int _health;
    private int _enemyHealth;

    public void UpdateHealth(HealthElement healthElement)
    {
        if (healthElement.ActorId == ACTOR_ID)
        {
            Health = healthElement.Health;
        }
        else
        {
            EnemyHealth = healthElement.Health;
        }
    }

    private void _updateTextElement(string elementTag, string message)
    {
        GameObject.Find(elementTag).GetComponent<Text>().text = message;
    }

    // Creates the server
    void ServerThreadCall() => new Server(this);

    // Start is called before the first frame update
    void Start()
    {
        Health = HERO_INITIAL_HEALTH;
        EnemyHealth = ENEMY_INITIAL_HEALTH;

        // Create separate thread for server
        new Thread(new ThreadStart(ServerThreadCall)).Start();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ++Health;
        }
        //else
        //{
        //    --Health;
        //}

        _elements = new List<UpdateElement>
        {
            new HealthElement(ACTOR_ID, Health)
        };

        Packet healthPacket = ConnectionManager.Instance.Packetize(_empty, _elements);
        ConnectionManager.Instance.SendPacket(healthPacket);


        /*********************************************************
         Blocking call at ReceivePacket:
         -Need to modify dll to use non-blocking for this to work
         -Server also needs to be up to test the receive function
         -Need to modify the destination in ConnectionManager       
         *********************************************************/

        //Packet enemyHealthPacket = ConnectionManager.Instance.ReceivePacket();
        //UnpackedPacket unpackedEnemyHealth = ConnectionManager.Instance.UnPack(enemyHealthPacket, 
        //new ElementId[]{ ElementId.HealthElement });

        //call some update function to change the text display
    }
}
