﻿#pragma checksum "..\..\Manager.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F782B4E91C11269175FDC9F9B1E8013302F65D1062A933102368346C9C622EF3"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Fuck;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Fuck {
    
    
    /// <summary>
    /// Manager
    /// </summary>
    public partial class Manager : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label workerIDlabel;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem Workers;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid WorkersGrid;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Addorkerbutton;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteWorker;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem Results;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ResultGrid;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem Storage;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid StorageGrid;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem Vans;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid VansGrid;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddVan;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\Manager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Restart;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Fuck;component/manager.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Manager.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\Manager.xaml"
            ((Fuck.Manager)(target)).Initialized += new System.EventHandler(this.Window_Initialized);
            
            #line default
            #line hidden
            return;
            case 2:
            this.workerIDlabel = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.Workers = ((System.Windows.Controls.TabItem)(target));
            return;
            case 4:
            this.WorkersGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 15 "..\..\Manager.xaml"
            this.WorkersGrid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.WorkersGrid_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Addorkerbutton = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\Manager.xaml"
            this.Addorkerbutton.Click += new System.Windows.RoutedEventHandler(this.Addorkerbutton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.DeleteWorker = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\Manager.xaml"
            this.DeleteWorker.Click += new System.Windows.RoutedEventHandler(this.DeleteWorker_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Results = ((System.Windows.Controls.TabItem)(target));
            return;
            case 8:
            this.ResultGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 9:
            this.Storage = ((System.Windows.Controls.TabItem)(target));
            return;
            case 10:
            this.StorageGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 11:
            this.Vans = ((System.Windows.Controls.TabItem)(target));
            return;
            case 12:
            this.VansGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 13:
            this.AddVan = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\Manager.xaml"
            this.AddVan.Click += new System.Windows.RoutedEventHandler(this.AddVan_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.Restart = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\Manager.xaml"
            this.Restart.Click += new System.Windows.RoutedEventHandler(this.Restart_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
