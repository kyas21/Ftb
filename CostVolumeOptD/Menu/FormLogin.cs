using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using ListForm;
using System.Reflection;

namespace Menu
{
    public partial class FormLogin : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        FormMenuA formMenuA = null;
        FormMenuB formMenuB = null;
        FormMenuAdm formMenuAdm = null;
        FormMenuS formMenuS = null;
        FormMenuInfo formMenuInfo = null;
        FormMenuDataMnt formMenuDataMnt = null;
        HumanProperty hp;
        MembersScData[] msd;
        bool iniPro = true;

        private string rootUser = "ROOT";
        private List<string> authList;

        
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormLogin()
        {
            InitializeComponent();
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//
        
        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//


        private void FormLogin_Load(object sender, EventArgs e)
        {
            Assembly mainAssembly = Assembly.GetEntryAssembly();
            this.Text = String.Format( "出来高管理システム （ ver {0} ） ", mainAssembly.GetName().Version );

            labelMessage.Text = "";    
            ListFormDataOp lo = new ListFormDataOp();
            msd = lo.SelectMembersScData();
        }


        private void FormLogin_Shown(object sender, EventArgs e)
        {
            iniPro = false;
        }


        private void button_Click(object sender, EventArgs e)
        {
            if (iniPro) return;

            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "buttonLogin":
                    if( !loginProcess() ) return;
                    hp = new HumanProperty();
                    if( !checkMembers() ) return;
                    if( !checkOffice() ) return;
                    if( !checkCommon() ) return;
                    if( !selectNextMenu() ) return;
                    //checedProcess();
                    break;
                case "buttonCancel":
                    labelMessage.Text = "";
                    textBoxMemberCode.Text = "";
                    break;
                case "buttonEnd":
                    string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
                    if (File.Exists(tempFile)) File.Delete(tempFile);
                    if( hp != null )
                    {
                        Exclusive exclusive = new Exclusive();
                        exclusive.Unregister( hp.MemberCode );
                    }
                    Application.Exit();
                    break;
                default:
                    break;
            }
        }


        private void textBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (iniPro) return;

            if( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab )
            {
                buttonLogin.PerformClick();
                //checedProcess();
                //return;
            }

