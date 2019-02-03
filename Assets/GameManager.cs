using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkLibrary;
using NetworkLibrary.MessageElements;

public class GameManager : MonoBehaviour
{ 
    private List<UpdateElement> elements;
    private int health;

    private int enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        enemyHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ++health;
        }
        else
        {
            --health;
        }
        elements = new List<UpdateElement>();
        elements.Add(new HealthElement(health, 0));

        Packet healthPacket = ConnectionManager.Instance.Packetize(elements, elements);
        ConnectionManager.Instance.SendPacket(healthPacket);

        Packet enemyHealthPacket = ConnectionManager.Instance.ReceivePacket();
        UnpackedPacket unpackedEnemyHealth = ConnectionManager.Instance.UnPack(enemyHealthPacket, 
            new ElementId[]{ ElementId.HealthElement });

        //call some update function to change the text display
    }
}
