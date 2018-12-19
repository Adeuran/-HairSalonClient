using HairSalonClient.Model.Repository;
using HairSalonClient.Model.Vo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonClient.Model
{
    class UploadData
    {
        #region singleTon 
        private static UploadData _ud;
        public static UploadData Ud
        {
            get
            {
                if (_ud == null)
                    _ud = new UploadData();
                return _ud;
            }

        }
        private UploadData()
        {
            UseageTime = 0;
            _resRep = ReservationRepository.Rr;
            _reservedServiceRepository = ReservedServiceRepository.RSR;
        }
        #endregion

        #region PROPERTISES & FIELDS
        public ObservableCollection<ServiceVo> ResServices { get; set; }
        public ReservationVo ReservationVo { get; set; }
        public int UseageTime { get; set; }

        private ReservationRepository _resRep;
        private ReservedServiceRepository _reservedServiceRepository;
        #endregion

        #region METHODS
        public void CalcUseageTime()
        {
            foreach (ServiceVo service in ResServices)
            {
                UseageTime += service.ServiceTime;
            }
        }
        public void Upload()
        {
            _resRep.InsertReservation(ReservationVo);
            uint id = _resRep.GetRecentNum();
            foreach (ServiceVo service in ResServices)
            {
                ReservedServiceVo reservedService = new ReservedServiceVo();
                reservedService.ResNum = id;
                reservedService.SerId = service.ServiceId;
                _reservedServiceRepository.InsertReservedService(reservedService);
            }
        }
        #endregion
    }
}
