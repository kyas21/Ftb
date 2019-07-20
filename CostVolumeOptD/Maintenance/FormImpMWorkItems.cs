using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using System.IO;
using System.Data.SqlClient;
using ClosedXML.Excel;

namespace Maintenance
{
    public partial class FormImpMWorkItems : Form
    {
        //---------------------------------------------------------------------
        //      Field
        //---------------------------------------------------------------------
        private bool initProc = true;       // 初期処理中true,初期処理完了false
        HumanProperty hp;
        private string fileName;
        ClosedXML.Excel.XLWorkbook oWBook = null;   // Excel Workbookオブジェクト
        const string masterName = "作業項目マスタ";
        private string bookName = masterName + ".xlsx";
        const string sheetName = "M_WorkItems";

        //---------------------------------------------------------------------
        //      Constructor
        //---------------------------------------------------------------------
        public FormImpMWorkItems()
        {
            InitializeComponent();
        }

        public FormImpMWorkItems( HumanProperty hp )
        {
            InitializeComponent();
            this.hp = hp;
        }
        //---------------------------------------------------------------------
        //      Property
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //      Method
        //---------------------------------------------------------------------
        private void FormImpMWorkItems_Load( object sender, EventArgs e )
        {
            textBoxMsg.Text = "";
            if ( hp.MemberName == "ROOT" ) hp.MemberCode = "000";
        }


        private void FormImpMWorkItems_Shown( object sender, EventArgs e )
        {
            initProc = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if ( initProc ) return;

            Button btn = ( Button )sender;

            switch ( btn.Name )
            {
                case "buttonOpen":
                    fileName = Files.Open( bookName, Folder.MyDocuments(), "xlsx" );
                    if ( fileName == null )
                    {
                        textBoxMsg.AppendText( "× " + fileName + "は不適切なファイルです。処理続行不可能です。\r\n" );
                    }
                    else
                    {
                        textBoxMsg.AppendText( "☆ " + fileName + "の内容を作業項目マスタに書き込みます。\r\n" );
                    }
                    break;
                case "buttonCancel":
                    textBoxMsg.Text = "";
                    break;
                case "buttonStart":
                    if ( fileName == null ) return;

                    MasterMaintOp mmo = new MasterMaintOp();
                    if ( !mmo.MWorkItems_Delete( hp.MemberCode ) )
                    {
                        textBoxMsg.AppendText( "× 旧データの削除に失敗しました処理を中断します。\r\n" );
                        return;
                    }
                    mmo = new MasterMaintOp();
                    int procCount = 0;
                    switch ( System.IO.Path.GetExtension( fileName ) )
                    {
                        case ".csv":
                            procCount = mmo.MaintWorkItemsByCSVData( fileName, hp.MemberCode );
                            break;
                        case ".xlsx":
                            oWBook = new XLWorkbook( fileName );
                            procCount = mmo.MaintWorkItemsByExcelData( oWBook.Worksheet( 1 ), hp.MemberCode );
                            break;
                        default:
                            procCount = -1;
                            textBoxMsg.AppendText( "× " + fileName + "は処理できないファイルです。\r\n" );
                            break;
                    }

                    if ( procCount < 0 ) return;
                    textBoxMsg.AppendText( "〇 " + fileName + "を処理しました。\r\n" );
                    textBoxMsg.AppendText( procCount + "件のデータを登録しました。\r\n" );
                    break;
                case "buttonExport":
                    textBoxMsg.AppendText( "☆ 処理を開始しました。\r\n" );
                    //string SetSQL = "ItemCode, UItem, Item, ItemDetail, Unit, StdCost, MemberCode, UpdateDate"
                    //                + " FROM M_WorkItems WHERE MemberCode = '" + hp.MemberCode + "'";
                    string SetSQL = "ItemCode, UItem, Item, ItemDetail, Unit, StdCost, MemberCode, UpdateDate"
                                    + " FROM M_WorkItems WHERE MemberCode = ";

                    SqlHandling sqlh = new SqlHandling();               // SQL実行クラス
                    // レコードを取得する
                    DataTable dt = sqlh.SelectFullDescription( SetSQL + "'" + hp.MemberCode + "'" );
                    if ( dt == null )
                    {
                        dt = sqlh.SelectFullDescription( SetSQL + "'000'" );
                        if ( dt == null )
                        {
                            textBoxMsg.AppendText( "× Excel出力ができませんでした。\r\n" );
                            return;
                        }
                        else
                        {
                            textBoxMsg.AppendText( "△ " + hp.MemberName + "様の作業項目マスタが未登録のため、共用の作業項目マスタをExcel出力します。\r\n" );
                        }

                    }

                    // フォーマット設定用構造体
                    PrintOut.Publish.FormatSet[] FormatSet = new PrintOut.Publish.FormatSet[dt.Columns.Count];
                    // フォーマット設定
                    FormatSetting( ref FormatSet );

                    // Excel出力クラス
                    PrintOut.Publish publ = new PrintOut.Publish( Folder.DefaultExcelTemplate( bookName ) );
                    // Excelファイル出力
                    textBoxMsg.AppendText(publ.ExcelFile(masterName, sheetName, dt, FormatSet));
                    break;
                case "buttonEnd":
                    this.Close();
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        /// フォーマット設定
        /// </summary>
        /// <param name="FormatSet">フォーマット設定構造体</param>
        private void FormatSetting( ref PrintOut.Publish.FormatSet[] FormatSet )
        {
            for ( int i = 0; i < FormatSet.Length; i++ )
            {
                switch ( i )
                {
                    case 0:             // ID
                        FormatSet[i].SetFormat = "@";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.Right;
                        break;
                    case 5:             // 標準単価
                        FormatSet[i].SetFormat = "#,0";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                    case 7:            // 更新日
                        FormatSet[i].SetFormat = "yyyy-mm-dd";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                    default:            // 上記以外
                        FormatSet[i].SetFormat = "";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                }




            }
        }
    }
}
