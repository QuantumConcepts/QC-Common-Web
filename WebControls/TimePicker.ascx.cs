using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuantumConcepts.Common.Web.WebControls
{
    public partial class TimePicker : System.Web.UI.UserControl
    {
        public TimeSpan Time { get { return GetTime(); } set { SetTime(value); } }

        private void SetTime(TimeSpan time)
        {
            int hour = (time.Hours > 12 ? time.Hours - 12 : time.Hours);

            if (hour == 0)
                hour = 12;

            hourTextBox.Text = hour.ToString();
            minuteTextBox.Text = time.Minutes.ToString().PadLeft(2, '0');
            amPMDropDownList.SelectedValue = (time.Hours >= 12 ? "PM" : "AM");
        }

        private TimeSpan GetTime()
        {
            bool isPM = "PM".Equals(amPMDropDownList.SelectedValue);
            int hour=int.Parse(hourTextBox.Text);

            if (isPM)
                hour += 12;

            if (hour == 0)
                hour = 12;

            return new TimeSpan(hour, int.Parse(minuteTextBox.Text), 0);
        }
    }
}