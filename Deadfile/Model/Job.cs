using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public sealed class Job : BindableBase
    {
        int id;
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        string address;
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged("Address");
                    OnPropertyChanged("FullAddress");
                }
            }
        }

        string town;
        public string Town
        {
            get
            {
                return town;
            }
            set
            {
                if (town != value)
                {
                    town = value;
                    OnPropertyChanged("Town");
                    OnPropertyChanged("FullAddress");
                }
            }
        }

        string postcode;
        public string Postcode
        {
            get
            {
                return postcode;
            }
            set
            {
                if (postcode != value)
                {
                    postcode = value;
                    OnPropertyChanged("Postcode");
                    OnPropertyChanged("FullAddress");
                }
            }
        }

        public string FullAddress
        {
            get
            {
                return string.Format("{0}, {1}, {2}", address ?? string.Empty, town ?? string.Empty, postcode ?? string.Empty);
            }
        }

        int clientId;
        public int ClientId
        {
            get
            {
                return clientId;
            }
            set
            {
                if (clientId != value)
                {
                    clientId = value;
                    OnPropertyChanged("ClientId");
                }
            }
        }

        JobStatus jobStatus;
        public JobStatus JobStatus
        {
            get
            {
                return jobStatus;
            }
            set
            {
                if (jobStatus != value)
                {
                    jobStatus = value;
                    OnPropertyChanged("JobStatus");
                }
            }
        }

    }
}
