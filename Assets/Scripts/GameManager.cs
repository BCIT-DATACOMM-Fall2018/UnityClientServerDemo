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
    public int Health
    {
        get { return _health; }
        set 
        { 
            _health = value;
            GameObject.Find("HeroHealth").GetComponent<Text>().text = "Hero Health: " + _health;
        }
    }

    public int EnemyHealth
    {
        get { return _enemyHealth; }
        set
        {
            _enemyHealth = value;
            GameObject.Find("EnemyHealth").GetComponent<Text>().text = "Enemy Health: " + _enemyHealth;
        }
    }

    private List<UpdateElement> _elements;
    private int _actorID = 1;
    private int _health;
    private int _enemyHealth;

    // Creates the server
    void ServerThreadCall() => new Server(this);

    public void UpdateHealth(HealthElement healthElement)
    {
        if (healthElement.ActorId == _actorID)
        {
            Health = healthElement.Health;
        }
        else
        {
            EnemyHealth = healthElement.Health;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = 10;
        EnemyHealth = 10;

        Console.WriteLine("Starting");

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
            new HealthElement(_actorID, Health)
        };

        //Packet healthPacket = ConnectionManager.Instance.Packetize(elements, elements);
        //ConnectionManager.Instance.SendPacket(healthPacket);

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
