using System;
using System.Drawing;
using MonoTouch.UIKit;


namespace SmartSchedule
{
	public class EventThumbView : UIView
	{
		private Event m_ev = null;

		public EventThumbView (Event ev)
		{
			m_ev = ev;
			InitThumbView (m_ev);
		}

		private void InitThumbView(Event ev)
		{

			UILabel lblTitle = new UILabel (new RectangleF (0, 0, 100, 39));
			lblTitle.Text = ev.Title;
			lblTitle.TextAlignment = UITextAlignment.Center;
			lblTitle.Font = UIFont.FromName (lblTitle.Font.Name, 15);

			UIView vLineView = new UIView (new RectangleF (0, 39, 100, 1));
			vLineView.BackgroundColor = UIColor.Blue;

			UILabel lblTime = new UILabel (new RectangleF (0, 40, 100, 19));
			lblTime.Font = UIFont.FromName (lblTime.Font.Name, 9);
			lblTime.TextAlignment = UITextAlignment.Center;
			string strTime = "From: " + ev.Start.Hour.ToString ("00") + ":" + ev.Start.Minute.ToString ("00");
			strTime += " To: " + ev.End.Hour.ToString ("00") + ":" + ev.End.Minute.ToString ("00");
			lblTime.Text = strTime;


			UIView vLineView1 = new UIView (new RectangleF (0, 59, 100, 1));
			vLineView1.BackgroundColor = UIColor.Blue;

			UILabel lblLoc = new UILabel (new RectangleF (0, 60, 100, 20));
			lblLoc.Text = ev.Location;
			lblLoc.Font = UIFont.FromName (lblLoc.Font.Name, 9);
			lblLoc.TextAlignment = UITextAlignment.Center;
	

			this.Add (lblTitle);
			this.Add (vLineView);
			this.Add (lblTime);
			this.Add (vLineView1);
			this.Add (lblLoc);

			this.Frame = new RectangleF (0, 0, 100, 80);

			this.BackgroundColor = UIColor.White;
		}
	}
}

