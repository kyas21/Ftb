using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTimePickerEx
{
    class DateTimePickerEx:System.Windows.Forms.DateTimePicker
    {
        public DateTimePickerEx()
        {
            ValueChanged += DateTimePickerEx_ValueChanged;
            Value = DateTime.Now;
            CustomFormat = "yyyy年MM月";
            Format = System.Windows.Forms.DateTimePickerFormat.Custom;
        }

        private void DateTimePickerEx_ValueChanged(object sender, EventArgs e)
        {
            Value = Value.AddDays(Value.Day * -1 + 1);
        }
    }
}
