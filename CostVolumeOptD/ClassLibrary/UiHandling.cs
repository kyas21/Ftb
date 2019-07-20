using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class UiHandling
    {
        //--------------------------------------//
        //Field
        //--------------------------------------//
        DataGridView dgv;
        //--------------------------------------//
        //Constructor
        //--------------------------------------//
        public UiHandling()
        {

        }

        public UiHandling( DataGridView dgv )
        {
            this.dgv = dgv;
        }

        //--------------------------------------//
        //Property
        //--------------------------------------//

        //--------------------------------------//
        // Method
        //--------------------------------------//

        public void DgvReadyNoRHeader()
        {
            DgvReadyNoRHeader( Color.LightBlue );
        }


        public void DgvReadyNoRHeader( Color color )
        {
            //***** DataGridView設定 *****
            dgv.RowHeadersVisible = false;

            //dataGridViewPerson.AllowUserToAddRows = false;    // ユーザ操作による行追加を無効(禁止)
            //dgv.AllowUserToResizeColumns = false;     // ユーザによる行、列のサイズ変更を禁止する
            dgv.AllowUserToResizeRows = false;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = color;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.MultiSelect = false;    // 複数行選択、Ctrl + A　による全選択を禁止する。
        }


        public void DgvReadyNoRCHeader( Color color )
        {
            DgvReadyNoRHeader( Color.Transparent );
            dgv.ColumnHeadersVisible = false;
        }


        //並び替えができないようにする
        public void DgvNotSortable( DataGridView dgv )
        {
            foreach( DataGridViewColumn c in dgv.Columns )
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public void DgvNotSortable()
        {
            foreach( DataGridViewColumn c in dgv.Columns )
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        // DataGridViewのすべてのカラム幅を指定されたサイズに変更する
        public void DgvColumnsWidth( int width )
        {
            for( int i = 0; i < dgv.ColumnCount; i++ ) dgv.Columns[i].Width = width;
        }

        // DataGridViewの指定カラム幅を指定されたサイズに変更する
        public void DgvColumnsWidth( int point, int width )
        {
            dgv.Columns[point].Width = width;
        }


        public void DgvColumnsAlignmentRight( int frCol, int toCol )
        {
            for( int i = 0; i < dgv.ColumnCount; i++ )
            {
                if( frCol < 0 ) frCol = 0;
                if( toCol > dgv.ColumnCount ) toCol = dgv.ColumnCount;
                if( i >= frCol && i <= toCol )
                    dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
        // DataGridViewのすべてのカラム高を指定されたサイズに変更する
        public void DgvRowsHeight( int height )
        {
            //for (int i = 0; i < dgv.RowCount; i++) dgv.Rows[i].Height = height;
            dgv.ColumnHeadersHeight = height;
            dgv.RowTemplate.Height = height;
        }

        // DataGridViewの指定カラム幅を指定されたサイズに変更する
        public void DgvRowsHeight( int point, int height )
        {
            dgv.Rows[point].Height = height;
        }

        public void DgvRowsReadOnly( int[] rowsArray )
        {
            for( int i = 0; i < rowsArray.Length; i++ ) dgv.Rows[rowsArray[i]].ReadOnly = true;
        }

        public void DgvRowsReadOnly( int[] rowsArray, Color color )
        {
            for( int i = 0; i < rowsArray.Length; i++ ) dgv.Rows[rowsArray[i]].ReadOnly = true;
            DgvRowsColor( rowsArray, color );
        }

        public void DgvRowsColor( int[] rowsArray, Color color )
        {
            for( int i = 0; i < rowsArray.Length; i++ ) dgv.Rows[rowsArray[i]].DefaultCellStyle.BackColor = color;
        }

        public void NoSortable()
        {
            foreach( DataGridViewColumn c in dgv.Columns )
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
        }


        public void DgvColumnsWrapModeON()
        {
            // セル内容に合わせ高さ自動調整
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // 列のセルのテキストを折り返して表示する
            for( int i = 0; i < dgv.ColumnCount; i++ ) dgv.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        // Wakamatsu 20170322
        /// <summary>
        /// 対象列の表示形式を"テキストを折り返して表示"、テキスト表示位置を設定する
        /// </summary>
        /// <param name="point">対象列</param>
        /// <param name="SetAlignment">設定表示位置</param>
        public void DgvColumnsWrapModeON( int point, DataGridViewContentAlignment SetAlignment )
        {
            // 列のセルのテキストを折り返して表示する
            dgv.Rows[point].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.Rows[point].DefaultCellStyle.Alignment = SetAlignment;
        }
        // Wakamatsu 20170322

        /// <summary>
        /// 指定された列位置(startPoint)から指定した列名（nameTextArray）を順次設定する
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="nameTextArray"></param>
        public void DgvColumnName( int startPoint, string[] nameTextArray )
        {
            for( int i = 0; i < nameTextArray.Length; i++ )
            {
                dgv.Columns[startPoint + i].Name = nameTextArray[i];
            }

        }

        /// <summary>
        /// 指定された列位置(startPoint)から指定した列タイトル（headerTextArray）を順次設定する
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="headerTextArray"></param>
        public void DgvColumnHeader( int startPoint, string[] headerTextArray )
        {
            for( int i = 0; i < headerTextArray.Length; i++ )
            {
                dgv.Columns[startPoint + i].HeaderText = headerTextArray[i];
            }

        }

        public void DgvColumnsReadOnly( int[] cellsArray )
        {
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                for( int j = 0; j < cellsArray.Length; j++ ) dgv.Rows[i].Cells[cellsArray[j]].ReadOnly = true;
            }
        }

        public void DgvColumnsReadOnly( int[] cellsArray, Color color )
        {
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                for( int j = 0; j < cellsArray.Length; j++ ) dgv.Rows[i].Cells[cellsArray[j]].ReadOnly = true;
            }

            DgvColumnsColor( cellsArray, color );
        }

        public void DgvColumnsColor( int[] cellsArray, Color color )
        {
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                for( int j = 0; j < cellsArray.Length; j++ ) dgv[j, i].Style.BackColor = color;
            }
        }

        public void DgvReadOnlyColorClear()
        {
            for( int i = 0; i < dgv.ColumnCount; i++ )
            {
                for( int j = 0; j < dgv.Rows.Count; j++ )
                {
                    dgv.Rows[j].Cells[i].ReadOnly = false;
                    dgv.Rows[j].Cells[i].Style.BackColor = Color.White;
                }
            }
        }

        public void DgvRowsBackColorSet( int[] rowsArray, int[] cellsArray, Color color )
        {
            for( int i = 0; i < cellsArray.Length; i++ )
            {
                for( int j = 0; j < rowsArray.Length; j++ ) dgv.Rows[rowsArray[j]].Cells[cellsArray[i]].Style.BackColor = color;
            }
        }

        /// <summary>
        /// ユーザによる、DataGirdViewの行と列のサイズ変更を禁止する
        /// </summary>
        /// <param name="dgv"></param>
        public void DgvNotResize()
        {
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
        }


        public static void FormSizeSTD( Form form )
        {
            FormSize( form, 1280, 720 );
        }


        public static void FormSize(Form form, int sizeW,int sizeH )
        {
            form.Width = sizeW;
            form.Height = sizeH;
        }

        public static void FormPosition(Form form)
        {
            FormPosition( form, 0, 0 );
        }

        public static void FormPosition(Form form,int posX,int posY )
        {
            form.StartPosition = FormStartPosition.Manual;
            form.DesktopLocation = new Point( posX, posY );
        }
    }
}
