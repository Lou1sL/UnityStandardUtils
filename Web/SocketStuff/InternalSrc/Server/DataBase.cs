using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UnityStandardUtils.Web.SocketStuff
{
    public class DataBase
    {
        private MySqlConnection _conn;
        public DataBase(string IP, int Port, string User, string Password)
        {
            _conn = new MySqlConnection("server="+IP+":"+Port+";user="+User+";password="+Password+";database=ocss;charset=utf8;");
        }

        public DataTable Select(string sql)
        {
            MySqlDataAdapter sda = null;
            DataTable dt = null;
            try
            {
                sda = new MySqlDataAdapter(sql, _conn);
                dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Change(string sql)
        {
            MySqlCommand cmd = null;
            try
            {
                _conn.Open();
                cmd = new MySqlCommand(sql, _conn);
                int i = cmd.ExecuteNonQuery();
                _conn.Close();
                return i;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Add(string sql)
        {
            MySqlCommand cmd = null;
            try
            {
                _conn.Open();
                cmd = new MySqlCommand(sql, _conn);
                int i = cmd.ExecuteNonQuery();
                _conn.Close();
                return i;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public int Delete(string sql)
        {
            MySqlCommand cmd = null;
            try
            {
                _conn.Open();
                cmd = new MySqlCommand(sql, _conn);
                int i = cmd.ExecuteNonQuery();
                _conn.Close();
                return i;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
