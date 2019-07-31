using Prism.Commands;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeGET.ViewModels
{
    class LayoutViewModel : BindableBase
    {
        private static IModuleManager _moduleManager;
        public DelegateCommand<string> NavigateCommand { get; set; }
        public LayoutViewModel(IModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
            _moduleManager.LoadModule("InfrastructureModule");
        }
    }
}
