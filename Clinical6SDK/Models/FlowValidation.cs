using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clinical6SDK.Models
{
    public class FlowValidation
    {
        private DateTime _minDateTime;
        private string _minString;
        public string MinValue
        {
            get
            {
                return _minString;
            }
        }
        public void SetCurrentDateMin()
        {
            _minDateTime = DateTime.UtcNow;
            _minString = _minDateTime.ToString("O"); // verify if this is the correct date format that it is used
        }
        public int MinAdd_Days { get; set; }
        public int MinAdd_Months { get; set; }
        public int MinAdd_Years { get; set; }

        public int MinSubstrac_Days { get; set; }
        public int MinSuubtract_Months { get; set; }
        public int MinSubstrac_Years { get; set; }

        private DateTime _maxDateTime;
        private string _maxString;
        public string MaxValue
        {
            get
            {
                return _maxString;
            }
        }
        public void SetCurrentDateMax()
        {
            _maxDateTime = DateTime.UtcNow;
            _maxString = _maxDateTime.ToString("O"); // verify if this is the correct date format that it is used
        }

        public int MaxAdd_Days { get; set; }
        public int MaxAdd_Months { get; set; }
        public int MaxAdd_Years { get; set; }

        public int MaxSubstrac_Days { get; set; }
        public int MaxSuubtract_Months { get; set; }
        public int MaxSubstrac_Years { get; set; }

        /**
         * Not sure how to implement this here
         * 
         * validation_details: null,
         * 
         * validation_details: {
         *      min: "current"
         *  },
         * 
         *  validation_details: {
         *      max: "current",
         *      min: {
         *          value: "current",
         *          subtract: { year: 2 },
         *          add: null
         *      }
         *  },
         */
    }
}
