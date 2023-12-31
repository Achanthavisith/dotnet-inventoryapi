﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace dotnet_inventoryapi.Models
{
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [StringLength(0, ErrorMessage = "Id is Auto Generated")]
        [SwaggerSchema(ReadOnly = true)]
        [BsonElement("id")]
        public string? Id { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("usage")]
        public int Usage { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [BsonElement("__v")]
        public int Version { get; set; }
    }
}
