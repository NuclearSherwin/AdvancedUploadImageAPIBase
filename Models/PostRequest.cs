using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FileUploadBase.Models;

public class PostRequest
{
    public int UserId { get; set; }


    public string Description { get; set; }

  
    [NotMapped]
    public IFormFile Image { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]


    public string? ImagePath { get; set; }

}