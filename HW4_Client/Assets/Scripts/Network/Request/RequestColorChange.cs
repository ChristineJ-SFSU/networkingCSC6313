using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestColorChange : NetworkRequest
{
    public RequestColorChange()
    {
        request_id = Constants.CMSG_ColorChange;
    }

    public void send(int pieceIndex)
    {
        packet = new GamePacket(request_id);
        packet.addInt32(pieceIndex);

    }
}