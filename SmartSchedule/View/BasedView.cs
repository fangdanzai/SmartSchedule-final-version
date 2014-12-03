using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace SmartSchedule
{
	public class BasedView : UIView
	{
		private UIViewController m_controller;

		public UIViewController Controller
		{
			get{ return m_controller; }
			set{ m_controller = value; }
		}

		public BasedView (RectangleF frame):base(frame)
		{
		}
	}
}

