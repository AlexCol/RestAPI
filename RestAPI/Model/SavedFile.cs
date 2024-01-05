using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Model;

[Table("savedfiles")]
public class SavedFile : BaseEntity
{
    public string FileName { get; set; }
    public byte[] FileData { get; set; }
    public string CreatedBy { get; set; }
}
