using HairSalonClient.Model;
using HairSalonClient.Model.Repository;
using HairSalonClient.Model.Util;
using HairSalonClient.Model.Vo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HairSalonClient.ViewModel
{
    class ReserveViewModel : BaseViewModel
    {
        #region PROPERTISES & FIELDS
        //Propertises
        public string Name { get; set; }
        public string Tel { get; set; }
        public bool IsMan { get; set; }
        public bool IsWoman { get; set; }
        public DateTime BirthDay { get; set; }
        public StylistVo Stylist { get; set; }
        public DateTime ResDate { get; set; }
        public int ResHour { get; set; }
        public int ResMinute { get; set; }
        public string Note { get; set; }

        private DateTime _availableDate;

        public DateTime AvailbleDate
        {
            get { return _availableDate; }
            set { _availableDate = value;
                OnPropertyChanged("AvailableDate");
            }
        }


        private List<int> _availableHour;
        public List<int> AvailableHour
        {
            get { return _availableHour; }
            set { _availableHour = value;
                OnPropertyChanged("AvailableHour");
            }
        }
        private List<int> _availableMinute;
        public List<int> AvailableMinute
        {
            get { return _availableMinute; }
            set
            {
                _availableMinute = value;
                OnPropertyChanged("AvailableMinute");
            }
        }

        private List<StylistVo> _stylists;
        public List<StylistVo> Stylists
        {
            get { return _stylists; }
            set { _stylists = value; }
        }

        //Repository
        private UploadData _uploadData;
        private StylistRepository _stylistRepository;

        //Commands
        public Command SendCommand { get; set; }
        #endregion

        #region CONSTRUCTOR
        public ReserveViewModel()
        {
            //Get From Repository
            _stylistRepository = StylistRepository.SR;
            Stylists = _stylistRepository.GetStylists();
            _uploadData = UploadData.Ud;

            //Create Instances
            SendCommand = new Command(ExecuteSendMethod);
            AvailableHour = new List<int>();
            AvailableMinute = new List<int>() { 0, 30 };

            //Set Default Value
            BirthDay = DateTime.Today;
            ResDate = DateTime.Today;
            AvailbleDate = DateTime.Today;
            for (int x = 0; x < 24; x++)
            {
                AvailableHour.Add(x);
            }
        }
        #endregion

        #region METHODS
        public void ExecuteSendMethod(object parameter)
        {
            if (String.IsNullOrWhiteSpace(Name) ||
                String.IsNullOrWhiteSpace(Tel) ||
                !(IsMan || IsWoman) ||
                BirthDay == DateTime.Today ||
                Stylist == null)
            {
                MessageBox.Show("한 개 이상의 입력란이 입력되지 않았습니다.");
                return;
            }

            else
            {
                DateTime StartTime = new DateTime(ResDate.Year, ResDate.Month, ResDate.Day, ResHour, ResMinute, 0);

                if (HasReservations(Stylist.StylistId, StartTime, ResDate, ResHour, ResMinute, _uploadData.UseageTime)) { 
                    MessageBox.Show("이미 예약이 있는 시간대입니다.");
                    return;
                }
                else
                {
                    //입력 내용 Vo로 저장
                    ReservationVo reservation = new ReservationVo();
                    reservation.UserName = Name;
                    reservation.UserTel = Tel;
                    reservation.Gender = IsMan ? 0 : 1;
                    reservation.UserBirthday = BirthDay;
                    reservation.StylistId = Stylist.StylistId;
                    reservation.StartAt = StartTime;
                    reservation.Note = Note;
                    reservation.EndAt = StartTime.AddMinutes(_uploadData.UseageTime);

                    //Vo를 업로드 클래스에 등록
                    _uploadData.ReservationVo = reservation;

                    //업로드 클래스 데이터 업로드
                    _uploadData.Upload();

                    NavigationServiceProvider.Navigate("/View/Finish.xaml");
                }
            }
        }

        private bool HasReservations(uint? stylistId,DateTime start,DateTime date, int hour,int minute, int usuageTime) //time  --> 예약 끝나는 시간
        {
            List<ReservationVo> reservations = ReservationRepository.Rr.GetReservations();
            DateTime endAt = start.AddHours(hour).AddMinutes(minute);

            //결과가 없으면
            bool isNotNull = reservations.Find(x => x.StylistId == stylistId) is ReservationVo;
            if (!isNotNull)
                return false;   //예약 없음

            foreach (ReservationVo res in reservations.Where(x => x.StylistId == stylistId))
            {
                TimeSpan temp = res.EndAt - res.StartAt;
                int resUsageTime = temp.Hours*60 + temp.Minutes;

                if (usuageTime == 0)
                    return false;
                else if (resUsageTime == 0)
                    continue;
                else if (start == res.StartAt)
                    return true;
                else if (res.StartAt <= start && start < res.EndAt)
                    return true;
                else if (res.StartAt < endAt && endAt <= res.EndAt)
                    return true;
            }
            return false;
        }
        #endregion
    }
}