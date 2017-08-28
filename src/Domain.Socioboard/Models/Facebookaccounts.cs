﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Socioboard.Models
{
    public class Facebookaccounts
    {
        public Facebookaccounts()
        {
            IsAccessTokenActive = true;
        }
        public virtual Int64 Id { get; set; }
        public virtual string FbUserId { get; set; }
        public virtual string FbUserName { get; set; }
        public virtual string AccessToken { get; set; }
        public virtual Int64 Friends { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string coverPic { get; set; }
        public virtual string birthday { get; set; }
        public virtual string education { get; set; }
        public virtual string college { get; set; }
        public virtual string workPosition { get; set; }
        public virtual string homeTown { get; set; }
        public virtual string gender { get; set; }
        public virtual string bio { get; set; }
        public virtual string about { get; set; }
        public virtual string workCompany { get; set; }
        public virtual Domain.Socioboard.Enum.FbProfileType FbProfileType { get; set; }
        public virtual Domain.Socioboard.Enum.FbPageSubscription FbPageSubscription { get; set; }
        public virtual string ProfileUrl { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Int64 UserId { get; set; }

        public virtual bool IsAccessTokenActive { get; set; }
        public virtual DateTime LastUpdate { get; set;}
        public virtual DateTime SchedulerUpdate { get; set; }
        public virtual DateTime PageShareathonUpdate { get; set; }
        public virtual DateTime GroupShareathonUpdate { get; set; }
        public virtual DateTime lastpagereportgenerated { get; set; }
        public virtual bool Is90DayDataUpdated { get; set; }
        public virtual DateTime contenetShareathonUpdate { get; set; }
        
    }
}
