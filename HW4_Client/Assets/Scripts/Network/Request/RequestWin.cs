using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestWin: NetworkRequest
{
	public RequestWin()
	{
		request_id = Constants.CMSG_Win;
			}

	public void send()
	{
		Debug.Log("send" + packet);
		packet = new GamePacket(request_id);
		
	}
}
