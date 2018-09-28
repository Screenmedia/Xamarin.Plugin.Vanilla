// -----------------------------------------------------------------------
//  <copyright file="ViewController.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2018. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Screenmedia.Plugin.Vanilla.Test.TestApp.iOS
{
    using System;
    using UIKit;

    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle)
            : base(handle)
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