            if( ( ( e.Modifiers & Keys.Control ) == Keys.Control )  && ( e.KeyCode == Keys.A ) )
            {
                string userName = Environment.UserName;
                CommonData cdp = new CommonData();
                if( cdp.ExitstenceLogin( userName ) ) chooseMemberNameData();
            }
        }

        //------------------------------------------------------------------//
        // Subroutine                                                       //
        //------------------------------------------------------------------//
        private bool checedProcess()
        {
            if( !loginProcess() ) return false;
            hp = new HumanProperty();
            if( !checkMembers() ) return false;
            if( !checkOffice() ) return false;
            if( !checkCommon() ) return false;
            if( !selectNextMenu() ) return false;
            return true;
        }


        private bool loginProcess()
        {
            labelMessage.Text = "";

            if (textBoxMemberCode.Text == null || textBoxMemberCode.Text == "")
            {
                labelMessage.Text = "社員番号が入力されていません!";
                labelMessage.ForeColor = Color.Red;
                //MessageBox.Show("ログイン名を入力してください");
                return false;
            }

            //#######################################
            // Super User
            if (textBoxMemberCode.Text == rootUser)
            {
                hp = new HumanProperty();
                if (formMenuDataMnt == null || formMenuDataMnt.IsDisposed)
                {
                    hp.MemberName = rootUser;
                    formMenuDataMnt = new FormMenuDataMnt(hp);
                    formMenuDataMnt.Show();
                }
            }
            //#######################################
            return true;
        }


        private bool checkMembers()
        {
            // 社員マスタ
            SqlHandling sh = new SqlHandling("M_Members");
            DataTable dt = sh.SelectAllData(" WHERE MemberCode = '" + textBoxMemberCode.Text + "'");
            if (dt == null)
            {
                labelMessage.Text = "該当する社員番号がありません。再入力してください!";
                labelMessage.ForeColor = Color.Red;
                return false;
            }

            DataRow dr = dt.Rows[0];
            hp.MemberType = Convert.ToInt32(dr["MemberType"]);
            hp.Enrollment = Convert.ToInt32(dr["Enrollment"]);
            if (hp.Enrollment == 0)
            {
                hp.MemberCode = Convert.ToString(dr["MemberCode"]);
                hp.MemberName = Convert.ToString(dr["Name"]);
                hp.OfficeCode = Convert.ToString(dr["OfficeCode"]);
                hp.Department = Convert.ToString(dr["Department"]);
                hp.AccessLevel = Convert.ToInt32(dr["AccessLevel"]);
            }
            else
            {
                labelMessage.Text = "操作未承認です。[取消]をクリックしてください。";
                labelMessage.ForeColor = Color.Red;
                return false;
            }
            return true;
        }


        private bool checkOffice()
        {
            // 事業所マスタ
            SqlHandling sh = new SqlHandling("M_Office");
            DataTable dt = sh.SelectAllData(" WHERE OfficeCode = '" + hp.OfficeCode + "'");
            if (dt == null)
            {
                labelMessage.Text = "事業所マスタの読み込みができません!";
                labelMessage.ForeColor = Color.Red;
                return false;
            }
            DataRow dr = dt.Rows[0];
            hp.OfficeName = Convert.ToString(dr["OfficeName"]);
            return true;
        }


        private bool checkCommon()
        {
            // 共通マスタ
            SqlHandling sh = new SqlHandling("M_Common");
            DataTable dt = sh.SelectAllData(" WHERE Kind = 'DEPT' AND ComSymbol = '" + hp.Department + "'");
            if (dt == null)
            {
                commonSelectError();
                return false;
            }
            DataRow dr = dt.Rows[0];
            hp.DepartName = Convert.ToString(dr["ComData"]);

            // 共通データを保持して画面遷移
            if (hp.OfficeCode != "H") hp.DepartName = "技術";

            string[] kindArray = new string[] { "TAX", "ADM", "OTH", "EXP" };
            for (int i = 0; i < kindArray.Length; i++)
            {
                dt = sh.SelectAllData(" WHERE Kind = '" + kindArray[i] + "'");
                if (dt == null)
                {
                    commonSelectError();
                    return false;
                }
                dr = dt.Rows[0];
                if (i == 0) hp.TaxRate = Convert.ToDecimal(dr["ComData"]) / 1000;
                if (i == 1) hp.AdminCostRate = Convert.ToDecimal(dr["ComData"]) / 1000;
                if (i == 2) hp.OthersCostRate = Convert.ToDecimal(dr["ComData"]) / 1000;
                if (i == 3) hp.Expenses = Convert.ToDecimal(dr["ComData"]) / 1000;
            }
            // 最新締め月
            dt = sh.SelectAllData(" WHERE Kind LIKE 'CLS%'");
            if (dt == null)
            {
                commonSelectError();
                return false;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                switch (Convert.ToString(dr["Kind"]))
                {
                    case "CLSH":
                        hp.CloseHDate = Convert.ToDateTime(dr["ComData"]);
                        break;
                    case "CLSK":
                        hp.CloseKDate = Convert.ToDateTime(dr["ComData"]);
                        break;
                    case "CLSS":
                        hp.CloseSDate = Convert.ToDateTime(dr["ComData"]);
                        break;
                    case "CLST":
                        hp.CloseTDate = Convert.ToDateTime(dr["ComData"]);
                        break;
                    default:
                        break;
                }
            }
            return true;
        }


        private bool selectNextMenu()
        {
            //if (hp.AccessLevel < 1)
            if (hp.AccessLevel >= 9999)
            {
                labelMessage.Text = hp.MemberName + " さんが利用します。";
                labelMessage.ForeColor = Color.Black;
                // Login->MenuA->各Menu　の順に処理されるとき使用
                if (formMenuA == null || formMenuA.IsDisposed)
                {
                    formMenuA = new FormMenuA(hp);
                    formMenuA.Show();
                }
                else
                {
                    MessageBox.Show("すでにこのメニューは開かれています。");
                    return false;
                }
                return true;
            }

            authList = Conv.AuthMembers( "MenuA" );
            if( authList != null )
            {
                if( authList.Contains( textBoxMemberCode.Text ) )
                {
                    labelMessage.Text = hp.MemberName + " さんが利用します。";
                    labelMessage.ForeColor = Color.Black;
                    if( formMenuA == null || formMenuA.IsDisposed )
                    {
                        formMenuA = new FormMenuA( hp );
                        formMenuA.Show();
                    }
                    else
                    {
                        MessageBox.Show( "すでにこのメニューは開かれています。" );
                        return false;
                    }
                    return true;
                }
            }


            authList = Conv.AuthMembers("MenuB");
            if (authList != null)
            {
                if (authList.Contains(textBoxMemberCode.Text))
                {
                    labelMessage.Text = hp.MemberName + " さんが利用します。";
                    labelMessage.ForeColor = Color.Black;
                    if (formMenuB == null || formMenuB.IsDisposed)
                    {
                        formMenuB = new FormMenuB(hp);
                        formMenuB.Show();
                    }
                    else
                    {
                        MessageBox.Show("すでにこのメニューは開かれています。");
                        return false;
                    }
                    return true;
                }
            }


            authList = Conv.AuthMembers( "MenuAdm");
            if( authList != null )
            {
                if( authList.Contains( textBoxMemberCode.Text ) )
                {
                    labelMessage.Text = hp.MemberName + " さんが利用します。";
                    labelMessage.ForeColor = Color.Black;
                    if( formMenuAdm == null || formMenuAdm.IsDisposed )
                    {
                        formMenuAdm = new FormMenuAdm( hp );
                        formMenuAdm.Show();
                    }
                    else
                    {
                        MessageBox.Show( "すでにこのメニューは開かれています。" );
                        return false;
                    }
                    return true;
                }
            }


            authList = Conv.AuthMembers( "MenuS" );
            if( authList != null )
            {
                if( authList.Contains( textBoxMemberCode.Text ) )
                {
                    labelMessage.Text = hp.MemberName + " さんが利用します。";
                    labelMessage.ForeColor = Color.Black;
                    if( formMenuS == null || formMenuS.IsDisposed )
                    {
                        formMenuS = new FormMenuS( hp );
                        formMenuS.Show();
                    }
                    else
                    {
                        MessageBox.Show( "すでにこのメニューは開かれています。" );
                        return false;
                    }
                    return true;
                }
            }

            authList = Conv.AuthMembers("CostInfo");
            if (authList != null)
            {
                if (authList.Contains(textBoxMemberCode.Text))
                {
                    labelMessage.Text = hp.MemberName + " さんが利用します。";
                    labelMessage.ForeColor = Color.Black;
                    if (formMenuInfo == null || formMenuInfo.IsDisposed)
                    {
                        formMenuInfo = new FormMenuInfo(hp);
                        formMenuInfo.Show();
                    }
                    else
                    {
                        MessageBox.Show("すでにこのメニューは開かれています。");
                        return false;
                    }
                    return true;
                }
            }

            labelMessage.Text = hp.MemberName + "さんは操作未承認です。[取消]をクリックしてください。";
            labelMessage.ForeColor = Color.Red;
            return false;
        }


        private void commonSelectError()
        {
            labelMessage.Text = "共通マスタの読み込みができませんでした!";
            labelMessage.ForeColor = Color.Red;
            return;
        }


        private void chooseMemberNameData()
        {
            MembersScData msds = FormMembersList.ReceiveItems(msd);
            if (msds == null) return;
            textBoxMemberCode.Text = msds.MemberCode;
        }

        
    }
}
