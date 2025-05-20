using System;
using System.Collections.Generic;

namespace BlazorApp1.Data.Entities;

public partial class Sale
{
    public int SalesId { get; set; }

    public string SalesName { get; set; }

    public string SalesDescription { get; set; }

    public DateTime? SalesDate { get; set; }

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
