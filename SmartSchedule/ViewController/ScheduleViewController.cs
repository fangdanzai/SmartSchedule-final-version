using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SmartSchedule
{
	public class ScheduleViewController : UIViewController
	{
		private ScheduleView m_scheuleView = null;
		private ScheduleBizLogic m_biz = null;

		//key: weakday value:events list
		private Dictionary<int,IList<Event> > m_events = null;

		public ScheduleViewController () : base()
		{
		}

		public ScheduleViewController(IntPtr handle):base(handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			m_biz = ScheduleBizLogic.ShareInstance ();
			m_biz.ResponseLoadScheduleEvents += (events) => 
			{
				ResponseLoadScheduleEvents(events);
			}; 

			this.Title = "Schedule";

			m_scheuleView = new ScheduleView (new RectangleF (0, 50, this.View.Frame.Width, this.View.Frame.Height));
			this.View.BackgroundColor = UIColor.White;
			this.View.Add (m_scheuleView);
			AddControlPanel ();

			m_scheuleView.ShowSelectedEvents = ShowEvents;

			m_events = new Dictionary<int, IList<Event>> ();
		}

		private void AddControlPanel()
		{
			UIButton btnAdd = UIButton.FromType (UIButtonType.RoundedRect);
			btnAdd.SetTitle ("AddNew", UIControlState.Normal);
			btnAdd.Frame = new RectangleF (this.View.Frame.Bottom - 70,this.View.Frame.Right - 20,  70, 20);
			this.View.Add (btnAdd);

			btnAdd.TouchUpInside += (object sender, EventArgs e) => 
			{
				AddNewEvent();
			};
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			LoadSchedule (DateTime.Now);
		}

		private void LoadSchedule(DateTime date)
		{
			m_biz.RequestLoadScheduleEvents (date);
		}

		private void ResponseLoadScheduleEvents(IList<Event> events)
		{
			UpdateEvents (events);
			this.InvokeOnMainThread( () => {UpdateScheduleView(events);});
		}

		private void UpdateScheduleView (IList<Event> events)
		{
			m_scheuleView.ClearSubviews ();
			if (null == events)
				return;
			foreach (Event ev in events) 
			{
				int nRow = ev.Start.Hour;
				int nCol = ev.Week;
			

				int nMinSpan = ev.End.Hour * 60 + ev.End.Minute - ev.Start.Hour * 60 - ev.Start.Minute;
				float height = m_scheuleView.CellHeight * (float)nMinSpan / 60f;

				UIView evThumView = new UIView (new RectangleF (0,m_scheuleView.CellHeight * ((float)ev.Start.Minute / 60f),30,height));

				long tick = DateTime.Now.Ticks;
				Random rm = new Random ((int)(tick & 0xffffffffL) | (int) (tick >> 32));

				//R
				byte r = Convert.ToByte (rm.Next (0, 255));
				//G
				byte g = Convert.ToByte (rm.Next (0, 255));
				//B
				byte b = Convert.ToByte (rm.Next (0, 255));

				evThumView.BackgroundColor = UIColor.FromRGB (r, g, b);

				m_scheuleView.AddSubview (nCol, nRow, evThumView);
			}
		}

		private void ShowEvents(int nHour,int nWeekday)
		{
			IList<Event> events = FindEvents (nHour, nWeekday);
			if (events == null)
				return;
			EventViewContainer container = new EventViewContainer ();
			container.AddEventViews (events);
			container.OnClose += (view) => 
			{
				view.RemoveFromSuperview();
			};

			float xPos = (UIScreen.MainScreen.Bounds.Width - container.Frame.Width) / 2;
			float yPos = (UIScreen.MainScreen.Bounds.Height - container.Frame.Height) / 2 - 100;
			container.Frame = new RectangleF (xPos, yPos, container.Frame.Width, container.Frame.Height);
			//container.Frame = new RectangleF (160,100, 100, 250);
			this.View.Add (container);

		}

		private IList<Event> FindEvents(int nHour,int nWeekday)
		{
			if (!m_events.ContainsKey (nWeekday))
				return null;
			return m_events [nWeekday];
		}

		private void UpdateEvents(IList<Event> events)
		{
			m_events.Clear ();

			if (null == events)
				return;
			foreach (Event ev in events) 
			{
				List<Event> lstEvs = null;

				if (!m_events.ContainsKey(ev.Week))
				{
					lstEvs = new List<Event> ();
					m_events [ev.Week] = lstEvs;
				}
				else
					lstEvs = m_events [ev.Week] as List<Event>;

				lstEvs.Add (ev);
			}
		}

		private void AddNewEvent()
		{
			UIViewController viewControl = new UIViewController ();
			NewEventView view = new NewEventView (new RectangleF (0, 50, this.View.Frame.Width, this.View.Frame.Height));
			viewControl.View.Add (view);
			view.Controller = viewControl;
			this.PresentViewController (viewControl, true, null);
		}
	}
}

