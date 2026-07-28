namespace MoviePlatform1.DAL.Dto.Response
{
    public class RegisterResponse
    {
        public string Message {  get; set; }
        public bool success {  get; set; }
        public List< string> error { get; set; }
    }
}
