using HairSalonClient.Model;
using HairSalonClient.Model.Repository;
using HairSalonClient.Model.Util;
using HairSalonClient.Model.Vo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HairSalonClient.ViewModel
{
    class MainPageViewModel : BaseViewModel
    {
        #region PROPERTISES & FEILDS
        //Lists

        //DB 서비스 목록
        private ObservableCollection<ServiceVo> _services;
        public ObservableCollection<ServiceVo> Services
        {
            get { return _services; }
            set
            {
                _services = value;
                OnPropertyChanged("Services");
            }
        }

        //예약할 서비스 목록
        private ObservableCollection<ServiceVo> _reserveServices;
        public ObservableCollection<ServiceVo> ReserveServices
        {
            get { return _reserveServices; }
            set
            {
                _reserveServices = value;
                OnPropertyChanged("ReserveServices");
            }
        }

        //Propertises

        //현재 선택한 서비스
        private ServiceVo _selService;
        public ServiceVo SelService
        {
            get { return _selService; }
            set
            {
                _selService = value;
                IsAddBtnEnable = true;
            }
        }

        //예약 목록에서 선택한 서비스
        private ServiceVo _selResService;
        public ServiceVo SelResService
        {
            get { return _selResService; }
            set
            {
                _selResService = value;
                IsRemoveBtnEnable = true;
            }
        }
        //Add버튼 동작 여부
        private bool _isAddBtnEnable;
        public bool IsAddBtnEnable
        {
            get { return _isAddBtnEnable; }
            set
            {
                _isAddBtnEnable = value;
                OnPropertyChanged("IsAddBtnEnable");
            }
        }
        //Remove버튼 동작 여부
        private bool _isRemoveBtnEnable;
        public bool IsRemoveBtnEnable
        {
            get { return _isRemoveBtnEnable; }
            set
            {
                _isRemoveBtnEnable = value;
                OnPropertyChanged("IsRemoveBtnEnable");
            }
        }
        //Repository
        private ServiceRepository _servRepository;
        //Commands
        public Command AddCommand { get; set; }
        public Command RemoveCommand { get; set; }
        public Command NextCommand { get; set; }
        #endregion

        #region CONSTRUCTOR
        public MainPageViewModel()
        {
            //Get From Repository
            _servRepository = ServiceRepository.SR;
            Services = new ObservableCollection<ServiceVo>(_servRepository.GetServices());

            //Create Instance
            ReserveServices = new ObservableCollection<ServiceVo>(new List<ServiceVo>());
            //Commands
            AddCommand = new Command(ExecuteAddMethod);
            RemoveCommand = new Command(ExecuteRemoveMethod);
            NextCommand = new Command(ExecuteNextMethod);
        }
        #endregion

        #region Method
        private void ExecuteAddMethod(object parameter)
        {
            int index = Services.IndexOf(SelService);
            ReserveServices.Add(SelService);
            Services.Remove(SelService);
        }
        private void ExecuteRemoveMethod(object parameter)
        {
            int index = ReserveServices.IndexOf(SelResService);
            Services.Add(SelResService);
            ReserveServices.Remove(SelResService);
        }
        private void ExecuteNextMethod(object parameter)
        {
            if (ReserveServices.Count == 0)
                MessageBox.Show("적어도 하나의 서비스를 추가해야 합니다.");
            else
            {
                UploadData.Ud.ResServices = ReserveServices;
                UploadData.Ud.CalcUseageTime();
                NavigationServiceProvider.Navigate("/View/Reserve.xaml");
            }
        }
        #endregion
    }
}
