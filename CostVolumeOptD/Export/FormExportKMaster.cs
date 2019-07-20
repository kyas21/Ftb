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
    public partial class FormExportKMaster :Form
    {
        //--------------------------------------------------------------------//
        //      Field
        //--------------------------------------------------------------------//
        private bool iniPro = true;

        HumanProperty hp;


        //--------------------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------------------//
        public FormExportKMaster()
        {
            InitializeComponent();
        }


        public FormExportKMaster( HumanProperty hp )
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
        private void FormExportKMaster_Load( object sender, EventArgs e )
        {
            textBoxMsg.Text = "";
            create_cbOffice();
            create_cbKubun();
        }


        private void FormExportKMaster_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            switch( btn.Name )
            {
                case "buttonOK":
                    string fileName = Folder.MyDocuments() + @"\K" + comboBoxOffice.Text + ".TXT";
                    GenericData gd = new GenericData();
                    int procCnt = gd.CreateExportKMaster( "Shift_JIS", fileName, 
                                                          Convert.ToString(comboBoxOffice.SelectedValue), Convert.ToString(comboBoxKubun.SelectedValue) );
                    if( procCnt < 0 )
                    {
                        textBoxMsg.AppendText( "× 「区分マスタ」の作成に失敗しました。\r\n" );
                        return;
                    }
                    else
                    {
                        textBoxMsg.AppendText( "〇 " + Convert.ToString( procCnt )
                            + " 件のデータを商魂取込用「区分マスタ」として、\r\nファイル：" + fileName + "\r\nに出力しました。\r\n" );
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


        private void create_cbKubun()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxKubun );
            cbe.ValueItem = new string[] { "20", "31", "32", "41" };
            cbe.DisplayItem = new string[] { "営業担当者", "得意先", "部門", "商品区分" };
            cbe.Basic();
        }

    }
}
