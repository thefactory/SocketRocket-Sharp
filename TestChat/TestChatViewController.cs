using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using SocketRocket;

namespace TestChat
{
	public partial class TestChatViewController : UITableViewController
	{
		SRWebSocket socket;
		List<ChatMessage> messages = new List<ChatMessage>();

		public TestChatViewController () : base ("TestChatViewController", null)
		{
		}

		public TestChatViewController(IntPtr handle) : base(handle) {}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.

			socket = new SRWebSocket(new NSUrl("ws://localhost:9000/chat"));
			socket.MessageReceived += HandleMessageReceived;
			socket.Open();

			TableView.DataSource = new TestChatDataSource(messages);

			inputView.BecomeFirstResponder();

			inputView.Changed += HandleInputChanged;
		}

		void HandleInputChanged (object sender, EventArgs e)
		{
			if (inputView.Text.EndsWith("\n")) {
				var msg = inputView.Text.TrimEnd ();
				messages.Add (new ChatMessage { Message = msg, Mine = true });
				TableView.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(messages.Count - 1, 0) }, UITableViewRowAnimation.None);
				socket.Send (new NSString(msg));
				inputView.Text = "";
			}
		}

		void HandleMessageReceived (object sender, SRMessageReceivedEventArgs e)
		{
			messages.Add(new ChatMessage { Message = e.Message.ToString(), Mine = false });
			TableView.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(messages.Count - 1, 0) }, UITableViewRowAnimation.None);
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}

	class ChatMessage
	{
		public string Message { get; set; }
		public bool Mine { get; set; }
	}

	class TestChatDataSource : UITableViewDataSource
	{
		private List<ChatMessage> messages;

		public TestChatDataSource(List<ChatMessage> msgs)
		{
			messages = msgs;
		}

		#region implemented abstract members of UITableViewDataSource

		public override int RowsInSection (UITableView tableView, int section)
		{
			return messages.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell("CELL");

			if (cell == null) {
				cell = new UITableViewCell(UITableViewCellStyle.Default, "CELL");
			}

			cell.TextLabel.Text = messages[indexPath.Row].Message;
			cell.TextLabel.BackgroundColor = UIColor.Clear;
			cell.ContentView.BackgroundColor = messages[indexPath.Row].Mine ? UIColor.Purple : UIColor.Green;

			return cell;
		}

		#endregion
	}
}

