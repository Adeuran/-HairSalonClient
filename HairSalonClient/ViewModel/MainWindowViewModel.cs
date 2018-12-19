using HairSalonClient.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonClient.ViewModel
{
    class MainWindowViewModel:BaseViewModel
    {
        #region PROPERTISES & FEILDS
        private string _navigationUri;

        public string NavigationUri
        {
            get { return _navigationUri; }
            set
            {
                _navigationUri = value;
                OnPropertyChanged("NavigationUri");
            }
        }

        #endregion

        #region CONSTROCTERS
        public MainWindowViewModel()
        {
            //Navigation Provider Ragistration
            NavigationServiceProvider._mainWindowInstance = this;

            //Initialize Propertises
            NavigationUri = "/View/MainPage.xaml";

        }
        #endregion
    }
}
