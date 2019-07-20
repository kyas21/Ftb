using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Export
{
    public partial class FormExportCostMaster :Form
    {

        //--------------------------------------------------------------------//
        //      Field
        //--------------------------------------------------------------------//
        private bool iniPro = true;

        HumanProperty hp;


        //--------------------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------------------//
        public FormExportCostMaster()
        {
            InitializeComponent();
        }


        public FormExportCostMaster( HumanProperty hp )
        {
            InitializeComponent();
            this.hp = hp;
        }
        //--------------------------------------------------------------------//
        //      Property
        //--------------------------------------------------------------------//

        //--------------------------------------------------------------------//
        //      Method
        //--------------------------------------------------------------------//
        private void FormExportCostMaster_Load( object sender, EventArgs e )
        {
            textBoxMsg.Text = "";
            create_cbOffice();
        }


        private void FormExportCostMaster_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            DataTable dt;
            Button btn = ( Button )sender;
            switch( btn.Name )
            {
                case "buttonCheck":
                    dt = takeCostMasterTable();
                    if( dt == null || dt.Rows.Count == 0 )
                    {
                        textBoxMsg.AppendText( "× 処理対象となるデータがありません!\r\n" );
                    }
                    else
                    {
                        textBoxMsg.AppendText( "☆ " + Convert.ToString( dt.Rows.Count )
                            + " 件のデータが処理対象となります。\r\n 商魂への「商品マスタ」を作成するためには「開始」ボタンをクリックしてください。\r\n" );
                    }
                    break;

                case "buttonOK":
                    //string fileName = Folder.MyDocuments() + @"\作業内訳原価_" + comboBoxOffice.Text + "_" 
                    string fileName = Folder.MyDocuments() + @"\SYOU" + comboBoxOffice.Text + ".TXT";
                    dt = takeCostMasterTable();
                    GenericData gd = new GenericData( dt );
                    int procCnt = gd.CreateExportCostMaster( "Shift_JIS", fileName );
                    if( procCnt < 0 )
                    {
                        textBoxMsg.AppendText( "× 「商品マスタ」の作成に失敗しました。\r\n" );
                        return;
                    }
                    else
                    {
                        textBoxMsg.AppendText( "〇 " + Convert.ToString( procCnt )
                            + " 件のデータを商魂取込用「商品マスタ」として、\r\nファイル：" + fileName + "\r\nに出力しました。\r\n" );
                    }
                    break;

                case "buttonCancel":
                    textBoxMsg.Text = "";
                    break;

                case "buttonEnd":
                    this.Close();
                    break;

                default:
                    break;
            }
        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            ComboBox cbb = ( ComboBox )sender;
        }


        //--------------------------------------------------------------------//
        //     SubRoutine
        //--------------------------------------------------------------------//
        // コントロールをプログラミングの効率化を目的として配列化する

        // ComboBox作成
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName" );
        }


        private DataTable takeCostMasterTable()
        {
            SqlHandling sh = new SqlHandling( "M_Cost" );
            DataTable dt = sh.SelectAllData( "WHERE OfficeCode = '" + Convert.ToString( comboBoxOffice.SelectedValue ) + "'" );
            return dt;
        }

























    }
}
