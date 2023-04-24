using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseTestEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int x { get; set; } // The x coordinate of the target location


	public ResponseTestEventArgs()
	{
		event_id = Constants.SMSG_Test;
	}
}

public class ResponseTest : NetworkResponse
{
	private int user_id;
	private int x;

	public ResponseTest()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		x = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseTestEventArgs args = new ResponseTestEventArgs
		{
			user_id = user_id,
			x = x,
		};

		return args;
	}
}
