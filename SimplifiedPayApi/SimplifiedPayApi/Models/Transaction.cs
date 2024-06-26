﻿using System.Text.Json.Serialization;

namespace SimplifiedPayApi.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Wallet? Payer { get; set; }
    public int PayerId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Wallet? Receiver { get; set; }
    public int ReceiverId { get; set; }
    public bool Status { get; set; }
    public DateTime Timestamp { get; set; }
}
