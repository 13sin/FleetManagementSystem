﻿#pragma checksum "C:\Users\Red\source\repos\FleetClient\FleetClient\Carrier\CarrierHome.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "875AC179CEE9CE207AB4E049F09DFF50"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FleetClient
{
    partial class CarrierHome : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Carrier\CarrierHome.xaml line 12
                {
                    this.rootPivot = (global::Windows.UI.Xaml.Controls.Pivot)(target);
                    ((global::Windows.UI.Xaml.Controls.Pivot)this.rootPivot).SelectionChanged += this.rootPivot_SelectionChanged;
                }
                break;
            case 3: // Carrier\CarrierHome.xaml line 26
                {
                    global::Windows.UI.Xaml.Controls.Button element3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element3).Click += this.Send_Click;
                }
                break;
            case 4: // Carrier\CarrierHome.xaml line 27
                {
                    this.newtruck = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.newtruck).Click += this.newtruck_Click;
                }
                break;
            case 5: // Carrier\CarrierHome.xaml line 28
                {
                    this.newtrailer = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.newtrailer).Click += this.newtrailer_Click;
                }
                break;
            case 6: // Carrier\CarrierHome.xaml line 17
                {
                    this.assignedshipmentordergridcontrol = (global::FleetClient.AssignShipmentOrderGrid)(target);
                }
                break;
            case 7: // Carrier\CarrierHome.xaml line 14
                {
                    this.carriershipmentordergridcontrol = (global::FleetClient.CarrierShipmentOrderGrid)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
