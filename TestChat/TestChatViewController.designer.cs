// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace TestChat
{
	[Register ("TestChatViewController")]
	partial class TestChatViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextView inputView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (inputView != null) {
				inputView.Dispose ();
				inputView = null;
			}
		}
	}
}
