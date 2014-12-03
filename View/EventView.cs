using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace SmartSchedule
{
	public class EventView : UIView
	{
		private const float m_fWidth = 100;
		private const float m_fHeight = 100;

		private const float m_fTitlleLabelHeight = 40;
		private const float m_fTimeLalbelHeight = 30;
		private const float m_fLocLabelHeight = 30;


		private Event m_event = null;

		public EventView (Event ev) : base()
		{
			m_event = ev;
			InitView (m_event);
		}

		private void InitView(Event ev)
		{
			//Title
			UILabel lblTitle = new UILabel (new RectangleF (0, 0, m_fWidth, m_fTimeLalbelHeight));
			lblTitle.Text = ev.Title;
			lblTitle.TextAlignment = UITextAlignment.Center;
			lblTitle.Font = UIFont.FromName (lblTitle.Font.Name, 15);
			//Time
			UILabel lblTime = new UILabel (new RectangleF (0, lblTitle.Frame.Y + lblTitle.Frame.Height, m_fWidth, m_fTimeLalbelHeight));
			lblTime.Text = ev.Start.ToString () + " ~ " + ev.End.ToString ();
			lblTime.TextAlignment = UITextAlignment.Center;
			lblTime.Font = UIFont.FromName (lblTime.Font.Name, 10);
			//Location
			UILabel lblLoc = new UILabel (new RectangleF (0, lblTime.Frame.Y + lblTime.Frame.Height, m_fWidth, m_fLocLabelHeight));
			lblLoc.Text = ev.Location;
			lblLoc.TextAlignment = UITextAlignment.Center;
			lblLoc.Font = UIFont.FromName (lblLoc.Font.Name, 10);

			this.Add (lblTitle);
			this.Add (lblTime);
			this.Add (lblLoc);

		}
	}
}

