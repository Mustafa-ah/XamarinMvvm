using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonText);
        Task<bool> ShowDecisionAlertAsync(string message, string title, string PositivebuttonText, string NigativebuttonText);
        event EventHandler AlertCliked;

        void ShowToast(string text);
    }
}
