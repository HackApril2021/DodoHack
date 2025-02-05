﻿namespace Models.Dtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        
        public string Content { get; set; }

        public string AddressString { get; set; }
        
        public string AddressAdditional { get; set; }

        public string CreationDateTime { get; set; }

        public LatLngDto Destination { get; set; }
    }
}