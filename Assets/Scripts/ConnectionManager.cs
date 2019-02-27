using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkLibrary;
using NetworkLibrary.MessageElements;

public class ConnectionManager
{
    private Destination destination;
    private UDPSocket socket;
    private ReliableUDPConnection connection;
    public Queue<HealthElement> healthElements;

    private ConnectionManager()
    {
        CreateSocketUDP();

        ConnectReliableUDP();

        healthElements = new Queue<HealthElement>();

        uint ip = 3232235537;
        //uint ip =

        destination = new Destination((uint)System.Net.IPAddress.HostToNetworkOrder((int)ip), (UInt16)System.Net.IPAddress.HostToNetworkOrder((short)8000));

        //Debug.Log(System.Net.IPAddress.HostToNetworkOrder((int)ip));

    }

    public static ConnectionManager Instance
    {
        get
        {
            return NestedConnectionManager.instance;
        }
    }

    private class NestedConnectionManager
    {
        static NestedConnectionManager() { }
        internal static readonly ConnectionManager instance = new ConnectionManager();
    }

    /*** Public functions ***/
    public void CreateSocketUDP()
    {
        socket = new UDPSocket();
        socket.Bind();
    }

    public void ConnectReliableUDP()
    {
        connection = new ReliableUDPConnection();
    }

    public void SendPacket(Packet packet)
    {
        socket.Send(packet, destination);
    }

    public Packet ReceivePacket()
    {
        Packet temp = socket.Receive();
        Debug.Log("Receive Packet:" + temp.Data[3]);
        return temp;
        //return socket.Receive();
    }

    public Packet Packetize(List<UpdateElement> reliableElements, List<UpdateElement> unreliableElements)
    {
        return connection.CreatePacket(unreliableElements, reliableElements);
    }

    public UnpackedPacket UnPack(Packet packet, ElementId[] expectedUnreliableIds)
    {
        return connection.ProcessPacket(packet, expectedUnreliableIds);
    }
}
