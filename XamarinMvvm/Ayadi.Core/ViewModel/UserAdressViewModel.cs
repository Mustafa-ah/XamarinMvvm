using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using Ayadi.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
using Ayadi.Core.Model;
using System.Collections.ObjectModel;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Messages;

namespace Ayadi.Core.ViewModel
{
    public class UserAdressViewModel : BaseViewModel, IUserAdressViewModel
    {

        // add new adress or update one 

        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private User _AppUser;

        private bool _isWorking;
        //private User _currentUser;

        //public User CurrentUser
        //{
        //    get { return _currentUser; }
        //    set { _currentUser = value; RaisePropertyChanged(() => CurrentUser); }
        //}


        private UserAdress _currentAdress;

        public UserAdress CurrentAdress
        {
            get { return _currentAdress; }
            set { _currentAdress = value; RaisePropertyChanged(() => CurrentAdress); }
        }


        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        //private string _cityName;

        //public string CityName
        //{
        //    get { return _cityName; }
        //    set { _cityName = value;RaisePropertyChanged(() => CityName ); }
        //}


        public UserAdressViewModel(IMvxMessenger messenger,
          IUserDataService userDataService,
           IConnectionService connectionService,
           IDialogService dialogService):base(messenger)
        {
            _userDataService = userDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;

            string fwfwf = _userDataService.AccessToken;
        }

        private int _adressId = -1;
        public void Init(int AdressId)
        {
            _adressId = AdressId == 0 ? -1 : AdressId;
        }

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            if (_connectionService.CheckOnline())
            {
                IsBusy = true;
                _AppUser = await _userDataService.GetSavedUser();

               // CurrentUser = await _userDataService.GetUserById(_AppUser.Id);//UserId

                if (_adressId == -1)
                {
                    CurrentAdress = new UserAdress();

                    _currentAdress.First_name = _AppUser.First_name;
                    _currentAdress.Last_name = _AppUser.Last_name;
                    _currentAdress.Email = _AppUser.Email;
                    _currentAdress.Phone_number = _AppUser.Phone;
                }
                else
                {
                    CurrentAdress = _AppUser.Billing_address;
                }

                //CityName = CurrentAdress.City;

                SelectedCity = new City(CurrentAdress.City);

                City gen = new City(TextSource.GetText("city_"));
                gen.IsSelected = true;
                gen.CityId = -1;
                City genM = new City("city 1");
                City genF = new City("city 2");

                Cities = new ObservableCollection<City>();
                Cities.Add(gen);
                Cities.Add(genM);
                Cities.Add(genF);

                SetCountries();

                IsBusy = false;
            }
            else
            {
                await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                    TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                // maybe we can navigate to a start page here, for you to add to this code base!
            }
        }

        //
        private ObservableCollection<Country> _countries;

        public ObservableCollection<Country> Countries
        {
            get { return _countries; }
            set { _countries = value; RaisePropertyChanged(() => Countries); }
        }


        //
        private Country _selectedCountry;

        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set { _selectedCountry = value; RaisePropertyChanged(() => SelectedCountry); }
        }


        private City _selectedCity;

        public City SelectedCity
        {
            get { return _selectedCity; }
            set { _selectedCity = value; RaisePropertyChanged(() => SelectedCity); }
        }

        //
        private ObservableCollection<City> _cities;

        public ObservableCollection<City> Cities
        {
            get { return _cities; }
            set { _cities = value; RaisePropertyChanged(() => Cities); }
        }

        private void SetCountries()
        {
            try
            {
                Countries = new ObservableCollection<Country>();
                Country country = new Country(TextSource.GetText("country_"));
                country.Id = -1;
                country.IsSelected = true;
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


        public MvxCommand SaveAdressCommand
        { get { return new MvxCommand(() => SaveAdress()); } }

        public MvxCommand<Country> CountrySelectedCommand
        {
            get
            {
                return new MvxCommand<Country>((ge) => {
                    foreach (var item in Cities)
                    {
                        item.IsSelected = false;
                    }
                    SelectedCountry = ge;
                    ge.IsSelected = true;
                });
            }
        }

        public MvxCommand<City> CitySelectedCommand
        {
            get
            {
                return new MvxCommand<City>((co) => {
                    foreach (var item in Cities)
                    {
                        item.IsSelected = false;
                    }
                    SelectedCity = co;
                    co.IsSelected = true;
                });
            }
        }

        private async void SaveAdress()
        {
            try
            {
                if (_isWorking)
                {
                    return;
                }

                if (!_connectionService.CheckOnline())
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                //else if (SelectedCountry.Id == -1)
                //{
                //    await _dialogService.ShowAlertAsync("select country", "Ayadi says...", "OK");
                //    return;
                //}
                
                else if (string.IsNullOrEmpty(CurrentAdress.First_name))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterFistNameMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (string.IsNullOrEmpty(CurrentAdress.Last_name))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterLastNameMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (string.IsNullOrEmpty(CurrentAdress.Email))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterEmailMsg_"),
                    TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (!Utility.HelperTools.ValidateEmail(CurrentAdress.Email))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterValidEmailMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
                else if(string.IsNullOrEmpty(CurrentAdress.City))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("selectCityMsg_"),
                    TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
                else if (IsBusy)
                {
                    return;
                }
               
                _isWorking = true;
                IsBusy = true;
                // add new adress
                if (_adressId == -1)
                {
                   // CurrentAdress.City = CityName;
                    CurrentAdress.Country = SelectedCountry.Name;
                    CurrentAdress.Country_id = SelectedCountry.Id;
                    CurrentAdress.Zip_postal_code = "0000";
                   // CurrentAdress.Created_on_utc = DateTime.Now.ToString();
                    IsBusy = true;
                    UserAdress AddedAdress = await _userDataService.AddUserAddress(_AppUser, CurrentAdress);
                    if (AddedAdress.Id != 0)
                    {
                        CurrentAdress.Id = AddedAdress.Id;
                        _dialogService.ShowToast(TextSource.GetText("adressAdded"));
                        //Close(this);
                    }
                    else
                    {
                        IsBusy = false;
                        await _dialogService.ShowAlertAsync(TextSource.GetText("adressNotAdded"), TextSource.GetText("tomoor_"),
                            TextSource.GetText("ok_"));
                        _isWorking = false;
                        return;
                    }
                   
                }
                else
                {
                    // update address
                    IsBusy = true;
                    bool IsAdded = await _userDataService.UpdateUserAddress(_AppUser, CurrentAdress);
                    if (IsAdded)
                    {
                        _dialogService.ShowToast(TextSource.GetText("adressupdated"));
                        //Close(this);
                    }
                    else
                    {
                        IsBusy = false;
                        await _dialogService.ShowAlertAsync(TextSource.GetText("adressNotupdated"), TextSource.GetText("tomoor_"),
                            TextSource.GetText("ok_"));
                        _isWorking = false;
                        return;
                    }
                }
                IsBusy = false;
                Messenger.Publish(new AdresessUpdatedMessage(this, CurrentAdress));
               // _isWorking = false;
                Close(this);
            }
            catch (Exception)
            {
                _isWorking = false;
                //throw;//x
            }
        }
    }
}
