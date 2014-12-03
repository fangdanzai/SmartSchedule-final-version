using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace SmartSchedule
{
	public class ScheduleView : UIView
	{
		public delegate void dlgOnShowSelectedEvents(int hHour,int nWeek);
		public dlgOnShowSelectedEvents ShowSelectedEvents = null;



		private const float m_fWeekHeaderItemWidth = 50f;
		private const float m_fHeaderIntervaloffsetH = 1f;


		private const float m_fCellWidth = m_fWeekHeaderItemWidth;
		private const float m_fCellIntervalOffset = m_fHeaderIntervaloffsetH;


		private const float m_fRowHeaderLabelHeight = 15f;
		private const float m_fRowHeaderWidth = 40f;
		private const float m_fRowHeight = 50f;

		private UIView m_viewWeekHeader = null;
		private UIView m_viewBody = null;

		private List<UIView> m_items = null;

		public float CellHeight
		{
			get{ return m_fRowHeight; }
		}

		public float CellWidth
		{
			get{ return m_fCellWidth; }
		}


		public ScheduleView ():base()
		{
		}

		public ScheduleView (RectangleF frame) : base (frame)
		{
			m_viewWeekHeader = createWeekHeaderView ();
			m_viewWeekHeader.Frame = new RectangleF (m_fRowHeaderWidth, 0, m_viewWeekHeader.Frame.Width, m_viewWeekHeader.Frame.Height);

			m_viewBody = createBodyView ();
			m_viewBody.Frame = new RectangleF (0, m_viewWeekHeader.Frame.Height + m_viewWeekHeader.Frame.Y, m_viewBody.Frame.Width, m_viewBody.Frame.Height);

			this.Add (m_viewWeekHeader);
			this.Add (m_viewBody);

			//this.BackgroundColor = UIColor.DarkGray;

			this.Frame = new RectangleF (frame.X, frame.Y, m_viewBody.Frame.Width, m_viewBody.Frame.Height + m_viewWeekHeader.Frame.Height);
		}

		public void AddSubview(int nCol,int nRow,UIView subView)
		{
			float xPos = m_viewWeekHeader.Frame.X + nCol * m_fWeekHeaderItemWidth + nCol * m_fHeaderIntervaloffsetH;
			float yPos = nRow * m_fRowHeight + subView.Frame.Y + nRow * m_fCellIntervalOffset;

			xPos += subView.Frame.Width / 2;


			subView.Frame = new RectangleF (xPos, yPos, subView.Frame.Width, subView.Frame.Height);
			m_viewBody.Add (subView);
			if (null == m_items)
				m_items = new List<UIView> ();
			m_items.Add (subView);
		}

		public void ClearSubviews()
		{
			if (null == m_items)
				return;
			foreach(UIView view in m_items)
			{
				view.RemoveFromSuperview ();
			}
			m_items.Clear ();
		}

		private UIView createWeekHeaderView()
		{
			UIView weekHeaderView = new UIView ();

			const int nWeekItemCoutn = 7;

			float fWeekHeaderItemHeight = 15;
			float xPos = 0;
			for(int ix = 0; ix < nWeekItemCoutn;++ix)
			{
				UILabel lbl = new UILabel(new RectangleF(xPos,0,m_fWeekHeaderItemWidth,fWeekHeaderItemHeight));
				lbl.Text = getWeekShortName(ix);
				lbl.BackgroundColor = UIColor.LightGray;
				lbl.TextColor = UIColor.White;
				lbl.TextAlignment = UITextAlignment.Center;
				lbl.Font = UIFont.FromName (lbl.Font.Name, 10);
				weekHeaderView.Add(lbl);

				xPos += m_fWeekHeaderItemWidth + m_fHeaderIntervaloffsetH;
			}

			weekHeaderView.Frame = new RectangleF (0,0, xPos, fWeekHeaderItemHeight);

			return weekHeaderView;
		}

		#region Create BodyView

		private CellView createCell(UIColor bgClr)
		{
			CellView cell = new CellView (new RectangleF (0, 0, m_fCellWidth, m_fRowHeight));
			cell.BackgroundColor = bgClr;

			cell.OnCellSelected = OnCellSelected;

			return cell;
		}

		private UIView createRowHeader(string strText,UIColor bgClr)
		{
			UILabel lbl = new UILabel (new RectangleF (0,0, m_fRowHeaderWidth - 2, m_fRowHeaderLabelHeight));
			lbl.Text = strText;
			lbl.TextAlignment = UITextAlignment.Center;
			lbl.TextColor = UIColor.White;
			lbl.Font = UIFont.FromName (lbl.Font.Name, 11);

			UIView line = new UIView (new RectangleF (m_fRowHeaderWidth - 2, 0, 2, m_fRowHeight));
			line.BackgroundColor = UIColor.Orange;

			UIView rowHeader = new UIView (new RectangleF(0,0,m_fRowHeaderWidth,m_fRowHeight));
			rowHeader.BackgroundColor = bgClr;

			rowHeader.Add (lbl);
			rowHeader.Add (line);

			return rowHeader;
		}

		private UIView createRow(int nCellCount,string rowHeaderText,UIColor bgClr,int nRow)
		{
			UIView row = new UIView ();

			UIView rowHeader = createRowHeader (rowHeaderText, bgClr);
			row.Add (rowHeader);

			float xPos = rowHeader.Frame.X + rowHeader.Frame.Width;
			float yPos = 0;
			for (int ix = 0; ix < nCellCount; ++ix)
			{
				CellView cell = createCell (bgClr);
				cell.Frame = new RectangleF (xPos, yPos, cell.Frame.Width, cell.Frame.Height);
				row.Add (cell);

				xPos += cell.Frame.Width + m_fCellIntervalOffset;

				cell.Column = ix;
				cell.Row = nRow;
			}

			row.Frame = new RectangleF (0, 0, xPos, m_fRowHeight);

			//Drawing half hour line
			//to do....


			row.BackgroundColor = UIColor.White;

			return row;
		}

		private UIView createBodyView()
		{
			UIScrollView scrollView = new UIScrollView ();
			scrollView.Frame = new RectangleF (0,0, m_viewWeekHeader.Frame.Width + m_fRowHeaderWidth, UIScreen.MainScreen.Bounds.Height - m_viewWeekHeader.Frame.Height - m_viewWeekHeader.Frame.Y);
			scrollView.ShowsHorizontalScrollIndicator = false;
			scrollView.ShowsVerticalScrollIndicator = false;

			int nHourFrom = 0;
			int nHourTo = 23;

			float xPos = 0;
			float yPos = 0;
		
			for (int ix = nHourFrom; ix <= nHourTo; ++ix) 
			{
				UIColor bg;
				string str = ix.ToString("00") + ":00";

				if (ix % 2 == 0)
					bg = UIColor.FromRGBA (176, 195, 222,255);
				else
					bg = UIColor.FromRGBA (100, 149, 237,255);
				UIView row = createRow (7, str, bg,ix);
				row.Frame = new RectangleF (xPos, yPos, row.Frame.Width, row.Frame.Height);
				yPos += row.Frame.Height + m_fCellIntervalOffset;

				scrollView.Add (row);
			}

			scrollView.ContentSize = new SizeF (scrollView.Frame.Width, yPos);

			return scrollView;
		}

		#endregion

		private string getWeekShortName(int nWeekIdx)
		{
			string strName = string.Empty;
			switch (nWeekIdx)
			{
			case 0:
				strName = "Sun";
				break;
			case 1:
				strName = "Mon";
				break;
			case 2:
				strName = "Tue";
				break;
			case 3:
				strName = "Wed";
				break;
			case 4:
				strName = "Thu";
				break;
			case 5:
				strName = "Fir";
				break;
			case 6:
				strName = "Sat";
				break;
			default:
				break;
			}
			return strName;
		}

		public void OnCellSelected(CellView cell)
		{
			if (null != ShowSelectedEvents)
				ShowSelectedEvents (cell.Row, cell.Column);
		}
	}

	public class CellView:UIView
	{

		public delegate void dlgCellSelected(CellView cell);
		public dlgCellSelected OnCellSelected = null;

		private UITapGestureRecognizer m_tapGesture = null;

		private int m_nColunm = 0;
		private int m_nRow = 0;

		public int Column 
		{
			get{ return m_nColunm; }
			set{ m_nColunm = value; }
		}

		public int Row
		{
			get{ return m_nRow; }
			set{ m_nRow = value; }
		}

		public CellView(RectangleF frame):base(frame)
		{
			m_tapGesture = new UITapGestureRecognizer (this, new MonoTouch.ObjCRuntime.Selector ("OnSelected:"));
			this.AddGestureRecognizer (m_tapGesture);
		}

		[Export("OnSelected:")]
		private void OnSelected(UIGestureRecognizer gesture)
		{
			if (null != OnCellSelected)
				OnCellSelected (this);
		}
	}
		
}

