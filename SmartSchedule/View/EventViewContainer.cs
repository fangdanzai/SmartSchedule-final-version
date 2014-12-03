using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.UIKit;

namespace SmartSchedule
{
	public class EventViewContainer : UIView
	{
		public delegate void dlgCloseView (EventViewContainer view);
		public event dlgCloseView OnClose = null;


		private UIScrollView m_scrollView = null;
		private float m_yPos = 0;

		private static float WIDTH = 100f;
		private static float HEIGHT = 170f;

		public EventViewContainer ()
		{
			UIButton btnClose = UIButton.FromType (UIButtonType.RoundedRect);
			btnClose.SetTitle ("Close", UIControlState.Normal);
			btnClose.Frame = new RectangleF (70, 0, 30, 20);
			btnClose.Font = UIFont.FromName (btnClose.Font.Name, 9);
			btnClose.TouchUpInside += (sender, e) => 
			{
				if(null != OnClose)
					OnClose(this);
			};
			this.Add (btnClose);

			InitScrollView ();

			this.Frame = new RectangleF (0, 0, WIDTH, HEIGHT);
			this.BackgroundColor = UIColor.Gray;
		}

		private void InitScrollView()
		{
			m_scrollView = new UIScrollView (new RectangleF (0,20, 100, 150));
			m_scrollView.ShowsVerticalScrollIndicator = false;
			m_scrollView.ShowsHorizontalScrollIndicator = false;
			m_scrollView.PagingEnabled = false;
			this.Add (m_scrollView);
		}

		public void AddEventView(Event ev)
		{
			EventThumbView view = new EventThumbView (ev);
			view.Frame = new RectangleF (0, m_yPos, view.Frame.Width, view.Frame.Height);
			m_scrollView.Add (view);
			m_yPos += view.Frame.Height;

			m_scrollView.ContentSize = new SizeF (100, m_yPos);
		}

		public void AddEventViews(IList<Event> events)
		{
			foreach (Event ev in events) 
			{
				AddEventView (ev);
			}
		}
	}
}

