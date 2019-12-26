using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.IO;
using System.Threading;
using TID.Plugin.Resource.Model;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using TID.Plugin.Resource.Event;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Linq;


namespace TID.Plugin.Resource.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand<string> ChoiceGrid { get; set; }  ///选择前台显示的页面
        public RelayCommand UpPage { get; set; }/// 返回上一级
        public RelayCommand<string> Openbutton { get; set; }//打开特殊路径的文件夹
        public RelayCommand<string> ChoiceRWLB { get; set; }//选择任务列表
        public RelayCommand<string> Islist { get; set; }//选择是否为图片或者列表略缩图
        public MainWindowViewModel()
        {
            UpPage = new RelayCommand(() =>
            {
                string[] prentsite = NowUrl.Split('\\');
                List<string> tempdate = new List<string>();
                tempdate = prentsite.ToList().Where(q => !string.IsNullOrEmpty(q)).ToList();
                tempdate.Remove(tempdate[tempdate.Count - 1]);
                string data = "";
                if (tempdate.Count == 0)
                    data = NowUrl;
                else
                    tempdate.ForEach(q =>
                    {
                        data = data + q + @"\";
                    });
                RresourceEventAggregator.GetEventAggregator().GetEvent<ReturnPageEvent>().Publish(data);
            });
            Openbutton = new RelayCommand<string>((e) =>
            {
                switch (e)
                {
                    case ("desk"):
                        specialURL = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        break;
                    case ("download"):
                        specialURL = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                        break;
                    case ("document"):
                        specialURL = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        break;
                    case ("img"):
                        specialURL = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                        break;
                }

                RresourceEventAggregator.GetEventAggregator().GetEvent<LeftButtonEvent>().Publish(specialURL);
            });

            #region 前台显示
            IsShowWWZY = Visibility.Hidden;
            IsShowRWLB = Visibility.Hidden;
            IsShowYZYK = Visibility.Hidden;
            IsShowBDCP = Visibility.Visible;
            ChoiceGrid = new RelayCommand<string>((e) =>
            {
                switch (e)
                {
                    case "1":
                        IsShowWWZY = Visibility.Visible;
                        IsShowRWLB = Visibility.Hidden;
                        IsShowYZYK = Visibility.Hidden;
                        IsShowBDCP = Visibility.Hidden;
                        break;
                    case "2":
                        IsShowWWZY = Visibility.Hidden;
                        IsShowRWLB = Visibility.Visible;
                        IsShowYZYK = Visibility.Hidden;
                        IsShowBDCP = Visibility.Hidden;
                        break;
                    case "3":
                        IsShowWWZY = Visibility.Hidden;
                        IsShowRWLB = Visibility.Hidden;
                        IsShowYZYK = Visibility.Visible;
                        IsShowBDCP = Visibility.Hidden;
                        break;
                    case "4":
                        IsShowWWZY = Visibility.Hidden;
                        IsShowRWLB = Visibility.Hidden;
                        IsShowYZYK = Visibility.Hidden;
                        IsShowBDCP = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            });
            XZLB = Visibility.Visible;
            SCLB = Visibility.Hidden;
            YWC = Visibility.Hidden;
            ChoiceRWLB = new RelayCommand<string>((e) =>
            {
                switch (e)
                {
                    case "1":
                        XZLB = Visibility.Visible;
                        SCLB = Visibility.Hidden;
                        YWC = Visibility.Hidden;
                        break;
                    case "2":
                        XZLB = Visibility.Hidden;
                        SCLB = Visibility.Visible;
                        YWC = Visibility.Hidden;
                        break;
                    case "3":
                        XZLB = Visibility.Hidden;
                        SCLB = Visibility.Hidden;
                        YWC = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            });

            ShowList = Visibility.Visible;
            ShowImg = Visibility.Hidden;
            Islist = new RelayCommand<string>((e) =>
            {
                switch (e)
                {
                    case "img":
                        ShowList = Visibility.Visible;
                        ShowImg = Visibility.Hidden;
                        break;
                    case "list":
                        ShowList = Visibility.Hidden;
                        ShowImg = Visibility.Visible;
                        break;
                }
            });

            #endregion
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    DiskInfoData = new ObservableCollection<DiskInfo>();
                    LoadAllDevices();
                    Thread.Sleep(5000);
                }
            });
            thread.IsBackground = true;
            thread.Start();
            LoadFile(@"C:/");
            GetNowUrl();
            RresourceEventAggregator.GetEventAggregator().GetEvent<DiskFileEvent>().Subscribe((e) =>
            {
                LoadFile(e);
            });///磁盘的文件
            RresourceEventAggregator.GetEventAggregator().GetEvent<NextlistEvent>().Subscribe((e) =>
            {
                LoadFile(e);
            });///文件夹里的文件
            RresourceEventAggregator.GetEventAggregator().GetEvent<LeftButtonEvent>().Subscribe((e) =>
            {
                LoadFile(e);
            });///特殊文件夹的文件
            RresourceEventAggregator.GetEventAggregator().GetEvent<ReturnPageEvent>().Subscribe((e) =>
            {
                LoadFile(e);
            });///返回上一级

        }

        #region 前台的显示属性

        private Visibility showList;
        /// <summary>
        /// 列表显示
        /// </summary>
        public Visibility ShowList
        {
            get { return showList; }
            set { Set(ref showList, value); }
        }

        private Visibility showImg;
        /// <summary>
        /// 图片显示
        /// </summary>
        public Visibility ShowImg
        {
            get { return showImg; }
            set { Set(ref showImg, value); }
        }

        private Visibility isShowWWZY;
        /// <summary>
        /// 外网资源
        /// </summary>
        public Visibility IsShowWWZY
        {
            get { return isShowWWZY; }
            set { Set(ref isShowWWZY, value); }
        }

        private Visibility isShowRWLB;
        /// <summary>
        /// 任务列表
        /// </summary>
        public Visibility IsShowRWLB
        {
            get { return isShowRWLB; }
            set { Set(ref isShowRWLB, value); }
        }

        private Visibility isShowYZYK;
        /// <summary>
        /// 云资源库
        /// </summary>
        public Visibility IsShowYZYK
        {
            get { return isShowYZYK; }
            set { Set(ref isShowYZYK, value); }
        }

        private Visibility isShowBDCP;
        /// <summary>
        /// 本地磁盘
        /// </summary>
        public Visibility IsShowBDCP
        {
            get { return isShowBDCP; }
            set { Set(ref isShowBDCP, value); }
        }

        private Visibility xzlb;
        /// <summary>
        /// 下载列表
        /// </summary>
        public Visibility XZLB
        {
            get { return xzlb; }
            set { Set(ref xzlb, value); }

        }

        private Visibility sclb;
        /// <summary>
        /// 上传列表
        /// </summary>
        public Visibility SCLB
        {
            get { return sclb; }
            set { Set(ref sclb, value); }

        }

        private Visibility ywc;
        /// <summary>
        /// 已完成
        /// </summary>
        public Visibility YWC
        {
            get { return ywc; }
            set { Set(ref ywc, value); }

        }
        #endregion
       
        #region 磁盘信息加载
        private ObservableCollection<DiskInfo> diskInfoData;
        /// <summary>
        /// 硬盘信息表
        /// </summary>
        public ObservableCollection<DiskInfo> DiskInfoData
        {
            get { return diskInfoData; }
            set { diskInfoData = value; RaisePropertyChanged(() => DiskInfoData); }
        }

        /// <summary>  
        /// 加载所有磁盘  
        /// </summary>  
        public void LoadAllDevices()
        {
            DiskInfoData = new ObservableCollection<DiskInfo>();
            DriveInfo[] driver = DriveInfo.GetDrives();
            foreach (DriveInfo drv in driver)
            {
                DiskInfo disk = new DiskInfo();
                disk.DiskName = drv.Name;
                disk.DiskRootDirectory = drv.RootDirectory.ToString();
                DiskInfoData.Add(disk);
            }
        }
        #endregion


        private ObservableCollection<ButtonInfo> buttonInfos;
        /// <summary>
        ///按钮地址信息
        /// </summary>
        public ObservableCollection<ButtonInfo> ButtonInfos
        {
            get { return buttonInfos; }
            set { buttonInfos = value; RaisePropertyChanged(() => ButtonInfos); }
        }
        public void GetNowUrl()
        {
            buttonInfos = new ObservableCollection<ButtonInfo>();
            ButtonInfo buttonInfo = new ButtonInfo();
            buttonInfo.Buttontext = NowUrl;
            ButtonInfos.Add(buttonInfo);
        }

        public string NowUrl { get; set; }///当前的地址的变量

        private string specialURL;
        /// <summary>
        /// 特殊文件夹的路径
        /// </summary>
        public string SpecialURL
        {
            get { return specialURL; }
            set { specialURL = value; RaisePropertyChanged(() => SpecialURL); }
        }

        #region 文件信息的加载
        private ObservableCollection<FileInfos> fileInfosData;
        /// <summary>
        /// 文件信息表
        /// </summary>
        public ObservableCollection<FileInfos> FileInfosData
        {
            get { return fileInfosData; }
            set { fileInfosData = value; RaisePropertyChanged(() => FileInfosData); }
        }
        private FileInfos fileInfos;
        /// <summary>
        /// 选中项的文件信息，SelectedItem
        /// </summary>
        public FileInfos FileInfos
        {
            get { return fileInfos; }
            set { fileInfos = value; RaisePropertyChanged(() => FileInfos); }
        }
        /// <summary>
        /// 加载文件名和图片的方法
        /// </summary>
        public void LoadFile(string path)
        {
            if (File.Exists(path))
            {
                // 是文件

            }
            else if (Directory.Exists(path))
            {
                NowUrl = path;
                try
                {
                    DirectoryInfo root = new DirectoryInfo(path);
                    DirectoryInfo[] dics = root.GetDirectories();
                    FileInfosData = new ObservableCollection<FileInfos>();
                    for (int i = 0; i < dics.Length; i++)
                    {
                        FileInfos fileInfo = new FileInfos();
                        fileInfo.FileName = dics[i].Name;
                        fileInfo.CurrentDirectory = dics[i].FullName;   //添加当前文件夹的路径
                        fileInfo.ParentDirectory = dics[i].Parent.FullName;//添加当前文件夹的父级路径
                        fileInfo.ShowFolder = Visibility.Visible;
                        FileInfosData.Add(fileInfo);
                    }
                    foreach (FileInfo f in root.GetFiles())
                    {
                        FileInfos fileInfo = new FileInfos();
                        fileInfo.FileName = f.Name;//获取文件名称
                        string fileurl = f.FullName;//获取文件所在路径
                        string parentDirectory = f.Directory.FullName;//获取文件的父级菜单
                        fileInfo.Suffix = f.Extension;//获取文件的后缀名
                        string filePicName = fileInfo.Suffix;//文件图片名称
                        if (fileInfo.Suffix == ".exe")
                        {
                            filePicName = f.FullName + f.Name;
                            MD5 md5 = MD5.Create();//创建md5算法
                            byte[] c = System.Text.Encoding.Default.GetBytes(filePicName);
                            byte[] b = md5.ComputeHash(c);//用来计算指定数组的hash值
                            //将每一个字节数组中的元素都tostring，在转成16进制
                            string newStr = null;
                            for (int i = 0; i < b.Length; i++)
                            {
                                newStr += b[i].ToString("x2");  //ToString(param);//传入不同的param可以转换成不同的效果
                            }
                            filePicName = newStr;
                        }
                        string tempImgSource = AppDomain.CurrentDomain.BaseDirectory + @"Image\" + filePicName + ".ico";//将文件保存在指定的路径
                        System.Drawing.Icon.ExtractAssociatedIcon(fileurl).ToBitmap().Save(tempImgSource);//将文件的路径存在创建的目录下
                        fileInfo.Icon = tempImgSource;//赋值
                        fileInfo.ShowFolder = Visibility.Hidden;
                        FileInfosData.Add(fileInfo);
                    }

                }
                catch (Exception e)
                {

                }
            }
        }
        #endregion


    }
}
