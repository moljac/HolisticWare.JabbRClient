using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using JabbR.Client.Models;
using JabbR.Models;

using Xamarin.Forms;

namespace JabbR.Client.XamarinForms
{
	public class App
	{	
		public static Page GetMainPage()
		{
			return new JabbRPage();
		}


	}
}
