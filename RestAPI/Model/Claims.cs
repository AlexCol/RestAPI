using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Model;

[Table("claims")]
public class Claims
{
    [Required]
    public User User { get; set; }
    [Required]
    public string Key { get; set; }
    [Required]
    public string Value { get; set; }
}
