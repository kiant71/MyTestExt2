﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace MyTestExt.ConsoleApp.cn.org.zhongdengwang.ws.InitRegisterService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSInitRegisterServiceServiceSoapBinding", Namespace="https://ws.zhongdengwang.org.cn/mfrs_ws/services/InitRegisterService")]
    public partial class WSInitRegisterServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback initRegisterOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSInitRegisterServiceService() {
            this.Url = global::MyTestExt.ConsoleApp.Properties.Settings.Default.MyTestExt_ConsoleApp_cn_org_zhongdengwang_ws_InitRegisterService_WSInitRegisterServiceService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event initRegisterCompletedEventHandler initRegisterCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="https://ws.zhongdengwang.org.cn/mfrs_ws/services/InitRegisterService", ResponseNamespace="https://ws.zhongdengwang.org.cn/mfrs_ws/services/InitRegisterService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary")]
        public byte[] initRegister([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary")] byte[] registerTypeBz, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary")] byte[] platformAuthCode, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary")] byte[] loginToken, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary")] byte[] xmlFileName, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary")] byte[] xmlFileContent, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary")] byte[] attachmentsZip) {
            object[] results = this.Invoke("initRegister", new object[] {
                        registerTypeBz,
                        platformAuthCode,
                        loginToken,
                        xmlFileName,
                        xmlFileContent,
                        attachmentsZip});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void initRegisterAsync(byte[] registerTypeBz, byte[] platformAuthCode, byte[] loginToken, byte[] xmlFileName, byte[] xmlFileContent, byte[] attachmentsZip) {
            this.initRegisterAsync(registerTypeBz, platformAuthCode, loginToken, xmlFileName, xmlFileContent, attachmentsZip, null);
        }
        
        /// <remarks/>
        public void initRegisterAsync(byte[] registerTypeBz, byte[] platformAuthCode, byte[] loginToken, byte[] xmlFileName, byte[] xmlFileContent, byte[] attachmentsZip, object userState) {
            if ((this.initRegisterOperationCompleted == null)) {
                this.initRegisterOperationCompleted = new System.Threading.SendOrPostCallback(this.OninitRegisterOperationCompleted);
            }
            this.InvokeAsync("initRegister", new object[] {
                        registerTypeBz,
                        platformAuthCode,
                        loginToken,
                        xmlFileName,
                        xmlFileContent,
                        attachmentsZip}, this.initRegisterOperationCompleted, userState);
        }
        
        private void OninitRegisterOperationCompleted(object arg) {
            if ((this.initRegisterCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.initRegisterCompleted(this, new initRegisterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void initRegisterCompletedEventHandler(object sender, initRegisterCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class initRegisterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal initRegisterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591