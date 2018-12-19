using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonClient.Model.Vo
{
    class StylistVo
    {
        public uint StylistId { get; set; }
        public string StylistName { get; set; }
        public uint? AdditionalPrice { get; set; }
        public byte? PersonalDay { get; set; }
    }
}
