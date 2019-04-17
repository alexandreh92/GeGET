using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Management;
using System.Management.Instrumentation;
using System.Security.AccessControl;
using System.Security.Principal;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Permission.xaml
    /// </summary>
    public partial class Permission : Window
    {
        public Permission()
        {
            InitializeComponent();
            teste();
        }


        private void teste()
        {
            SelectQuery sQuery = new SelectQuery("Win32_Group", "Domain='" + System.Environment.UserDomainName.ToString() + "'");

            try
            {
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(sQuery);

                foreach (ManagementObject mObject in mSearcher.Get())
                {
                    cmb.Items.Add(mObject["Name"]);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }

        }

        private void SetAcess()
        {
            string teste = "D:\\teste";
            string Usuario = "Administradores";
            DirectoryInfo myDirectoryInfo = new DirectoryInfo(teste);
            DirectorySecurity myDirectorySecurity = myDirectoryInfo.GetAccessControl();
            string User = System.Environment.UserDomainName + "\\" + Usuario;
            myDirectorySecurity.AddAccessRule(new FileSystemAccessRule(Usuario, FileSystemRights.Modify, AccessControlType.Deny));
            myDirectoryInfo.SetAccessControl(myDirectorySecurity);
            MessageBox.Show("Permissions Altered Successfully");
            
        }

        private void RemoveAll()
        {
            string teste = "D:\\teste";
           // string Usuario = "Administradores";
            DirectoryInfo myDirectoryInfo = new DirectoryInfo(teste);
            DirectorySecurity myDirectorySecurity = myDirectoryInfo.GetAccessControl();
            AuthorizationRuleCollection rules = myDirectorySecurity.GetAccessRules(true, true, typeof(NTAccount));

            foreach (AuthorizationRule rule in rules)
            {
                if (rule is FileSystemAccessRule)
                {
                    myDirectorySecurity.RemoveAccessRule((FileSystemAccessRule)rule);
                }
            }
            myDirectorySecurity.SetAccessRuleProtection(true, false);
            myDirectoryInfo.SetAccessControl(myDirectorySecurity);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RemoveAll();
            //SetAcess();
            //System.Windows.Forms.FolderBrowserDialog myFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            //myFolderBrowserDialog.ShowDialog();
            //DirectoryInfo myDirectoryInfo = new DirectoryInfo(myFolderBrowserDialog.SelectedPath.ToString());
        }
    }
}
