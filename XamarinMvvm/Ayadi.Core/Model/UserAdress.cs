using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;

namespace Ayadi.Core.Model
{
    public class UserAdress : BaseModel
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public int? Country_id { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address { get; set; }
        public string Phone_number { get; set; }
        public string Zip_postal_code { get; set; }
        //public string Created_on_utc { get; set; }

        //[JsonIgnore]
        //public MvxCommand DeleteAdressCommand
        //{ get { return new MvxCommand(() => SendDeleteMessage()); } }

        //[JsonIgnore]
        //protected IMvxMessenger MessengerDelete;
        //[JsonIgnore]
        //protected IMvxMessenger MessengerUpdate;
        //[JsonIgnore]
        //private MvxCommand _deleteCommand;
        //[JsonIgnore]
        //public IMvxCommand DeleteAdressCommandeee
        //{
        //    get
        //    {
        //        _deleteCommand = _deleteCommand ?? new MvxCommand(() => SendDeleteMessage());
        //        return _deleteCommand;
        //    }
        //}
        //private void SendDeleteMessage()
        //{
        //    // this condition make sure that this method wont be excuted Twice 
        //    if (MessengerDelete == null)
        //    {
        //        MessengerDelete = Mvx.Resolve<IMvxMessenger>();
        //        MessengerDelete.Publish(new Messages.DeleteAdressMessage(this, this));
        //    }

        //}

        //[JsonIgnore]
        //public MvxCommand UpdateAdressCommand
        //{ get { return new MvxCommand(() => SendUpdateMessage()); } }

        //[JsonIgnore]
        //private MvxCommand _UpdateCommand;
        //[JsonIgnore]
        //public IMvxCommand UpdateAdressCommand
        //{
        //    get
        //    {
        //        _UpdateCommand = _UpdateCommand ?? new MvxCommand(() => SendUpdateMessage());
        //        return _UpdateCommand;
        //    }
        //}
        //private void SendUpdateMessage()
        //{
        //    // this condition make sure that this method wont be excuted Twice(or more) for this object
        //    if (MessengerUpdate == null)
        //    {
        //        MessengerUpdate = Mvx.Resolve<IMvxMessenger>();
        //        MessengerUpdate.Publish(new Messages.UpdateAdressMessage(this, this));
        //    }
           
        //}


        public override bool Equals(object obj)
        {
            if (obj is UserAdress)
            {
                UserAdress ComAdress = obj as UserAdress;
                return ComAdress.Id == this.Id;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
