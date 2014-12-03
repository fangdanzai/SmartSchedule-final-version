using System;
using System.Collections;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace SmartSchedule
{
	public class ScheduleBizLogic
	{
		private static ScheduleBizLogic g_instance = null;

		public delegate void dlgResponseLoadScheduleEvents(IList<Event> events);
		public event dlgResponseLoadScheduleEvents ResponseLoadScheduleEvents = null;


		private List<Event> m_events = new List<Event>();

		protected ScheduleBizLogic ()
		{
		}

		public static ScheduleBizLogic ShareInstance()
		{
			if (null == g_instance) 
			{
				g_instance = new ScheduleBizLogic ();
			}

			return g_instance;
		}

		public void AddEvent(Event ev)
		{
			m_events.Add (ev);
		}


		public void RequestLoadScheduleEvents(DateTime dt)
		{
			List<Event> lstEvents = new List<Event> ();

			DateTime dtStart = dt.AddDays (0 - Convert.ToInt32 (dt.DayOfWeek.ToString ("d")));
			DateTime dtEnd = dt.AddDays (7 - Convert.ToInt32 (dt.DayOfWeek.ToString ("d")));

			foreach (Event ev in m_events)
			{
				if (ev.Start.Day >= dtStart.Day && ev.Start.Day <= dtEnd.Day)
					lstEvents.Add (ev);
			}

			if (null != ResponseLoadScheduleEvents)
				ResponseLoadScheduleEvents (lstEvents);

		}

	}
}

