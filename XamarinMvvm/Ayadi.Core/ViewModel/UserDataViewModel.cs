using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Extensions;
using Ayadi.Core.Contracts.Services;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;

namespace Ayadi.Core.ViewModel
{
    public class UserDataViewModel : BaseViewModel, IUserDataViewModel
    {

        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        public UserDataViewModel(IMvxMessenger messenger,
        IUserDataService userDataService,
         IConnectionService connectionService,
         IDialogService dialogService):base(messenger)
        {
            _userDataService = userDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;


        }

        private User _AppUser;

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; RaisePropertyChanged(() => CurrentUser); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private Gender _selectedGender;

        public Gender SelectedGender
        {
            get { return _selectedGender; }
            set { _selectedGender = value; RaisePropertyChanged(() => SelectedGender); }
        }

        //
        private ObservableCollection<Gender> _genders;

        public ObservableCollection<Gender> Genders
        {
            get { return _genders; }
            set { _genders = value; RaisePropertyChanged(() => Genders); }
        }

        private string _dateOfBirth;

        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value;  RaisePropertyChanged(() => DateOfBirth); }
        }


        private Country _selectedCountry;

        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set { _selectedCountry = value; RaisePropertyChanged(() => SelectedCountry); }
        }

        //
        private ObservableCollection<Country> _countries;

        public ObservableCollection<Country> Countries
        {
            get { return _countries; }
            set { _countries = value; RaisePropertyChanged(() => Countries); }
        }


        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {

            _AppUser = await _userDataService.GetSavedUser();
            if (_connectionService.CheckOnline())
            {
                IsBusy = true;
                CurrentUser = await _userDataService.GetUserById(_AppUser);
                CurrentUser.Id = _AppUser.Id;
                //  DateOfBirth = $"{CurrentUser.DateOfBirthDay}-{CurrentUser.DateOfBirthMonth}-{CurrentUser.DateOfBirthYear}";
                IsBusy = false;
                DateTime _dateOfBirth = new DateTime((int)CurrentUser.DateOfBirthYear,
                    (int)CurrentUser.DateOfBirthMonth, (int)CurrentUser.DateOfBirthDay, 1, 1, 1, 1);

                DateOfBirth = _dateOfBirth.ToString("MMMM dd, yyyy");
                //CurrentUser.PhoneNumber = "57455545";
                // IsBusy = false;

                SetGenderSpinner();
            }
            else
            {
                await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                  TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                // maybe we can navigate to a start page here, for you to add to this code base!
            }

            SetCountries();
           

        }
        private void SetCountries()
        {
            try
            {
                Countries = new ObservableCollection<Country>();
                Country country = new Country("Country"); country.IsSelected = true;

                Countries.Add(country);

                #region countries list ...........
                List<string> countriesList = new List<string>()
                {
                    "United States",
                    "Canada",
                    "Argentina",
                    "Armenia",
                    "Aruba",
                    "Australia",
                    "Austria",
                    "Azerbaijan",
                    "Bahamas",
                    "Bangladesh",
                    "Belarus",
                    "Belgium",
                    "Belize",
                    "Bermuda",
                    "Bolivia",
                    "Bosnia and Herzegowina",
                    "Brazil",
                    "Bulgaria",
                    "Cayman Islands",
                    "Chile",
                    "China",
                    "Colombia",
                    "Costa Rica",
                    "Croatia",
                    "Cuba",
                    "Cyprus",
                    "Czech Republic",
                    "Denmark",
                    "Dominican Republic",
                    "East Timor",
                    "Ecuador",
                    "Egypt",
                    "Finland",
                    "France",
                    "Georgia",
                    "Germany",
                    "Gibraltar",
                    "Greece",
                    "Guatemala",
                    "Hong Kong",
                    "Hungary",
                    "India",
                    "Indonesia",
                    "Ireland",
                    "Israel",
                    "Italy",
                    "Jamaica",
                    "Japan",
                    "Jordan",
                    "Kazakhstan",
                    "Korea, Democratic People's Republic of",
                    "Kuwait",
                    "Malaysia",
                    "Mexico",
                    "Netherlands",
                    "New Zealand",
                    "Norway",
                    "Pakistan",
                    "Palestine",
                    "Paraguay",
                    "Peru",
                    "Philippines",
                    "Poland",
                    "Portugal",
                    "Puerto Rico",
                    "Qatar",
                    "Romania",
                    "Russian Federation",
                    "Saudi Arabia",
                    "Singapore",
                    "Slovakia (Slovak Republic)",
                    "Slovenia",
                    "South Africa",
                    "Spain",
                    "Sweden",
                    "Switzerland",
                    "Taiwan",
                    "Thailand",
                    "Turkey",
                    "Ukraine",
                    "United Arab Emirates",
                    "United Kingdom",
                    "United States minor outlying islands",
                    "Uruguay",
                    "Uzbekistan",
                    "Venezuela",
                    "Serbia",
                    "Afghanistan",
                    "Albania",
                    "Algeria",
                    "American Samoa",
                    "Andorra",
                    "Angola",
                    "Anguilla",
                    "Antarctica",
                    "Antigua and Barbuda",
                    "Bahrain",
                    "Barbados",
                    "Benin",
                    "Bhutan",
                    "Botswana",
                    "Bouvet Island",
                    "British Indian Ocean Territory",
                    "Brunei Darussalam",
                    "Burkina Faso",
                    "Burundi",
                    "Cambodia",
                    "Cameroon",
                    "Cape Verde",
                    "Central African Republic",
                    "Chad",
                    "Christmas Island",
                    "Cocos (Keeling) Islands",
                    "Comoros",
                    "Congo",
                    "Congo (Democratic Republic of the)",
                    "Cook Islands",
                    "Cote D'Ivoire",
                    "Djibouti",
                    "Dominica",
                    "El Salvador",
                    "Equatorial Guinea",
                    "Eritrea",
                    "Estonia",
                    "Ethiopia",
                    "Falkland Islands (Malvinas)",
                    "Faroe Islands",
                    "Fiji",
                    "French Guiana",
                    "French Polynesia",
                    "French Southern Territories",
                    "Gabon",
                    "Gambia",
                    "Ghana",
                    "Greenland",
                    "Grenada",
                    "Guadeloupe",
                    "Guam",
                    "Guinea",
                    "Guinea-bissau",
                    "Guyana",
                    "Haiti",
                    "Heard and Mc Donald Islands",
                    "Honduras",
                    "Iceland",
                    "Iran (Islamic Republic of)",
                    "Iraq",
                    "Kenya",
                    "Kiribati",
                    "Korea",
                    "Kyrgyzstan",
                    "Lao People's Democratic Republic",
                    "Latvia",
                    "Lebanon",
                    "Lesotho",
                    "Liberia",
                    "Libyan Arab Jamahiriya",
                    "Liechtenstein",
                    "Lithuania",
                    "Luxembourg",
                    "Macau",
                    "Macedonia",
                    "Madagascar",
                    "Malawi",
                    "Maldives",
                    "Mali",
                    "Malta",
                    "Marshall Islands",
                    "Martinique",
                    "Mauritania",
                    "Mauritius",
                    "Mayotte",
                    "Micronesia",
                    "Moldova",
                    "Monaco",
                    "Mongolia",
                    "Montenegro",
                    "Montserrat",
                    "Morocco",
                    "Mozambique",
                    "Myanmar",
                    "Namibia",
                    "Nauru",
                    "Nepal",
                    "Netherlands Antilles",
                    "New Caledonia",
                    "Nicaragua",
                    "Niger",
                    "Nigeria",
                    "Niue",
                    "Norfolk Island",
                    "Northern Mariana Islands",
                    "Oman",
                    "Palau",
                    "Panama",
                    "Papua New Guinea",
                    "Pitcairn",
                    "Reunion",
                    "Rwanda",
                    "Saint Kitts and Nevis",
                    "Saint Lucia",
                    "Saint Vincent and the Grenadines",
                    "Samoa",
                    "San Marino",
                    "Sao Tome and Principe",
                    "Senegal",
                    "Seychelles",
                    "Sierra Leone",
                    "Solomon Islands",
                    "Somalia",
                    "South Georgia & South Sandwich Islands",
                    "South Sudan",
                    "Sri Lanka",
                    "St. Helena",
                    "St. Pierre and Miquelon",
                    "Sudan",
                    "Suriname",
                    "Svalbard and Jan Mayen Islands",
                    "Swaziland",
                    "Syrian Arab Republic",
                    "Tajikistan",
                    "Tanzania",
                    "Togo",
                    "Tokelau",
                    "Tonga",
                    "Trinidad and Tobago",
                    "Tunisia",
                    "Turkmenistan",
                    "Turks and Caicos Islands",
                    "Tuvalu",
                    "Uganda",
                    "Vanuatu",
                    "Vatican City State (Holy See)",
                    "Viet Nam",
                    "Virgin Islands (British)",
                    "Virgin Islands (U.S.)",
                    "Wallis and Futuna Islands",
                    "Western Sahara",
                    "Yemen",
                    "Zambia",
                    "Zimbabwe"
                };
                #endregion

                foreach (string country_ in countriesList)
                {
                    Countries.Add(new Country(country_));
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        //commands

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }

        public MvxCommand SaveCommand
        { get { return new MvxCommand(() => SaveData()); } }

        public MvxCommand<Gender> GenderSelectedCommand
        {
            get
            {
                return new MvxCommand<Gender>((ge) => {
                    foreach (var item in Genders)
                    {
                        item.IsSelected = false;
                    }
                    SelectedGender = ge;
                    ge.IsSelected = true;
                });
            }
        }

        public MvxCommand<Country> CountrySelectedCommand
        {
            get
            {
                return new MvxCommand<Country>((co) => {
                    foreach (var item in Countries)
                    {
                        item.IsSelected = false;
                    }
                    SelectedCountry = co;
                    co.IsSelected = true;
                });
            }
        }

        private async void SaveData()
        {
            try
            {
                if (!_connectionService.CheckOnline())
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
                else if (string.IsNullOrEmpty(CurrentUser.First_name))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterFistNameMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (string.IsNullOrEmpty(CurrentUser.Last_name))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterLastNameMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (string.IsNullOrEmpty(CurrentUser.Email))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterEmailMsg_"),
                    TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (!Utility.HelperTools.ValidateEmail(CurrentUser.Email))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterValidEmailMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                //else if (SelectedGender.Name == TextSource.GetText("gender_"))
                //{
                //    await _dialogService.ShowAlertAsync(TextSource.GetText("enterGenderMsg_"),
                //     TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                //    return;
                //}


                DateTime _dateOfBirth;
                if (!DateTime.TryParse(DateOfBirth, out _dateOfBirth))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterBirthDayMsg_"),
                       TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
                else
                {
                    CurrentUser.DateOfBirthDay = _dateOfBirth.Day;
                    CurrentUser.DateOfBirthYear = _dateOfBirth.Year;
                    CurrentUser.DateOfBirthMonth = _dateOfBirth.Month;
                }
                CurrentUser.AcceptPrivacyPolicyEnabled = true;
                CurrentUser.Registered_in_store_id = 1;

                // CurrentUser.PhoneNumber = "01113145562";
                CurrentUser.Username = CurrentUser.Email;

                CurrentUser.Gender = SelectedGender.Id;

                CurrentUser.AccessToken = _AppUser.AccessToken;

                IsBusy = true;
               
                User _signedUser = await _userDataService.PutUser(CurrentUser);

                if (_signedUser.Id != 0)
                {
                    CurrentUser.Id = _signedUser.Id;
                    CurrentUser.IsGuestUser = false;
                    await _userDataService.SaveUserToLocal(CurrentUser);
                    _dialogService.ShowToast(TextSource.GetText("dataSavedMsg_"));
                    IsBusy = false;
                }
                else
                {
                    IsBusy = false;
                    _dialogService.ShowToast(TextSource.GetText("dataNotSavedMsg_"));
                    
                }
                // return _signedUser.Id;
            }
            catch (Exception)
            {
                IsBusy = false;
                //throw;//x
            }

        }

        private void SetGenderSpinner()
        {
            try
            {
                Genders = new ObservableCollection<Gender>();
                if (CurrentUser.Gender == "F")
                {
                    Gender Fgen = new Gender(TextSource.GetText("female_"),CurrentUser.Gender);
                    Fgen.IsSelected = true;
                    SelectedGender = Fgen;
                    Genders.Add(Fgen);

                    Gender Mgen = new Gender(TextSource.GetText("male_"),"M");
                    Genders.Add(Mgen);
                }
                else if (CurrentUser.Gender == "M")
                {
                   
                    Gender Mgen = new Gender(TextSource.GetText("male_"), CurrentUser.Gender);
                    Mgen.IsSelected = true;
                    SelectedGender = Mgen;
                    Genders.Add(Mgen);

                    Gender Fgen = new Gender(TextSource.GetText("female_"), "F");
                    Genders.Add(Fgen);
                }
                else
                {
                    Gender gen = new Gender(CurrentUser.Gender, CurrentUser.Gender);
                    gen.IsSelected = true;
                    SelectedGender = gen;
                    Genders.Add(gen);

                    Gender genF = new Gender(TextSource.GetText("female_"), "F");
                    Genders.Add(genF);

                    Gender genM = new Gender(TextSource.GetText("male_"), "M");
                    Genders.Add(genM);
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
    }
}
