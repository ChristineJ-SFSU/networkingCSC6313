using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseWinEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int test;

	public ResponseWinEventArgs()
	{
		event_id = Constants.SMSG_Win;
	}
}

public class ResponseWin : NetworkResponse
{
	private int user_id;
	private int test;

	public ResponseWin()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		test  = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseWinEventArgs args = new ResponseWinEventArgs
		{
			user_id = user_id,
			test = test

		};

		return args;
	}
}
