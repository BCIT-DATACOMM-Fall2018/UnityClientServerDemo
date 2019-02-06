using System;
using System.Collections.Generic;
using NetworkLibrary;
using NetworkLibrary.MessageElements;

public class Server
{
    public Server(GameManager gameManager)
    {
        // Create a UDPSocket
        UDPSocket socket = new UDPSocket();

        // Bind the socket. Address must be in network byte order
        socket.Bind((ushort)System.Net.IPAddress.HostToNetworkOrder((short)8000));

        // Create a ReliableUDPConnection
        ReliableUDPConnection connection = new ReliableUDPConnection();

        while (true)
        {
            // Receive a packet. Receive calls block
            Packet packet = socket.Receive();

            Console.WriteLine("Got packet.");

            //// Unpack the packet using the ReliableUDPConnection
            //UnpackedPacket unpacked = connection.ProcessPacket(packet, new ElementId[] { ElementId.HealthElement });

            //// Iterate through the unreliable elements and call their UpdateState function.
            //foreach (var element in unpacked.UnreliableElements)
            //{
            //   //gameManager.UpdateHealth((HealthElement)element);

            //    List<UpdateElement> elements = new List<UpdateElement>
            //    {
            //        element
            //    };

            //    Console.WriteLine("Sending response packet.");

            //    // Create a new packet
            //    packet = connection.CreatePacket(elements, elements);

            //}

            // Send the packet
            socket.Send(packet, socket.LastReceivedFrom);
        }
    }
}
