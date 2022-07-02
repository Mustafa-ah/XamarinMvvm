using System.Threading.Tasks;
using Android.App;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Ayadi.Core.Contracts.Services;
using System;
using System.Threading;
using Android.Content;
using Android.Widget;

namespace Tomoor.Droid.Services
{
    public class DialogService : IDialogService
    {
        protected Activity CurrentActivity =>
            Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public event EventHandler AlertCliked;

        public Task ShowAlertAsync(string message,
            string title, string buttonText)
        {
            return Task.Run(() =>
            {
                Alert(message, title, buttonText);
            });
        }

        public async Task<bool> ShowDecisionAlertAsync(string message, string title, string PositivebuttonText, string NigativebuttonText)
        {
             //return await DecisionAlert(message, title, PositivebuttonText, NigativebuttonText);
            return await Task.FromResult<bool>(DecisionAlert(message, title, PositivebuttonText, NigativebuttonText));
        }

        public void ShowToast(string text)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                Toast.MakeText(CurrentActivity, text, ToastLength.Short).Show();
            }, null);
        }

        private void Alert(string message, string title, string okButton)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                var builder = new AlertDialog.Builder(CurrentActivity);
                builder.SetIconAttribute
                    (Android.Resource.Attribute.AlertDialogIcon);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetPositiveButton(okButton, delegate { });
                builder.Create().Show();
            }, null);
        }

        private bool DecisionAlert(string message, string title, string PositivebuttonText, string NigativebuttonText)
        {
            bool result = false;

                    var builder = new AlertDialog.Builder(CurrentActivity);
                    builder.SetIconAttribute
                        (Android.Resource.Attribute.AlertDialogIcon);
                    builder.SetTitle(title);
                    builder.SetMessage(message);
                    builder.SetPositiveButton(PositivebuttonText, 
                        delegate { result = true; AlertCliked.Invoke(result, new EventArgs()); });
                    builder.SetNegativeButton(NigativebuttonText, 
                        delegate { result = false; AlertCliked.Invoke(result, new EventArgs()); });
            builder.Create().Show();

            return result;

        }


    }
}