﻿using System.IO;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine;

public static partial class Util_net
{
    public const ushort TCP_CHUNK = 1024;

    //----------------------------------------------------------------------------------------------------------

    public static void TryTCP_paragon(Action onError, Action<TcpClient, BinaryWriter, BinaryReader> onCommunication) => TryTCP(DOMAIN_3VE, PORT_PARAGON, true, onError, onCommunication);
    public static void TryTCP(in string host, in ushort port, in bool log, in Action onError, in Action<TcpClient, BinaryWriter, BinaryReader> onCommunication)
    {
        string endPoint = $"{{ {host}:{port} }}";

        if (log)
            Debug.Log($"TCP connecting to: {endPoint}".ToSubLog());

        try
        {
            double t1 = Util.TotalMilliseconds;
            using TcpClient socket = new(host, port);

            if (socket.Connected)
            {
                using NetworkStream stream = socket.GetStream();
                using BinaryWriter writer = new(stream, Encoding.UTF8);
                using BinaryReader reader = new(stream, Encoding.UTF8);

                double t2 = Util.TotalMilliseconds - t1;

                if (log)
                    Debug.Log($"TCP connected to: {endPoint} ({t2.MillisecondsLog()})".ToSubLog());

                t1 = Util.TotalMilliseconds;
                onCommunication(socket, writer, reader);

                t2 = Util.TotalMilliseconds - t1;

                if (log)
                    Debug.Log($"TCP closing: {endPoint} ({t2.MillisecondsLog()})".ToSubLog());

                return;
            }
            else
                Debug.LogWarning($"TCP connection failure {endPoint}");
        }
        catch (SocketException e)
        {
            Debug.LogWarning($"{nameof(SocketException)} {endPoint} \"{e.Message.TrimEnd('\n', '\r', '\t', '\0')}\"");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        onError?.Invoke();
    }
}