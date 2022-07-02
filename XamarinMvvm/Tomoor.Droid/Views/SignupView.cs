using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views;
using Ayadi.Core.ViewModel;
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "SignupView")]
    public class SignupView : MvxActivity<SignupViewModel>, DatePickerDialog.IOnDateSetListener
    {
        BindableProgressBar _bindableProgressBar;
        EditText datePickerText;

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            datePickerText.Text = new DateTime(year, month + 1, dayOfMonth).ToLongDateString();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_SignUp);

            this.Window.SetSoftInputMode(SoftInput.StateHidden);

            datePickerText = FindViewById<EditText>(Resource.Id.editTextBirthday);
            datePickerText.Focusable = false;
            datePickerText.Click += delegate
            {
                DateTime date;
                if (!DateTime.TryParse(datePickerText.Text, out date))
                {
                    date = DateTime.Now;
                }
                DatePickerDialogFragment dialog = new DatePickerDialogFragment(this, date, this);
                dialog.Show(FragmentManager, "date");
            };

            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<SignupView, SignupViewModel>();
            set.Bind(datePickerText).To(vm => vm.DateOfBirth);
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();
        }
    }
}