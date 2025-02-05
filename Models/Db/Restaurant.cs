﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;

namespace Models.Db
{
    public class Restaurant
    {
        public long Id { get; set; }

        [ForeignKey(nameof(LocationLatLng))]
        public long LocationLatLngId { get; set; }

        public virtual LatLng LocationLatLng { get; set; }

        public string AddressString { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<CourierAccount> AssignedCouriers { get; set; }

        // Can't be null, it's always bound to zone
        [ForeignKey(nameof(Zone))]
        public long? ZoneId { get; set; }

        public virtual Zone Zone { get; set; }

        public bool UseAutoDeliveryServer { get; set; }
    }
}