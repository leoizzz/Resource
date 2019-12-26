using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TID.Plugin.Resource.Event;

namespace TID.Plugin.Resource.Model
{
    public class FileInfos : ViewModelBase
    {

        private string fileName;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; RaisePropertyChanged(() => FileName); }
        }

        private string icon;
        /// <summary>
        /// 文件图标
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { icon = value; RaisePropertyChanged(() => Icon); }
        }

        private string currentDirectory;
        /// <summary>
        /// 当前文件的文件路径
        /// </summary>
        public string CurrentDirectory
        {
            get { return currentDirectory; }
            set { currentDirectory = value; RaisePropertyChanged(() => CurrentDirectory); }
        }


        private string suffix;
        /// <summary>
        /// 文件后缀
        /// </summary>
        public string Suffix 
        {
            get { return suffix; }
            set { suffix = value; RaisePropertyChanged(() => Suffix); }
        }

        private string filetype;
        /// <summary>
        /// 文件类型
        /// </summary>
        public string Filetype
        {
            get { return filetype; }
            set { filetype = value; RaisePropertyChanged(() => Filetype); }
        }

        private string parentDirectory;
        /// <summary>
        /// 父级地址
        /// </summary>
        public string ParentDirectory
        {
            get { return parentDirectory; }
            set { parentDirectory = value; RaisePropertyChanged(() => ParentDirectory); }

        }

        public Visibility ShowFolder { get; set; }//如果是文件夹则为Visible,如果是文件则为Hidden

        public RelayCommand NextList { get; set; }
        public FileInfos()
        {
            NextList = new RelayCommand(() =>
            {
                RresourceEventAggregator.GetEventAggregator().GetEvent<NextlistEvent>().Publish(CurrentDirectory);
            });
        }
     

    }
}
