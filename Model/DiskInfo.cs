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
    public class DiskInfo : ViewModelBase
    {

        private string diskName;
        /// <summary>
        /// 磁盘名称
        /// </summary>
        public string DiskName
        {
            get { return diskName; }
            set { diskName = value; RaisePropertyChanged(() => DiskName); }
        }

        private string diskRootDirectory;
        /// <summary>
        /// 磁盘的根目录
        /// </summary>
        public string DiskRootDirectory
        {
            get { return diskRootDirectory; }
            set { diskRootDirectory = value; RaisePropertyChanged(() => DiskRootDirectory); }
        }

        public RelayCommand DiskFile { get; set; }
        public DiskInfo()
        {
            DiskFile = new RelayCommand(() =>
            {
                RresourceEventAggregator.GetEventAggregator().GetEvent<DiskFileEvent>().Publish(diskRootDirectory);
            });

        }
    }
}
