using System;
using System.Collections.Generic;

namespace BlazorApp1.Data.Entities;

public partial class Delivery
{
    public int DeliveryId { get; set; }

    public string DeliveryName { get; set; }

    public string DeliveryAddress { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public decimal BusinessValue { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedById { get; set; }

    public DateOnly? CreateDate { get; set; }

    public int? UpdatedById { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? DeletedById { get; set; }

    public DateOnly? DeleteDate { get; set; }

    public string ReceiptImage { get; set; }

    public virtual ICollection<Pullout> Pullouts { get; set; } = new List<Pullout>();
}
