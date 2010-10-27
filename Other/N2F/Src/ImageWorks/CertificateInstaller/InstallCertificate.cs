using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.IO;


namespace CertificateInstaller
{
    [RunInstaller(true)]
    public partial class InstallCertificate : Installer
    {
        public InstallCertificate()
        {
            InitializeComponent();
        }

        public override void Commit(IDictionary savedState)
        {   
            base.Commit(savedState);

            String TargetDirectory = Path.GetDirectoryName(Context.Parameters["AssemblyPath"]);
            InstallCert(TargetDirectory);
        }

        private void WriteLog(string strInstallLocation, string text)
        {
            StreamWriter wr = new StreamWriter(File.Open(strInstallLocation + "\\n2f.log", FileMode.OpenOrCreate, FileAccess.Write));
            wr.WriteLine(text);
            wr.Close();
        }

        private void InstallCert(string strInstallLocation)
        {
            try
            {
                WriteLog(strInstallLocation,"Starting installation");
                FileStream fs = File.Open(strInstallLocation + "\\N2F_CS_DER.cer", FileMode.Open, FileAccess.Read);

                byte[] b = new byte[fs.Length];

                fs.Read(b, 0, b.Length);

                WriteLog(strInstallLocation,"Installing root certificate");
                X509Certificate2 cert = new X509Certificate2(b);
                X509Store store = new X509Store(StoreName.AuthRoot, StoreLocation.LocalMachine);

                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);
                store.Close();

                WriteLog(strInstallLocation, "Installing root certificate successfull");

                WriteLog(strInstallLocation, "Installing trusted publisher certificate");
                store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);
                store.Close();

                WriteLog(strInstallLocation, "Installing trusted publisher certificate successfull");

            }

            catch (System.Exception ex)
            {
                WriteLog(strInstallLocation, "Error Installing Certificate(s)" + ex.ToString());
            }
        }
    }
}
