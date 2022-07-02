using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace Tomoor.Droid.Views
{
    internal class DatePickerDialogFragment : DialogFragment
    {
        //https://benjaminhysell.com/archive/2014/04/mvvmcross-xamarin-android-popup-datepicker-on-edittext-click/
        private readonly Context _context;
        private DateTime _date;
        private readonly DatePickerDialog.IOnDateSetListener _listener;

        public DatePickerDialogFragment(Context context, DateTime date, DatePickerDialog.IOnDateSetListener listener)
        {
            _context = context;
            _date = date;
            _listener = listener;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = new DatePickerDialog(_context, _listener, _date.Year, _date.Month - 1, _date.Day);
            return dialog;
        }

    }
}