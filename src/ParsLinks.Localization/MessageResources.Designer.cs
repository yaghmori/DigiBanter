﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParsLinks.Localization {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class MessageResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MessageResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ParsLinks.Localization.MessageResources", typeof(MessageResources).Assembly);
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
        ///   Looks up a localized string similar to Are you sure you want to delete this attachment?.
        /// </summary>
        public static string AttachmentDeleteConfirmation {
            get {
                return ResourceManager.GetString("AttachmentDeleteConfirmation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file size exceeds the maximum limit {0}..
        /// </summary>
        public static string FileSizeExceedsMmaximumLimit {
            get {
                return ResourceManager.GetString("FileSizeExceedsMmaximumLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file has been successfully uploaded..
        /// </summary>
        public static string FileSuccessfullyUploaded {
            get {
                return ResourceManager.GetString("FileSuccessfullyUploaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file upload was intrupted.
        /// </summary>
        public static string FileUploadError {
            get {
                return ResourceManager.GetString("FileUploadError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file upload was successful.
        /// </summary>
        public static string FileUploadSucceed {
            get {
                return ResourceManager.GetString("FileUploadSucceed", resourceCulture);
            }
        }
    }
}