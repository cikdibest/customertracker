﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomerTracker.ClientControllerService.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public double TimerInMinutes {
            get {
                return ((double)(this["TimerInMinutes"]));
            }
            set {
                this["TimerInMinutes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("01")]
        public string MachineCode {
            get {
                return ((string)(this["MachineCode"]));
            }
            set {
                this["MachineCode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:51243/ServerStatusListener/GetApplicationServices")]
        public string ApiAddressetGetApplicationServices {
            get {
                return ((string)(this["ApiAddressetGetApplicationServices"]));
            }
            set {
                this["ApiAddressetGetApplicationServices"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:51243/ServerStatusListener/PostServerCondition")]
        public string ApiAddressPostServerCondition {
            get {
                return ((string)(this["ApiAddressPostServerCondition"]));
            }
            set {
                this["ApiAddressPostServerCondition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public int CpuUsageAlarmLimit {
            get {
                return ((int)(this["CpuUsageAlarmLimit"]));
            }
            set {
                this["CpuUsageAlarmLimit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2048")]
        public int DiskUsageAlarmLimit {
            get {
                return ((int)(this["DiskUsageAlarmLimit"]));
            }
            set {
                this["DiskUsageAlarmLimit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1024")]
        public int RamUsageAlarmLimit {
            get {
                return ((int)(this["RamUsageAlarmLimit"]));
            }
            set {
                this["RamUsageAlarmLimit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("150")]
        public int ServiceThreadCountAlarmLimit {
            get {
                return ((int)(this["ServiceThreadCountAlarmLimit"]));
            }
            set {
                this["ServiceThreadCountAlarmLimit"] = value;
            }
        }
    }
}
