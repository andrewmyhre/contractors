﻿using System;
using System.Collections.Generic;

namespace Contractors.Core.Domain
{
    public class Candidate
    {
        public object Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string EmailMd5Hash { get; set; }
        public string LookingFor { get; set; }
        public string Avoiding { get; set; }
        public decimal DesiredRate { get; set; }
        public RemunerationPeriod DesiredRatePeriod { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Placement> WorkHistory { get; set; }
        public string JobTitle { get; set; }
        public int ContractLengthInMonths { get; set; }
    }
}