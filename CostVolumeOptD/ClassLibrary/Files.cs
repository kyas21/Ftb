using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary
{
    public static class Files
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        //private string fileName;
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        /*
        public Files()
        {
        }
        public Files(string fileName)
        {
            this.fileName = fileName;
        }
        */
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        //public string FileName { get; set; }
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public static string Open(string defFile,string defFolder,string defExt)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // 既定値表示
            openFileDialog.FileName = defFile;
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            openFileDialog.InitialDirectory = defFolder;
            //openFileDialog.InitialDirectory = @"C:\";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            openFileDialog.Filter = defExt + "ファイル(*." + defExt + ")|*." + defExt + "|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに
            //「すべてのファイル」が選択されているようにする
            openFileDialog.FilterIndex = 1;
            //タイトルを設定する
            openFileDialog.Title = "開くファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            openFileDialog.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            openFileDialog.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            openFileDialog.CheckPathExists = true;

            //ダイアログを表示する
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }



    }
}
