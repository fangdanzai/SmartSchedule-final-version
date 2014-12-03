using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SmartSchedule
{
	public class CalendarView : BasedView
	{
		private static float WIDTH_UNIT = 40f;
		private static float HEIGHT_UNIT = 40f;

		private static float WIDTH_VIEW = UIScreen.MainScreen.Bounds.Width;
		private static float HEIGHT_HEADER_VIEW = 50f;

		private SSDate m_nCurDate;

		public delegate void dlgSelectDate(SSDate date);
		public dlgSelectDate OnSelectDate = null;


		public SSDate CurrentDate
		{
			get{ return m_nCurDate; }
			private set{ m_nCurDate = value; }
		}



		public CalendarView (RectangleF frame):base(frame)
		{
			CurrentDate = new SSDate ();
			InitCalendarView (m_nCurDate);
		}

		private void InitCalendarView(SSDate date)
		{
			//Header
			UIView headerView = createHeaderView (date);

			//Boday
			UIView bodyView = createBodyView (date);
			bodyView.Frame = new RectangleF ((headerView.Frame.Width - bodyView.Frame.Width) / 2, headerView.Frame.Height, bodyView.Frame.Width, bodyView.Frame.Height);


			this.Frame = new RectangleF (this.Frame.Location, new SizeF(WIDTH_VIEW,headerView.Frame.Height + bodyView.Frame.Height));

			this.Add (headerView);
			this.Add (bodyView);


			this.BackgroundColor = UIColor.White;
		}

		private UIView createHeaderView(SSDate date)
		{
			UIView headerView = new UIView (new RectangleF (0, 0, WIDTH_VIEW, HEIGHT_HEADER_VIEW));

			UIButton btnPre = UIButton.FromType (UIButtonType.RoundedRect);
			btnPre.SetTitle ("<",UIControlState.Normal);
			btnPre.Frame = new RectangleF (10, 5, 40, 40);
			btnPre.TouchUpInside += (sender, e) => 
			{
				ShowPreMonth();
			};


			UIButton btnNext = UIButton.FromType (UIButtonType.RoundedRect);
			btnNext.SetTitle (">", UIControlState.Normal);
			btnNext.Frame = new RectangleF (WIDTH_VIEW - 50, 5, 40, 40);
			btnNext.TouchUpInside += (sender, e) => 
			{
				ShowNextMonth();
			};

			UILabel lblTitle = new UILabel ();
			lblTitle.Text = date.Year.ToString () + "-" + date.Month.ToString ();
			lblTitle.TextAlignment = UITextAlignment.Center;
			lblTitle.Frame = new RectangleF (btnPre.Frame.Width + btnPre.Frame.X + 5, 5, 210, 50);

			headerView.Add (btnPre);
			headerView.Add (btnNext);
			headerView.Add (lblTitle);

			headerView.BackgroundColor = UIColor.White;
			return headerView;
		}

		public UIView createBodyView(SSDate date)
		{
			float nWidth = (date.WeeksInMonth + 1) * WIDTH_UNIT;
			float nHeight = (date.WeeksInMonth + 1) * HEIGHT_UNIT;
			int nRow = 0;

			UIView bodyView = new UIView(new RectangleF(0,0, nWidth,nHeight));
			//Week symble
			string[] shortWeekSymble = date.ShortWeekOfSymbles;
			for (int ix = 0; ix < shortWeekSymble.Length; ++ix) 
			{
				UILabel lblSymble = new UILabel (new RectangleF (ix * WIDTH_UNIT, 0, WIDTH_UNIT, HEIGHT_UNIT));
				lblSymble.Text = shortWeekSymble [ix];
				lblSymble.Font = UIFont.FromName (lblSymble.Font.Name,14);
				bodyView.Add (lblSymble);
			}
			//Days
			int nDay = 1;
			int nDays = date.Days;
			int nWeekday = date.FirstDayOfWeek;

			nRow = shortWeekSymble.Length % 7 + shortWeekSymble.Length / 7;

			for (int ix = nWeekday - 1; ix < nWeekday + nDays - 1; ++ix) 
			{
				if (ix % 7 == 0)
					++nRow;
				UIButton btn = UIButton.FromType (UIButtonType.RoundedRect);
				btn.Tag = nDay;
				btn.SetTitle (nDay.ToString(),UIControlState.Normal);
				btn.Frame = new RectangleF (ix % 7 * WIDTH_UNIT, nRow * HEIGHT_UNIT, WIDTH_UNIT, HEIGHT_UNIT);
				bodyView.Add (btn);

				btn.TouchUpInside += (sender, e) => 
				{
					CurrentDate = new SSDate(CurrentDate.Year,CurrentDate.Month,btn.Tag,0,0);
					if(null != OnSelectDate)
						OnSelectDate(CurrentDate);
				};

				++nDay;
			}

			bodyView.BackgroundColor = UIColor.White;

			return bodyView;
		}

		#region Handler

		private void ShowPreMonth()
		{

		}

		private void ShowNextMonth()
		{

		}

		#endregion 

	}


	public class SSDate
	{
		private int m_nYear;
		private int m_nMonth;
		private int m_nDay;
		private int m_nHour;
		private int m_nMinute;
		private int m_nSecond;
		private int m_nWeekday;
		private int m_nWeeksInMonth;
		private int m_nFirstDayOfWeek;
		private int m_nDays;

		private string[] m_aryShortWeekSymbles = null;

		private NSDate m_date;

		public int Year
		{
			get{ return m_nYear; }
			private set{ m_nYear = value; }
		}

		public int Month
		{
			get{ return m_nMonth; }
			private set{ m_nMonth = value; }
		}

		public int Day
		{
			get{ return m_nDay; }
			private set{ m_nDay = value; }
		}

		public int Hour
		{
			get{ return m_nHour; }
			private set{ m_nHour = value; }
		}

		public int Minute
		{
			get{ return m_nMinute; }
			private set{ m_nMinute = value; }
		}

		public int Second
		{
			get{ return m_nSecond; }
			private set{ m_nSecond = value; }
		}

		public int Weekday
		{
			get{ return m_nWeekday; }
			private set{ m_nWeekday = value; }
		}

		public int WeeksInMonth
		{
			get{ return m_nWeeksInMonth; }
			private set{ m_nWeeksInMonth = value; }
		}

		public int Days
		{
			get{ return m_nDays; }
			private set{ m_nDays = value; }
		}

		public int FirstDayOfWeek
		{
			get{ return m_nFirstDayOfWeek; }
			private set{ m_nFirstDayOfWeek = value; }
		}

		public string[] ShortWeekOfSymbles
		{
			get{ return m_aryShortWeekSymbles; }
			private set{ m_aryShortWeekSymbles = value; }
		}

		public SSDate()
		{
			m_date = NSDate.Now;
			InitDate (m_date);
		}

		public SSDate(NSDate date)
		{
			m_date = date;
			InitDate (m_date);
		}

		public SSDate(int nYear,int nMonth,int nDay,int nHour,int nMinute)
		{
			NSCalendar calendar = NSCalendar.CurrentCalendar;

			NSDateComponents comps = new NSDateComponents ();
			comps.Year = nYear;
			comps.Month = nMonth;
			comps.Day = nDay;
			comps.Hour = nHour;
			comps.Minute = nMinute;
			comps.Second = 0;

			m_date = calendar.DateFromComponents (comps);

			InitDate (m_date);
		}

		public SSDate(DateTime dt)
		{
			NSCalendar calendar = NSCalendar.CurrentCalendar;

			NSDateComponents comps = new NSDateComponents ();
			comps.Year = dt.Year;
			comps.Month = dt.Month;
			comps.Day = dt.Day;
			comps.Hour = dt.Hour;
			comps.Minute = dt.Minute;
			comps.Second = dt.Second;

			m_date = calendar.DateFromComponents (comps);

			InitDate (m_date);
		}

		public override string ToString()
		{
			return Year.ToString () + "/" + Month.ToString ("00") + "/" + Day.ToString ("00") + " " + Hour.ToString ("00") + ":" + Minute.ToString ("00");
		}

		private void InitDate(NSDate date)
		{
			NSCalendar calendar = NSCalendar.CurrentCalendar;

			NSDateComponents comps = calendar.Components( 
				NSCalendarUnit.Year 	|
				NSCalendarUnit.Month	|
				NSCalendarUnit.Day		|
				NSCalendarUnit.Hour 	|
				NSCalendarUnit.Minute	|
				NSCalendarUnit.Second	|
				NSCalendarUnit.Weekday	|								
				NSCalendarUnit.WeekOfMonth, date);

			Year = comps.Year;
			Month = comps.Month;
			Day = comps.Day;
			Hour = comps.Hour;
			Minute = comps.Minute;
			Second = comps.Second;
			Weekday = comps.Weekday;

			ShortWeekOfSymbles = calendar.ShortWeekdaySymbols;
			Days = GetDaysOfMonth (Year, Month);
			FirstDayOfWeek = GetFirstDayInMonth (Year, Month);
			WeeksInMonth = GetWeeksInMonth (Year, Month);
		}

		private int GetDaysOfMonth(int nYear,int nMonth)
		{
			int nDays = 31;

			if (2 == nMonth) {
				if ((nYear % 4 == 0 && nYear % 400 != 0) || nYear % 400 == 0)
					nDays = 29;
				else
					nDays = 28;
			} else {
				switch (nMonth) {
				case 1:
				case 3:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
					nDays = 31;
					break;
				default:
					nDays = 30;
					break;
				}
			}

			return nDays;
		}

		private int GetFirstDayInMonth(int nYear,int nMonth)
		{
			NSCalendar c = NSCalendar.CurrentCalendar;

			NSDateComponents date = new NSDateComponents ();
			date.Year = nYear;
			date.Month = nMonth;
			date.Day = 1;
			date.TimeZone = NSTimeZone.LocalTimeZone;
			NSDate tmpDate= c.DateFromComponents (date);

			NSDateComponents comp = c.Components (NSCalendarUnit.Weekday, tmpDate);

			return comp.Weekday;

		}

		private int GetWeeksInMonth(int nYear,int nMonth)
		{
			NSCalendar c = NSCalendar.CurrentCalendar;

			NSDateComponents date = new NSDateComponents ();
			date.Year = Year;
			date.Month = Month;
			date.Day = GetDaysOfMonth (Year, Month);
			date.TimeZone = NSTimeZone.LocalTimeZone;
			NSDate tmpDate= c.DateFromComponents (date);

			NSDateComponents comp = c.Components (NSCalendarUnit.WeekOfMonth, tmpDate);

			return comp.WeekOfMonth;
		}

	}
}

