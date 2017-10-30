// -----------------------------------------------------------------------
//  <copyright file="ViewController.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2017. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace VanillaSample.iOS
{
    using System;
    using UIKit;
    using VanillaSample.Core.ViewModels;

    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        { 
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var vm = new ViewModel();
            Label.Text = vm.IceCreamFlavour;
        }
    }
}
