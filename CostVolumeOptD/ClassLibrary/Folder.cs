using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Folder
    {
        public static string DefaultExcelTemplate(string bookName)
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ExcelTemplate\" + bookName;
        }

        public static string DefaultLocation()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }


        public static string UserDeskTop()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        public static string MyDocuments()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }



        /// <summary>
        ///指定フォルダの直下にある全てのフォルダ名を取得するメソッド 
        /// </summary>
        /// <param name="Folder_Name">対象のフォルダ</param>
        /// <returns>取得したフォルダ名の配列</returns>
        public static string[] GetFolderName(string Folder_Name)
        {
            string[] Folder_List; // フォルダ名格納用配列

            // 指定フォルダが無い場合
            if (System.IO.Directory.Exists(Folder_Name) == false)
            {
                Folder_List = new string[0]; // 要素数が0の配列を返却
                return Folder_List;
            }

            // フォルダ名取得
            Folder_List = System.IO.Directory.GetDirectories(Folder_Name);

            return Folder_List;

        }

        /// <summary>
        /// 指定フォルダの直下にある全てのファイル名を取得するメソッド 
        /// </summary>
        /// <param name="Folder_Name">対象のフォルダ</param>
        /// <returns>取得したファイル名の配列</returns>
        public static string[] GetFileName(string Folder_Name)
        {
            string[] File_List; // ファイル名格納用配列

            // 指定フォルダが無い場合
            if (System.IO.Directory.Exists(Folder_Name) == false)
            {
                File_List = new string[0]; // 要素数が0の配列を返却
                return File_List;
            }

            // ファイル名取得
            File_List = System.IO.Directory.GetFiles(Folder_Name);

            return File_List;
        }

        /// <summary>
        /// フォルダ複写用メソッド 
        /// </summary>
        /// <param name="Source_Folder_Name">複写元フォルダ</param>
        /// <param name="Dest_Folder_Name">複写先フォルダ</param>
        /// <param name="Overwrite_Flag">複写先に既に同名ファイルが存在する場合の指示。 true:上書きする / false: 上書きしない</param>
        public static void CopyFolder(string Source_Folder_Name, string Dest_Folder_Name, bool Overwrite_Flag)
        {
            // 複写先フォルダが存在しない場合、フォルダを作成
            if (!System.IO.Directory.Exists(Dest_Folder_Name))
            {
                // フォルダ作成
                System.IO.Directory.CreateDirectory(Dest_Folder_Name);

                // 作成したフォルダに、フォルダの属性を複写
                System.IO.File.SetAttributes(Dest_Folder_Name, System.IO.File.GetAttributes(Source_Folder_Name));

                Overwrite_Flag = true; // 複写先フォルダを作ったのだから、そこにファイルは存在しないので、以後上書きで可
            }

            // 複写元フォルダ直下のファイル名を取得
            string[] File_List = GetFileName(Source_Folder_Name);

            // 複写先に既に同名ファイルが存在する場合に、上書きが許可されている場合
            if (Overwrite_Flag)
            {
                // 複写元フォルダの直下にあるファイルを複写
                foreach (string File_Work in File_List)
                {
                    // 複写先フォルダ名の末尾に、取得ファイル名を付加
                    string Dest_Direc_Work = System.IO.Path.Combine(Dest_Folder_Name, System.IO.Path.GetFileName(File_Work));

                    // ファイル複写
                    System.IO.File.Copy(File_Work, Dest_Direc_Work, true);
                }
            }
            else
            {
                // 複写元フォルダの直下にあるファイルを複写
                foreach (string File_Work in File_List)
                {
                    // 複写先フォルダ名の末尾に、取得ファイル名を付加
                    string Dest_Direc_Work = System.IO.Path.Combine(Dest_Folder_Name, System.IO.Path.GetFileName(File_Work));

                    if (!System.IO.File.Exists(Dest_Direc_Work))
                    {
                        // ファイル複写
                        System.IO.File.Copy(File_Work, Dest_Direc_Work, false);
                    }
                }
            }

            // 複写元フォルダ直下のフォルダ名を取得
            string[] Folder_List = GetFolderName(Source_Folder_Name);

            // 複写元フォルダの直下にある全フォルダに対して、フォルダ複写用メソッド(自メソッド)を実行
            foreach (string Directory_Work in Folder_List)
            {
                // 複写先フォルダ名の末尾に、複写元フォルダ名を付加
                string Dest_Direc_Work = System.IO.Path.Combine(Dest_Folder_Name, System.IO.Path.GetFileName(Directory_Work));

                // 指定フォルダ直下の各フォルダにおいて、複写処理を実行
                CopyFolder(Directory_Work, Dest_Direc_Work, Overwrite_Flag);
            }

        }

        /// <summary>
        /// 指定されたフォルダを作成する。既存の場合はそのまま。
        /// </summary>
        /// <param name="folderName"></param>
        public static void CreateFolder(string folderName)
        {
            if (!System.IO.Directory.Exists(folderName))
            {
                // フォルダ作成
                System.IO.Directory.CreateDirectory(folderName);
            }
        }




    }
}
