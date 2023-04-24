using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseColorChangeEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int piece_idx { get; set; } // The index of the piece to move. Belongs to player with id user_id


	public ResponseColorChangeEventArgs()
	{
		event_id = Constants.SMSG_ColorChange;
	}
}

public class ResponseColorChange : NetworkResponse
{
	private int user_id;
	private int piece_idx;


	public ResponseColorChange()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		piece_idx = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseColorChangeEventArgs args = new ResponseColorChangeEventArgs
		{
			user_id = user_id,
			piece_idx = piece_idx,
		};

		return args;
	}
}
