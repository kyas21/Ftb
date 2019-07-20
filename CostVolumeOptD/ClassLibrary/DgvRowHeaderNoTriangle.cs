using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class DgvRowHeaderNoTriangle : DataGridViewRowHeaderCell
    {
        protected override void Paint(
                                    Graphics graphics,
                                    Rectangle clipBounds,
                                    Rectangle cellBounds,
                                    int rowIndex,
                                    DataGridViewElementStates cellState,
                                    Object value,
                                    Object formattedValue,
                                    string errorText,
                                    DataGridViewCellStyle cellStyle,
                                    DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                    DataGridViewPaintParts paintParts
                                        )
        {
            paintParts &= ~DataGridViewPaintParts.ContentBackground;
            base.Paint( graphics, clipBounds, cellBounds, rowIndex, cellState, value,
                        formattedValue, errorText, cellStyle, advancedBorderStyle,
                        paintParts );
        }
    }
}
