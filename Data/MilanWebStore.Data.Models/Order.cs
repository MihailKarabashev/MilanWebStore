﻿using MilanWebStore.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MilanWebStore.Data.Models
{
    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.OrderProducts = new HashSet<OrderProduct>();
        }

        public OrderStatus OrderStatus { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public decimal TotalPrice { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int? AddressId { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
