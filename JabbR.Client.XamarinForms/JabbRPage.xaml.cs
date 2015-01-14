using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace JabbR.Client.XamarinForms
{	
	public partial class JabbRPage : TabbedPage
	{	
		public JabbRPage ()
		{
			InitializeComponent ();

		}

		public async void buttonLogin_Clicked(object sender, EventArgs ea)
		{
			string server = textBoxJabbRServer.Text;
			string username = textBoxUsername.Text;
			string password = textBoxPassword.Text;
			string room = textBoxRoom.Text;

			await JabbRClientMobile.Client.LoginAsync(server, username, password);
			await JabbRClientMobile.Client.JoinRoomAsync(room);

			return;
		}
	}
}

