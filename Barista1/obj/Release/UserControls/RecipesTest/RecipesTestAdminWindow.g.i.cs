﻿#pragma checksum "..\..\..\..\UserControls\RecipesTest\RecipesTestAdminWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A41A7E355EDC73551C8E4BEA9D13B408C7EB8F49"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Barista1.UserControls.RecipesTest;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace Barista1.UserControls.RecipesTest {
    
    
    /// <summary>
    /// RecipesTestAdminWindow
    /// </summary>
    public partial class RecipesTestAdminWindow : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\..\UserControls\RecipesTest\RecipesTestAdminWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbTestName;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\UserControls\RecipesTest\RecipesTestAdminWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel AnswerStack;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\UserControls\RecipesTest\RecipesTestAdminWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddAnswer;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\UserControls\RecipesTest\RecipesTestAdminWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
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
            System.Uri resourceLocater = new System.Uri("/Barista1;component/usercontrols/recipestest/recipestestadminwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UserControls\RecipesTest\RecipesTestAdminWindow.xaml"
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
            this.txbTestName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.AnswerStack = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.btnAddAnswer = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\..\UserControls\RecipesTest\RecipesTestAdminWindow.xaml"
            this.btnAddAnswer.Click += new System.Windows.RoutedEventHandler(this.btnAddAnswer_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\..\UserControls\RecipesTest\RecipesTestAdminWindow.xaml"
            this.btnSave.Click += new System.Windows.RoutedEventHandler(this.btnSave_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

