﻿using HairSalonClient.Model.Vo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonClient.Model.Repository
{
    class ReservedServiceRepository : BaseRepository
    {
        #region field
        readonly List<ReservedServiceVo> _list;
        #endregion

        #region ctor , singleTon
        private static ReservedServiceRepository _reservedServiceRepository;

        public static ReservedServiceRepository RSR
        {
            get
            {
                if (_reservedServiceRepository == null)
                    _reservedServiceRepository = new ReservedServiceRepository();
                return _reservedServiceRepository;
            }
        }


        private ReservedServiceRepository()
        {
            _list = GetReservedServices();
        }
        #endregion

        #region RerservedService Method
        public List<ReservedServiceVo> GetReservedServicesFromLocal()
        {
            return new List<ReservedServiceVo>(_list);
        }

        public List<ReservedServiceVo> GetReservedServices(uint resNum)
        {
            _conn.Msc.Open();
            List<ReservedServiceVo> list = new List<ReservedServiceVo>();
            _sql = "SELECT * FROM reservedservice WHERE resNum = @resNum";
            MySqlCommand cmd = new MySqlCommand(_sql, _conn.Msc);

            cmd.Parameters.AddWithValue("@resNum", resNum);

            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ReservedServiceVo rsv = new ReservedServiceVo();
                rsv.ResNum = (uint)rdr["resNum"];
                rsv.SerId = (ushort)rdr["serId"];
                list.Add(rsv);
            }
            _conn.Msc.Close();
            return list;
        }

        public List<ReservedServiceVo> GetReservedServices()
        {
            _conn.Msc.Open();
            List<ReservedServiceVo> list = new List<ReservedServiceVo>();
            _sql = "SELECT * FROM reservedservice";
            MySqlCommand cmd = new MySqlCommand(_sql, _conn.Msc);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ReservedServiceVo rsv = new ReservedServiceVo();
                rsv.ResNum = (uint)rdr["resNum"];
                rsv.SerId = (ushort)rdr["serId"];
                list.Add(rsv);
            }
            _conn.Msc.Close();
            return list;
        }

        public bool InsertReservedService(ReservedServiceVo rsv)
        {
            _conn.Msc.Open();
            _sql = "INSERT INTO reservedservice(resNum,serId) VALUES(@resNum,@serId)";
            MySqlCommand cmd = new MySqlCommand(_sql, _conn.Msc);

            cmd.Parameters.AddWithValue("@resNum", rsv.ResNum);
            cmd.Parameters.AddWithValue("@serId", rsv.SerId);

            if (cmd.ExecuteNonQuery() == -1)
            {
                _conn.Msc.Close();
                return false;
            }

            _conn.Msc.Close();
            return true;
        }

        public bool RemoveReservedService(uint resNum, uint serId)
        {
            _conn.Msc.Open();
            _sql = "DELETE FROM reservedservice WHERE resNum = @resNum AND serId = @serId";
            MySqlCommand cmd = new MySqlCommand(_sql, _conn.Msc);

            cmd.Parameters.AddWithValue("@resNum", resNum);
            cmd.Parameters.AddWithValue("@serId", serId);

            if (cmd.ExecuteNonQuery() != -1)
            {
                _conn.Msc.Close();
                return true;
            }
            _conn.Msc.Close();
            return false;


        }
        #endregion
    }
}
