﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeGET.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GeGET.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # CHANGELOG TODAS AS ALTERAÇÕES SERÃO DOCUMENTADAS NESTE ARQUIVO.
        ///## [2.0.0.18] - 25-07-2019 - ATUALIZAÇÃO DE EMERGÊNCIA
        ///### CORRIGIDO:
        ///- CORRIGIDO ERRO QUE ACONTECIA QUANDO O NÚMERO DE OPÇÕES ULTRAPASSAVA O LIMITE DA TELA DE COPIAR ITENS DE ORÇAMENTO;
        ///- CORRIGIDO LISTAGEM POR SEQUENCIA NA TELA DE ORÇAMENTOS.
        ///## [2.0.0.17] - 25-07-2019 - ATUALIZAÇÃO DE ROTINA
        ///### ADICIONADO:
        ///- ADICIONADO TABS NO FUNCIONAMENTO DAS PÁGINAS PARA MELHOR NAVEGAÇÃO;
        ///- ADICIONADO TELA DE EXPORTAR GRUPO DE ITENS;
        ///- ADICION [rest of string was truncated]&quot;;.
        /// </summary>
        public static string changelog {
            get {
                return ResourceManager.GetString("changelog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;App&gt;
        ///  &lt;Mirror&gt;\\SERVIDORAD\Storage\version.xml&lt;/Mirror&gt;
        ///  &lt;!--&lt;Mirror&gt;\\130.1.1.101\teste\version.xml&lt;/Mirror&gt;--&gt;
        ///&lt;/App&gt;.
        /// </summary>
        public static string updateurl {
            get {
                return ResourceManager.GetString("updateurl", resourceCulture);
            }
        }
    }
}
