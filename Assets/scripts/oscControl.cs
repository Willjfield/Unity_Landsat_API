//
//	  UnityOSC - Example of usage for OSC receiver
//
//	  Copyright (c) 2012 Jorge Garcia Martin
//
// 	  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// 	  documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// 	  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
// 	  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// 	  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// 	  of the Software.
//
// 	  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// 	  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// 	  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// 	  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// 	  IN THE SOFTWARE.
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class oscControl : MonoBehaviour {
	
	private Dictionary<string, ServerLog> servers;
	private Dictionary<string, ClientLog> clients;

	public Vector3 obPos;

	// Script initialization
	void Start() {	
		OSCHandler.Instance.Init(); //init OSC
		servers = new Dictionary<string, ServerLog>();
		clients = new Dictionary<string,ClientLog> ();
		obPos.Set(0, 0, 0);
	}

	// NOTE: The received messages at each server are updated here
    // Hence, this update depends on your application architecture
    // How many frames per second or Update() calls per frame?
	void Update() {
		
		OSCHandler.Instance.UpdateLogs();
		servers = OSCHandler.Instance.Servers;
		clients = OSCHandler.Instance.Clients;

		//OSCHandler.Instance.SendMessageToClient ("TouchOSC Bridge", "/value", (int)4);
		
	    foreach( KeyValuePair<string, ServerLog> item in servers )
		{
			// If we have received at least one packet,
			// show the last received from the log in the Debug console
			if(item.Value.log.Count > 0) 
			{
				int lastPacketIndex = item.Value.packets.Count - 1;

				String messageAddress = item.Value.packets[lastPacketIndex].Address.ToString();
				Debug.Log("received event");
				if(messageAddress == "/touch"){
					Debug.Log("received touch");

					string receivedString = item.Value.packets[lastPacketIndex].Data[0].ToString();

					var charsToRemove = new string[] { "(", ")"};
					foreach (var c in charsToRemove)
					{
						receivedString = receivedString.Replace(c, string.Empty);
					}
					string[] strValues = receivedString.Split (',');
					float[] values = new float[strValues.Length];

					for (int i = 0; i< strValues.Length;i++)
					{
						values[i] = float.Parse(strValues[i]);
					}

					Debug.Log (values[0]);
					Debug.Log (values[1]);
					/*
					float valueX = float.Parse (item.Value.packets[lastPacketIndex].Data[0].ToString());
					float valueY = float.Parse (item.Value.packets[lastPacketIndex].Data[1].ToString());
					UnityEngine.Debug.Log("touch event: "+valueX+","+valueY);
					*/
					obPos.Set((values[0]/100)-5,0,(values[1]/100)-9);
					transform.position=obPos;
					obPos.Set(0,0,0);

				}

				if(messageAddress == "/ori"){
					float valueY = float.Parse (item.Value.packets[lastPacketIndex].Data[0].ToString());
					float valueX = -1*float.Parse (item.Value.packets[lastPacketIndex].Data[1].ToString());
					float valueZ = -1*float.Parse (item.Value.packets[lastPacketIndex].Data[2].ToString());
					UnityEngine.Debug.Log("ori event: "+valueX+","+valueY);
					obPos.Set(valueX,valueY,valueZ);
					transform.eulerAngles = obPos;
					obPos.Set(0,0,0);
				}

				//Log all OSC data received:
				/*UnityEngine.Debug.Log(String.Format("SERVER: {0} ADDRESS: {1} VALUE 0: {2}", 
				                                    item.Key, // Server name
				                                    item.Value.packets[lastPacketIndex].Address, // OSC address
				                                    item.Value.packets[lastPacketIndex].Data[0].ToString())); //First data value
				                                    */


			}
	    }
	}
}