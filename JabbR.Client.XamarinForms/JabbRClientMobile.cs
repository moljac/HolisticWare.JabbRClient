using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;

using JabbR.Models;
using JabbR.Client.Models;

namespace JabbR.Client.XamarinForms
{
	public class JabbRClientMobile
	{
		static JabbRClientMobile()
		{
			JabbRClientMobile.Client = new JabbRClientMobile();

			return;
		}

		public JabbRClientMobile()
		{
			return;
		}

		public static JabbRClientMobile Client { get; set; }

		private JabbRClient ClientJabbRAPI = null;
	    private LogOnInfo InfoJabbrAPI = null;
		private User UserJabbRAPI = null;

		public async Task InitializeAsync(string server)
		{
            ClientJabbRAPI = new JabbRClient(server);

            // Uncomment to see tracing
            //client.TraceWriter = Debug.Out;

            // Subscribe to new messages
			ClientJabbRAPI.MessageReceived += ClientJabbRAPI_MessageReceived;
			ClientJabbRAPI.UserJoined += ClientJabbRAPI_UserJoined;
            ClientJabbRAPI.UserLeft += ClientJabbRAPI_UserLeft;
            ClientJabbRAPI.PrivateMessage += ClientJabbRAPI_PrivateMessage;


            //RunClientAPITestsAsync(server, roomName, username, password, ClientJabbRAPI, wh);

			return;
		}


		public async Task LoginAsync(string server, string username, string password)
		{
			try
			{
				await InitializeAsync(server);
				//await CreateAccountAsync(server, username, password);

				// Connect to chat
				InfoJabbrAPI = await ClientJabbRAPI.Connect(username, password);

				Debug.WriteLine("Logged on successfully. You are currently in the following rooms:");
				foreach (var room in InfoJabbrAPI.Rooms)
				{
					Debug.WriteLine(room.Name);
					Debug.WriteLine(room.Private);
				}

				Debug.WriteLine("User id is {0}. Don't share this!", InfoJabbrAPI.UserId);

				Debug.WriteLine("");

				// Get my user info
				UserJabbRAPI = await ClientJabbRAPI.GetUserInfo();

				Debug.WriteLine(UserJabbRAPI.Name);
				Debug.WriteLine(UserJabbRAPI.LastActivity);
				Debug.WriteLine(UserJabbRAPI.Status);
				Debug.WriteLine(UserJabbRAPI.Country);
			}
			catch (System.Exception exc)
			{
				string msg = exc.Message;
			}

			return;
		}

		public async Task JoinRoomAsync(string room)
		{
			// Join a room called test
			await ClientJabbRAPI.JoinRoom(room);

			// Send a client side message
			//var message = new ClientMessage
			//{
			//    Id = Guid.NewGuid().ToString(),
			//    Content = "Hey",
			//    Room = roomName
			//};

			// Send the message to the server and wait for the ack
			//await client.Send(message, TimeSpan.FromSeconds(2));


		}

		public async Task GetRoomInfoAsync(string room)
		{
			// Get info about the test room
			Room roomInfo = await ClientJabbRAPI.GetRoomInfo(room);

			Debug.WriteLine("Users");

			foreach (var u in roomInfo.Users)
			{
				Debug.WriteLine(u.Name);
			}

			Debug.WriteLine("");

			foreach (var u in roomInfo.Users)
			{
				if (u.Name != UserJabbRAPI.Name)
				{
					//await client.SendPrivateMessage(u.Name, "hey there, this is private right?");
				}
			}

			return;
		}

		private void ClientJabbRAPI_MessageReceived(Message message, string arg2)
		{
			Debug.WriteLine("[{0}] {1}: {2}", message.When, message.User.Name, message.Content);

			return;
		}

		private void ClientJabbRAPI_UserJoined(User user, string room, bool arg3)
		{
			Debug.WriteLine("{0} joined {1}", user.Name, room);
		}

		private void ClientJabbRAPI_UserLeft(User user, string room)
		{
			Debug.WriteLine("{0} left {1}", user.Name, room);
		}

		private void ClientJabbRAPI_PrivateMessage(string from, string to, string message)
		{
			Debug.WriteLine("*PRIVATE* {0} -> {1} ", from, message);
		}


		private async void RunClientAPITestsAsync(string server, string roomName, string userName, string password, IJabbRClient client, ManualResetEventSlim wh)
        {
       
            try
            {
                await CreateAccountAsync(server, userName, password);

                // Connect to chat
                InfoJabbrAPI = await client.Connect(userName, password);

                Debug.WriteLine("Logged on successfully. You are currently in the following rooms:");
                foreach (var room in InfoJabbrAPI.Rooms)
                {
                    Debug.WriteLine(room.Name);
                    Debug.WriteLine(room.Private);
                }

                Debug.WriteLine("User id is {0}. Don't share this!", InfoJabbrAPI.UserId);

                Debug.WriteLine("");

                // Get my user info
                UserJabbRAPI = await client.GetUserInfo();

                Debug.WriteLine(UserJabbRAPI.Name);
                Debug.WriteLine(UserJabbRAPI.LastActivity);
                Debug.WriteLine(UserJabbRAPI.Status);
                Debug.WriteLine(UserJabbRAPI.Country);

                // Join a room called test
                await client.JoinRoom(roomName);

                // Send a client side message
                //var message = new ClientMessage
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Content = "Hey",
                //    Room = roomName
                //};

                // Send the message to the server and wait for the ack
                //await client.Send(message, TimeSpan.FromSeconds(2));

                // Get info about the test room
                Room roomInfo = await client.GetRoomInfo(roomName);

                Debug.WriteLine("Users");

                foreach (var u in roomInfo.Users)
                {
                    Debug.WriteLine(u.Name);
                }

                Debug.WriteLine("");

                foreach (var u in roomInfo.Users)
                {
                    if (u.Name != userName)
                    {
                        //await client.SendPrivateMessage(u.Name, "hey there, this is private right?");
                    }
                }

                // Set the flag
                //await client.SetFlag("bb");

                // Set the user note
                //await client.SetNote("This is testing a note");

                // Mark the client as typing
                //await client.SetTyping(roomName);

                // Clear the note
                await client.SetNote(null);

                // Say hello to the room
                await client.Send("Good morning! Apologiez to all for p", roomName);

                if (roomInfo.Owners.Contains(userName))
                {
                    // Post a notification (You must be room owner)
                    //await client.PostNotification(new ClientNotification
                    //{
                    //    Source = "Github",
                    //    Content = "This is a fake github notification from the client",
                    //    ImageUrl = "data:image/png;base64,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",
                    //    Room = "test"
                    //});
                }

                //Debug.WriteLine("Press any key to leave the room and disconnect");
                //Debug.Read ();

                await client.LeaveRoom(roomName);

                client.Disconnect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.GetBaseException().Message);
            }
            finally
            {
                wh.Set();
            }
        }

        private static async Task CreateAccountAsync(string server, string userName, string password)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(server)
            };

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "username", userName },
                { "email", "foo@bar.com" },
                { "password", password },
                { "confirmPassword", password }
            });

            HttpResponseMessage response = await client.PostAsync("/account/create", content);

            response.EnsureSuccessStatusCode();
        }




	}
}

