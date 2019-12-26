using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TID.Plugin.Resource.Event;

namespace TID.Plugin.Resource.Model
{
    public class ButtonInfo : ViewModelBase
    {
        private string buttontext;
        /// <summary>
        /// URL地址
        /// </summary>
        public string Buttontext
        {
            get { return Buttontext; }
            set { buttontext = value; RaisePropertyChanged(() => Buttontext); }
        }


        public RelayCommand SelectURL { get; set; }
        public ButtonInfo()
        {

            SelectURL = new RelayCommand(() =>
            {
                RresourceEventAggregator.GetEventAggregator().GetEvent<DiskFileEvent>().Publish(buttontext);
            });

        }

    }
}
