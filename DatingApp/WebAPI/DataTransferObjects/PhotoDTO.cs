using WebAPI.Entities;

namespace WebAPI.DataTransferObjects
{
    public class PhotoDTO
    {
        public int Id { get; set; }

        public string? Url { get; set; }

        public bool IsMain { get; set; }
    }
}
