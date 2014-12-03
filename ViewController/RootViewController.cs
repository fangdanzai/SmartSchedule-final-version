using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SmartSchedule
{
	public class RootViewController : UIViewController
	{
		private static float YPOS_SUBVIEWS = 55f;

		private CalendarView m_calenderView = null;
		private UIScrollView m_scrollView = null;



		public RootViewController (IntPtr handel) : base (handel)
		{
		}

		public RootViewController() : base()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Title = "Schedule";

			ScheduleView sv = new ScheduleView (new RectangleF (0, YPOS_SUBVIEWS, this.View.Frame.Width, this.View.Frame.Height));
			this.View.BackgroundColor = UIColor.White;
			this.View.Add (sv);
			return;


			m_calenderView = new CalendarView (new RectangleF (0,YPOS_SUBVIEWS, UIScreen.MainScreen.Bounds.Width, 300));
			m_calenderView.OnSelectDate = perfromDateSelected;

			this.View.BackgroundColor = UIColor.White;
			this.View.Add (m_calenderView);

			float xPos = 0;
			float yPos = m_calenderView.Frame.Y + m_calenderView.Frame.Height;
			float width = UIScreen.MainScreen.Bounds.Width;
			float height = 240;

			m_scrollView = new UIScrollView (new RectangleF (xPos, yPos, width, height));
			m_scrollView.PagingEnabled = false;
			m_scrollView.ShowsVerticalScrollIndicator = false;
			m_scrollView.ShowsHorizontalScrollIndicator = false;
			this.Add (m_scrollView);
		}

		public override bool ShouldAutorotate ()
		{
			return true;
		}
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.LandscapeLeft;
		}

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation ()
		{
			return UIInterfaceOrientation.LandscapeLeft;
		}

		private void perfromDateSelected(SSDate date)
		{
			UIViewController viewCtrl = new UIViewController();
			RectangleF frame = new RectangleF (this.View.Frame.X, YPOS_SUBVIEWS, this.View.Frame.Width, this.View.Frame.Height);
			//NewEventView ev = new NewEventView (frame,date);
			//ev.Controller = viewCtrl;
			//ev.OnEventAdded = EventAdded;
			//viewCtrl.Add (ev);

			this.NavigationController.PushViewController (viewCtrl,true);
		}

		private void EventAdded(Event ev)
		{
			int nItemCount = m_scrollView.Subviews.Length;

			UIView view = new EventThumbView (ev);
			view.Frame = new RectangleF (0, nItemCount * view.Frame.Height, view.Frame.Width, view.Frame.Height);

			m_scrollView.Add (view);

			m_scrollView.ContentSize = new SizeF (m_scrollView.Frame.Width, m_scrollView.Subviews.Length * view.Frame.Height);

		}
	}
}

