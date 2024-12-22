using Microsoft.AspNetCore.Components;
using MudBlazor;
using ParsLinks.Admin.Components;
using Color = MudBlazor.Color;

namespace ParsLinks.Admin.Services
{


    public class DialogService : ParsLinks.Shared.Services.IDialogService
    {


        private readonly MudBlazor.IDialogService _dialog;


        //TODO : encapsulate Service
        public DialogService(MudBlazor.IDialogService dialog)
        {
            _dialog = dialog;
        }
        public IDialogReference ShowDialog<T>(DialogOptions options, string title = "") where T : ComponentBase
        {
            return _dialog.Show<T>(title, options);
        }

        public IDialogReference ShowDialog<T>(DialogOptions options) where T : ComponentBase
        {


            return _dialog.Show<T>("", options);

        }

        public IDialogReference ShowDialog<T>() where T : ComponentBase
        {


            return _dialog.Show<T>();

        }
        public IDialogReference ShowDialog<T>(Dictionary<string, object> parameters, DialogOptions options, string title = "") where T : ComponentBase
        {

            var param = new DialogParameters();
            foreach (var item in parameters)
            {
                param.Add(item.Key, item.Value);
            }

            return _dialog.Show<T>(title, param, options);

        }
        public IDialogReference ShowDialog<T>(Dictionary<string, object> parameters, string title = "") where T : ComponentBase
        {

            var param = new DialogParameters();
            foreach (var item in parameters)
            {
                param.Add(item.Key, item.Value);
            }

            return _dialog.Show<T>(title, param);

        }

        public async Task<DialogResult?> ShowMessageBoxAsync(string body,
            string title = "",
            string btnCancel = "Cancel",
            string titleIcon = "",
            string btnOk = "OK",
            string btnColor = "Primary",
            string iconColor = "Inherit",
            string btnIcon = "")
        {


            var buttonColor = Color.Primary;
            var glyphsColor = Color.Inherit;

            if (btnColor.Equals(nameof(Color.Primary), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Primary;

            if (btnColor.Equals(nameof(Color.Inherit), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Inherit;

            if (btnColor.Equals(nameof(Color.Default), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Inherit;

            if (btnColor.Equals(nameof(Color.Warning), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Warning;

            if (btnColor.Equals(nameof(Color.Dark), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Dark;

            if (btnColor.Equals(nameof(Color.Success), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Success;

            if (btnColor.Equals(nameof(Color.Info), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Info;

            if (btnColor.Equals(nameof(Color.Transparent), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Transparent;

            if (btnColor.Equals(nameof(Color.Error), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Error;

            if (btnColor.Equals(nameof(Color.Secondary), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Secondary;

            if (btnColor.Equals(nameof(Color.Tertiary), StringComparison.OrdinalIgnoreCase))
                buttonColor = Color.Tertiary;






            if (iconColor.Equals(nameof(Color.Primary), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Primary;

            if (iconColor.Equals(nameof(Color.Inherit), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Inherit;

            if (iconColor.Equals(nameof(Color.Default), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Inherit;

            if (iconColor.Equals(nameof(Color.Warning), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Warning;

            if (iconColor.Equals(nameof(Color.Dark), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Dark;

            if (iconColor.Equals(nameof(Color.Success), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Success;

            if (iconColor.Equals(nameof(Color.Info), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Info;

            if (iconColor.Equals(nameof(Color.Transparent), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Transparent;

            if (iconColor.Equals(nameof(Color.Error), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Error;

            if (iconColor.Equals(nameof(Color.Secondary), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Secondary;

            if (iconColor.Equals(nameof(Color.Tertiary), StringComparison.OrdinalIgnoreCase))
                glyphsColor = Color.Tertiary;







            var param = new DialogParameters()
            {
                {nameof(MessageDialog.Title), title },
                { nameof(MessageDialog.ButtonText), btnOk },
                { nameof(MessageDialog.ContentText), body },
                { nameof(MessageDialog.ButtonColor), buttonColor },
                { nameof(MessageDialog.BtnCancel), btnCancel },
                { nameof(MessageDialog.ButtonIcon), btnIcon },
                { nameof(MessageDialog.TitleIcon), titleIcon },
                { nameof(MessageDialog.IconColor), glyphsColor },
            };

            var options = new DialogOptions()
            {
                CloseButton = true,
                CloseOnEscapeKey = false,
                MaxWidth = MaxWidth.ExtraSmall
            };
            var dg = await _dialog.ShowAsync<MessageDialog>("", param, options);
            var result = await dg.Result;
            return result;

        }

        public async Task<bool> ShowConfirmationDialogAsync(string body, string title = "", string btnOk = "Submit")
        {


            var color = Color.Primary;



            var param = new DialogParameters()
            {
                {nameof(MessageDialog.Title), title },
                { nameof(MessageDialog.ButtonText), btnOk },
                { nameof(MessageDialog.ContentText), body },
                { nameof(MessageDialog.ButtonColor), color },
                { nameof(MessageDialog.BtnCancel), "Cancel" }
            };

            var options = new DialogOptions()
            {
                CloseButton = true,
                CloseOnEscapeKey = false,
                MaxWidth = MaxWidth.ExtraSmall
            };
            var dg = _dialog.Show<MessageDialog>("", param, options);
            var result = await dg.Result;
            return result?.Canceled == false;

        }

        public async Task<bool> ShowDeleteMessageBoxAsync(string body, string title = "Delete", string btnOk = "Delete")
        {


            var color = Color.Error;



            var param = new DialogParameters()
            {
                {nameof(MessageDialog.Title), title },
                { nameof(MessageDialog.ButtonText), btnOk },
                { nameof(MessageDialog.ContentText), body },
                { nameof(MessageDialog.ButtonColor), color },
                { nameof(MessageDialog.BtnCancel), "Cancel" },
                { nameof(MessageDialog.ButtonIcon), Icons.Material.Rounded.Delete },
                { nameof(MessageDialog.TitleIcon), Icons.Material.Rounded.Delete },
            };

            var options = new DialogOptions()
            {
                CloseButton = true,
                CloseOnEscapeKey = false,
                MaxWidth = MaxWidth.ExtraSmall
            };
            var dg = _dialog.Show<MessageDialog>("", param, options);
            var result = await dg.Result;
            return !result.Canceled;

        }
        public async Task<DialogResult?> ShowSuccessMessageBoxAsync(string body, string title = "Success")
        {


            var color = Color.Primary;



            var param = new DialogParameters()
            {
                {nameof(MessageDialog.Title), title },
                { nameof(MessageDialog.ButtonText), "OK" },
                { nameof(MessageDialog.ContentText), body },
                { nameof(MessageDialog.ButtonColor), Color.Primary },
                { nameof(MessageDialog.BtnCancel), "Cancel" },
                { nameof(MessageDialog.TitleIcon), @Icons.Material.Filled.Check },
                { nameof(MessageDialog.IconColor), Color.Success },
            };

            var options = new DialogOptions()
            {
                CloseButton = true,
                CloseOnEscapeKey = false,
                MaxWidth = MaxWidth.ExtraSmall
            };
            var dg = _dialog.Show<MessageDialog>("", param, options);
            var result = await dg.Result;
            return result;

        }


    }

}
