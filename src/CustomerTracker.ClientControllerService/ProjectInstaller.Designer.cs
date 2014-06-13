namespace CustomerTracker.ClientControllerService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ClientControllerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.CustomerTrackerClientControllerInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ClientControllerProcessInstaller
            // 
            this.ClientControllerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.ClientControllerProcessInstaller.Password = null;
            this.ClientControllerProcessInstaller.Username = null;
            // 
            // CustomerTrackerClientControllerInstaller
            // 
            this.CustomerTrackerClientControllerInstaller.Description = "Ankaref Client Controller Service";
            this.CustomerTrackerClientControllerInstaller.DisplayName = "Ankaref Client Controller Service";
            this.CustomerTrackerClientControllerInstaller.ServiceName = "ClientControllerService";
            this.CustomerTrackerClientControllerInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ClientControllerProcessInstaller,
            this.CustomerTrackerClientControllerInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ClientControllerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller CustomerTrackerClientControllerInstaller;
    }
}