namespace TheWorld.Services{
	
	
	public interface IMailServices{
		bool SendMail(string to, string from, string subject, string body);
	}
}