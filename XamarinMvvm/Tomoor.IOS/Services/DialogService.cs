using Ayadi.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Tomoor.IOS.Services
{
    public class DialogService : IDialogService
    {
        public event EventHandler AlertCliked;

        public Task ShowAlertAsync(string message, string title, string buttonText)
        {
            return Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIAlertView alert = new UIAlertView()
                {
                    Title = title,
                    Message = message
                };
                alert.AddButton(buttonText);
                alert.Show();
            }));
        }

        public Task<bool> ShowDecisionAlertAsync(string message, string title, string PositivebuttonText, string NigativebuttonText)
        {
            throw new NotImplementedException();
        }

        public void ShowToast(string text)
        {
            throw new NotImplementedException();
        }
    }
}
