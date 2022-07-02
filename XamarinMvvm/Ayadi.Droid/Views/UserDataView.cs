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
using MvvmCross.Binding.BindingContext;

namespace Ayadi.Droid.Views
{
    [Activity(Label = "UserDataView")]
    public class UserDataView : MvxActivity<UserDataViewModel>, DatePickerDialog.IOnDateSetListener
    {
        EditText datePickerText;

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            datePickerText.Text = new DateTime(year, month + 1, dayOfMonth).ToLongDateString();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_userData);

            try
            {
                this.Window.SetSoftInputMode(SoftInput.StateHidden);
                datePickerText = FindViewById<EditText>(Resource.Id.editTextBirthday);
                datePickerText.Focusable = false;
                datePickerText.Click += delegate
                {
                    DateTime date;
                    if (!DateTime.TryParse(datePickerText.Text,out date))
                    {
                        date = DateTime.Now;
                    }
                    DatePickerDialogFragment dialog = new DatePickerDialogFragment(this, date, this);
                    dialog.Show(FragmentManager, "date");

                    //if (datePickerText.Text == null)
                    //{
                    //    DatePickerDialogFragment dialog = new DatePickerDialogFragment(this, DateTime.Now, this);
                    //    dialog.Show(FragmentManager, "date");
                    //}
                    //else
                    //{
                    //    DatePickerDialogFragment dialog = new DatePickerDialogFragment(this, Convert.ToDateTime(datePickerText.Text), this);
                    //    dialog.Show(FragmentManager, "date");
                    //}
                   
                };


                var set = this.CreateBindingSet<UserDataView, UserDataViewModel>();
                set.Bind(datePickerText).To(vm => vm.DateOfBirth);
                set.Apply();
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
    }
}