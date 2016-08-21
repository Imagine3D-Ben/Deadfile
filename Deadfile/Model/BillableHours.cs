using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public sealed class BillableHours : BindableBase
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

        int jobId;
        public int JobId
        {
            get
            {
                return jobId;
            }
            set
            {
                if (jobId != value)
                {
                    jobId = value;
                    OnPropertyChanged("JobId");
                }
            }
        }

        int employeeId;
        public int EmployeeId
        {
            get
            {
                return employeeId;
            }
            set
            {
                if (employeeId != value)
                {
                    employeeId = value;
                    OnPropertyChanged("EmployeeId");
                }
            }
        }

        DateTime date;
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        int hours;
        public int Hours
        {
            get
            {
                return hours;
            }
            set
            {
                if (hours != value)
                {
                    hours = value;
                    OnPropertyChanged("Hours");
                }
            }
        }

        string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

    }
}
