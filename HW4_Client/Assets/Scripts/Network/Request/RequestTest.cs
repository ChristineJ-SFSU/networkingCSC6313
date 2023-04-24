using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestTest : NetworkRequest
{
	public RequestTest()
	{
		request_id = Constants.CMSG_Test;
	}

	public void send(int x)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(x);
	}
}
