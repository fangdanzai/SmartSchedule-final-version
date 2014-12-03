using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace SmartSchedule
{
	public class NewEventView : BasedView
	{
		public delegate void dlgAddNewEvent(Event ev);
		public dlgAddNewEvent OnEventAdded = null;

		private DateTime m_date = DateTime.Now;
	
		private static float WIDTH = 200;

		private UITextField m_txtTitle = null;
		private UITextField m_txtLoc = null;
		private UITextField m_txtStart = null;
		private UITextField m_txtEnd = null;
		private ScheduleBizLogic m_biz = null;
		private UITextField m_txtWeek = null;
		private UIScrollView m_scrollView = null;

		public NewEventView (RectangleF frame):base(frame)
		{
			this.Frame = frame;
			m_biz = ScheduleBizLogic.ShareInstance ();
			InitEventView ();

		}

		public NewEventView(RectangleF frame,DateTime dt):base(frame)
		{
			m_date = dt;
			this.Frame = frame;
			InitEventView ();

		}

		private void KeyboardWillShowNotification(NSNotification notification)
		{
			m_scrollView.ContentSize = new SizeF (m_scrollView.ContentSize.Width, 100 + m_scrollView.ContentSize.Height);
		}

		private void InitEventView()
		{

			//HeaderView
			UIView headerView = createHeaderView ();
			headerView.Frame = new RectangleF (0, 10, WIDTH, headerView.Frame.Height);
			//BodyView
			//UIView bodyView = createBodyView ();
			//bodyView.Frame = new RectangleF (0, headerView.Frame.Y + headerView.Frame.Height + 30, bodyView.Frame.Width, bodyView.Frame.Height);
			//ActionView
			UIView actionView = createActionView ();
			actionView.Frame = new RectangleF ((headerView.Frame.Width - actionView.Frame.Width) / 2, headerView.Frame.Y + headerView.Frame.Height + 10,actionView.Frame.Width, actionView.Frame.Height);


			m_scrollView = new UIScrollView (new RectangleF(0, 0, WIDTH, actionView.Frame.Y + actionView.Frame.Height));

			m_scrollView.Add (headerView);
			m_scrollView.Add (actionView);
			m_scrollView.ShowsVerticalScrollIndicator = false;
			m_scrollView.ShowsHorizontalScrollIndicator = false;
			m_scrollView.ContentSize = new SizeF (m_scrollView.Frame.Width, m_scrollView.Frame.Height);

			this.Add (m_scrollView);
			//this.Add (headerView);
			//this.Add (bodyView);
			//this.Add (actionView);

			this.BackgroundColor = UIColor.LightGray;

			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification, KeyboardWillShowNotification);

		}


		private UIView createHeaderView()
		{
			float fHeight = 30f;
			float yPos = 0f;

			m_txtTitle = new UITextField (new RectangleF(0, yPos, WIDTH, fHeight));
			m_txtTitle.Placeholder = "Title";
			yPos += m_txtTitle.Frame.Y + m_txtTitle.Frame.Height;

			UIView viewHLine = DrawHLine (new RectangleF (0, yPos, WIDTH, 1),UIColor.Gray);
			yPos += viewHLine.Frame.Height;

			m_txtLoc = new UITextField (new RectangleF(0, fHeight, this.Frame.Width, fHeight));
			m_txtLoc.Placeholder = "Location";
			yPos += m_txtLoc.Frame.Height;

			UIView viewHLine1 = DrawHLine (new RectangleF (0, yPos, WIDTH, 1),UIColor.Gray);
			yPos += viewHLine1.Frame.Height;

			m_txtStart = new UITextField (new RectangleF(0, yPos, WIDTH, fHeight));
			m_txtStart.Placeholder = "Start";
			yPos += m_txtStart.Frame.Height;

			UIView viewHLine2 = DrawHLine (new RectangleF (0, yPos, WIDTH, 1),UIColor.Gray);
			yPos += viewHLine2.Frame.Height;

			m_txtEnd = new UITextField (new RectangleF(0, yPos, WIDTH, fHeight));
			m_txtEnd.Placeholder = "End";
			yPos += m_txtEnd.Frame.Height;

			UIView viewHLine3 = DrawHLine (new RectangleF (0, yPos, WIDTH, 1),UIColor.Gray);
			yPos += viewHLine3.Frame.Height;

			m_txtWeek = new UITextField (new RectangleF(0, yPos, WIDTH, fHeight));
			m_txtWeek.Placeholder = "Week";
			yPos += m_txtWeek.Frame.Height;


			UIView headerView = new UIView (new RectangleF (0, 0, this.Frame.Width, yPos));

			headerView.Add (m_txtTitle);
			headerView.Add (viewHLine);
			headerView.Add (m_txtLoc);
			headerView.Add (viewHLine1);
			headerView.Add (m_txtStart);
			headerView.Add (viewHLine2);
			headerView.Add (m_txtEnd);
			headerView.Add (viewHLine3);
			headerView.Add (m_txtWeek);


			headerView.BackgroundColor = UIColor.White;

			return headerView;
		}

		private UIView createBodyView()
		{
			float fHeight = 0f;

			string strCurTime = ">";
			/*if(null != m_date)
				strCurTime = m_date.ToString() + " >";
			*/
			UIView startView = new BtnCell ("Start",strCurTime);
			fHeight += startView.Frame.Height;

			UIView hLineView = DrawHLine (new RectangleF (0, fHeight, WIDTH, 1),UIColor.Gray);
			fHeight += hLineView.Frame.Height;

			UIView endView = new BtnCell ("End", ">");
			endView.Frame = new RectangleF (0, fHeight, WIDTH, endView.Frame.Height);
			fHeight += endView.Frame.Height;

			UIView bodyView = new UIView (new RectangleF (0, 0, WIDTH, fHeight));

			bodyView.Add (startView);
			bodyView.Add (hLineView);
			bodyView.Add (endView);

			bodyView.BackgroundColor = UIColor.White;

			return bodyView;
		}

		private UIView createActionView()
		{
			UIButton btnAddEvent = UIButton.FromType (UIButtonType.RoundedRect);
			btnAddEvent.SetTitle ("Add new event", UIControlState.Normal);
			btnAddEvent.Frame = new RectangleF (0, 0, 140, 30);

			btnAddEvent.TouchUpInside += (sender, e) => 
			{
				AddNewEvent();
			};

			return btnAddEvent;
		}

		private void AddNewEvent()
		{
			//Event ev = new Event(
			DateTime dtStart = Convert.ToDateTime (m_txtStart.Text);
			DateTime dtEnd = Convert.ToDateTime (m_txtEnd.Text);
			Event ev = new Event (dtStart, dtEnd, m_txtTitle.Text, m_txtLoc.Text);
			ev.Week = Convert.ToInt32 (m_txtWeek.Text);
			m_biz.AddEvent (ev);
		
			this.Controller.DismissViewController (true, null);
		}


		private UIView DrawHLine(RectangleF frame,UIColor color)
		{
			UIView hLineView = new UIView (frame);
			hLineView.BackgroundColor = color;

			return hLineView;
		}

		private void HandleTap(UITapGestureRecognizer recognizer)
		{
			m_txtTitle.BecomeFirstResponder ();
		}
	}

	public class BtnCell : UIView
	{
		private static float HEIGHT = 30f;

		private UILabel m_lblLeft = null;
		private UIButton m_btnRight = null;
		private Object m_objTag = null;
				

		public string LeftText
		{
			get{ return m_lblLeft.Text; }
			set{ m_lblLeft.Text = value; }
		}

		public string RightText
		{
			get{ return m_btnRight.Title(UIControlState.Normal); }
			set{ m_btnRight.SetTitle(value,UIControlState.Normal); }
		}

		public Object ObjTag
		{
			get{ return m_objTag; }
			set{ m_objTag = value; }
		}

		public BtnCell(string strLeft,string strRight)
		{
			InitCell (strLeft, strRight);
		}

		private void InitCell(string strLeft,string strRight)
		{
			m_lblLeft = new UILabel (new RectangleF(0, 0, 50, HEIGHT));
			LeftText = strLeft;
			m_lblLeft.TextAlignment = UITextAlignment.Left;
			m_lblLeft.BackgroundColor = UIColor.Red;

			m_btnRight = UIButton.FromType (UIButtonType.RoundedRect);
			RightText = strRight;

			m_btnRight.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			m_btnRight.VerticalAlignment = UIControlContentVerticalAlignment.Center;

			m_btnRight.Frame = new RectangleF (150, 0, 20, HEIGHT);
			m_btnRight.TouchUpInside += (sender, e) => 
			{
				return;
			};

			this.Add (m_lblLeft);
			this.Add (m_btnRight);

			this.Frame = new RectangleF (0, 0, 100, HEIGHT);
		}


	}
}

