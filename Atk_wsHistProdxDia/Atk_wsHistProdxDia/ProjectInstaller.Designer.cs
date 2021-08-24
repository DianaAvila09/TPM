namespace Atk_wsHistProdxDia
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
         this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
         this.Atk_wsHistProdxDia = new System.ServiceProcess.ServiceInstaller();
         // 
         // serviceProcessInstaller1
         // 
         this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
         this.serviceProcessInstaller1.Password = null;
         this.serviceProcessInstaller1.Username = null;
         // 
         // Atk_wsHistProdxDia
         // 
         this.Atk_wsHistProdxDia.Description = "Genera hist. de prod. por dia";
         this.Atk_wsHistProdxDia.DisplayName = "Atk_wsHistProdxDia";
         this.Atk_wsHistProdxDia.ServiceName = "Atk_wsHistProdxDia";
         this.Atk_wsHistProdxDia.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
         // 
         // ProjectInstaller
         // 
         this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.Atk_wsHistProdxDia});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller Atk_wsHistProdxDia;
    }
}