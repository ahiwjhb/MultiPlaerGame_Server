using MultiPlayerGame.Dao;
using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientRegisterRequest(MessageChannel sender, RegisterRequest request) {
            string username = request.Username;
            string password = request.Password;

            var requestResult = new RequestResult() {
                ActionCode = ActionCode.Register,
                IsSuccessful = false,
                Information = string.Empty
            };

            try {
                if (UserTableDao.QueryIDByUsername(username) != null) {
                    requestResult.Information = "用户名已经存在";
                }
                else {
                    UserTableDao.InsertToTable(username, password);
                    requestResult.IsSuccessful = true;
                }
            }
            catch (Exception e) {
                requestResult.Information = e.Message;
            }

            sender.SendMessageAsync(requestResult);
        }
    }
}
