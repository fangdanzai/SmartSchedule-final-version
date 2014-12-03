using System;

using MonoTouch.Foundation;


namespace SmartSchedule
{
	public class Event
	{
		private DateTime m_dtStart;
		private DateTime m_dtEnd;
		private string m_title;
		private string m_location;
		private int m_nWeek;

		public DateTime Start
		{
			get{ return m_dtStart; }
			private set{ m_dtStart = value; }
		}

		public DateTime End
		{
			get{ return m_dtEnd; }
			private set{ m_dtEnd = value; }
		}

		public string Title
		{
			get{ return m_title; }
			private set{ m_title = value; }
		}

		public string Location
		{
			get{ return m_location; }
			private set{ m_location = value; }
		}

		public int Week
		{
			get{ return m_nWeek; }
			set{ m_nWeek = value; }
		}

		public Event (DateTime dtStart,DateTime dtEnd,string strTitle,string strLoc)
		{
			Start = dtStart;
			End = dtEnd;
			Title = strTitle;
			Location = strLoc;
		}
	}
}

