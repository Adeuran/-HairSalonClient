using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonClient.Model.Repository
{
    class BaseRepository
    {
        protected Connection _conn;
        protected string _sql;

        protected BaseRepository()
        {
            _conn = Connection.Conn;
        }
    }
}
