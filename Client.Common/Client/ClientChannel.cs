using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Common;

public class ClientMessage
{
    public MessageType Type;
    public DateTime Time;
    public string Text;
    public string Speaker;

    public ClientMessage()
    {
        Time = DateTime.Now;
    }

    public ClientMessage(MessageType type, DateTime time, string text)
    {
        Type = type;
        Time = time;
        Speaker = "";
        Text = text;
    }

    public ClientMessage(MessageType type, DateTime time, string speaker, string text)
    {
        Type = type;
        Time = time;
        Speaker = speaker;
        Text = text;
    }

    public string Message
    {
        get
        {
            return Time.Hour + ":" + Time.Minute + " " + (Speaker != "" ? Speaker + ": " : "") + Text;
        }
    }
}

public class ClientChannel
{
    public ushort ID;
    public string Name;

    public List<ClientMessage> Messages = new List<ClientMessage>();

    public ClientChannel(ushort ChannelID, string ChannelName)
    {
        ID = ChannelID;
        Name = ChannelName;
    }

    public void Add(ClientMessage Message)
    {
        Messages.Add(Message);
    }
}
