using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ShoppingCartService.Models;

namespace ShoppingCartService.DataAccess.Entities
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public String CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public Address ShippingAddress { get; set; }
        public List<Item> Items { get; set; } = new();
    }
}