using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Data;

namespace Quark.source.Utils.Database
{
    class InternalBD
    {
        private SQLiteConnection m_dbConn;
        private SQLiteCommand m_sqlCmd;
        private static InternalBD instance;

        private InternalBD()
        {
            if (!File.Exists(Globals.db_path))
            {
                MessageBox.Show("main.db not found!", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }

            this.m_dbConn = new SQLiteConnection($"Data Source={Globals.db_path};Version=3;");
            this.m_sqlCmd = new SQLiteCommand();
            this.m_dbConn.Open();
        }

        public static InternalBD getInstance()
        {
            if (instance == null)
                instance = new InternalBD();
            return instance;
        }

        ~InternalBD()
        {
            try
            {
                this.m_dbConn.Close();
            }
            catch (Exception)
            {
            }
        }

        public List<string> GetGroups()
        {
            DataTable _dTable = new DataTable();
            string _sqlQuery = "SELECT group_name FROM groups";
            List<string> _data = new List<string>();
            try
            {
                
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(_sqlQuery, m_dbConn);
                adapter.Fill(_dTable);

                for (int i = 0; i < _dTable.Rows.Count; i++)
                    foreach (var a in _dTable.Rows[i].ItemArray)
                        _data.Add(a.ToString());
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return _data;
        }

        public List<string> GetStudents(string group="")
        {
            DataTable _dTable = new DataTable();
            string _sqlQuery;

            if (group == "")
                _sqlQuery = "SELECT student FROM students";
            else
                _sqlQuery = $"SELECT student FROM students WHERE group_name=\"{group}\"";

            List<string> _data = new List<string>();
            try
            {

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(_sqlQuery, m_dbConn);
                adapter.Fill(_dTable);

                for (int i = 0; i < _dTable.Rows.Count; i++)
                    foreach (var a in _dTable.Rows[i].ItemArray)
                        _data.Add(a.ToString());
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return _data;
        }
    }
}
