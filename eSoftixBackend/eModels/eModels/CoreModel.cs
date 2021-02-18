﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eModels
{
    public  class CoreModel :KeyModel
    {
        [MaxLength(100)]
        public string created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;

        public bool is_deleted { get; set; } = false;

        [MaxLength(100)]
        public string deleted_by { get; set; }
        public DateTime? deleted_date { get; set; }  

        public bool status { get; set; } = true;

        [NotMapped]
        [JsonIgnore]
        public bool is_change_status { get; set; } = false;

        [NotMapped]
        [JsonIgnore]
        public bool is_editing { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_deleting { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_restoring { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_saving { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_selected { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        public bool is_loading { get; set; } = false;

    }
}