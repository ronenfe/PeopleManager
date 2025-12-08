using System.ComponentModel.DataAnnotations;

namespace PeopleManager.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} הינו שדה חובה")]
        [Display(Name = "שם מלא")]
        public string FullName { get; set; }

        [Display(Name = "טלפון")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} הינו שדה חובה")]
        [EmailAddress(ErrorMessage = "האימייל אינו חוקי")]
        [Display(Name = "אימייל")]
        public string Email { get; set; }

        [Display(Name = "קובץ תמונה")]
        public string PhotoFileName { get; set; }

        // Stored image bytes (optional). When present, used for list and PDF export.
        public byte[] PhotoData { get; set; }

        // Content type of the uploaded image (e.g. image/png)
        public string PhotoContentType { get; set; }
    }
}