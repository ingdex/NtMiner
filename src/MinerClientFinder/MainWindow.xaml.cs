﻿using NTMiner.Views;
using NTMiner.Views.Ucs;
using NTMiner.Vms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NTMiner {
    public partial class MainWindow : BlankWindow {
        public MainWindowViewModel Vm {
            get {
                return (MainWindowViewModel)this.DataContext;
            }
        }

        public MainWindow() {
            InitializeComponent();
            NotiCenterWindow.Bind(this);
            this.AddEventPath<LocalIpSetInitedEvent>("本机IP集刷新后刷新状态栏", LogEnum.DevConsole,
                action: message => {
                    UIThread.Execute(() => Vm.RefreshLocalIps());
                }, location: this.GetType());
            this.AddCmdPath<ShowLocalIpsCommand>(LogEnum.DevConsole, action: message => {
                UIThread.Execute(() => {
                    LocalIpConfig.ShowWindow();
                });
            }, location: this.GetType());
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                this.DragMove();
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            DataGrid dg = (DataGrid)sender;
            Point p = e.GetPosition(dg);
            if (p.Y < dg.ColumnHeaderHeight) {
                return;
            }
            if (dg.SelectedItem != null) {
                string ip = (string)dg.SelectedItem;
                Clipboard.SetDataObject(ip);
                VirtualRoot.Out.ShowSuccess(ip, autoHideSeconds: 1, "复制成功");
            }
        }

        private void RbSpeed_Checked(object sender, RoutedEventArgs e) {
            RadioButton rbtn = (RadioButton)sender;
            if (rbtn == RbFast) {
                Vm.Timeout = 100;
            }
            else if (rbtn == RbNormal) {
                Vm.Timeout = 200;
            }
            else {
                Vm.Timeout = 300;
            }
        }
    }
}