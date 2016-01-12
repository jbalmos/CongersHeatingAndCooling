using log4net;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace PricingUpdateListenerService
{
    public partial class PricingUpdateListenerService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PricingUpdateListenerService));

        private Task _processWorksheetRequestTask;
        private CancellationTokenSource _cancellationTokenSource;

        public PricingUpdateListenerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogInfo("Queuing Background Task");
            _cancellationTokenSource = new CancellationTokenSource();
            _processWorksheetRequestTask = Task.Run(() => DoWorkAsync(_cancellationTokenSource.Token));
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            LogInfo("Shutting down");
            _cancellationTokenSource.Cancel();
            try
            {
                _processWorksheetRequestTask.Wait();
            }
            catch (Exception ex)
            {
                // handle exeption
                LogError(string.Format("{0}{1}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace), ex);
            }
        }
        public async Task DoWorkAsync(CancellationToken token)
        { 
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    DateTime startTime = DateTime.Now;

                    IList<Message> messages = FetchAllMessages(
                        ConfigurationManager.AppSettings["popHostName"],
                        Int32.Parse(ConfigurationManager.AppSettings["popPort"]),
                        true,
                        ConfigurationManager.AppSettings["popUserName"],
                        ConfigurationManager.AppSettings["popPassword"]);

                    ProcessMessages(messages);

                    log.Info("Done processing pending requests, sleeping for 5 minutes.");
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
                catch (TaskCanceledException ex)
                {
                    log.Info("Cancellation request recieved");
                    return;
                }
                catch (Exception ex)
                {
                    // Handle exception
                    LogError(string.Format("{0}{1}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace), ex);
                }
            }
        }

        public void ProcessMessages( IList<Message> messages )
        {
            foreach (Message message in messages)
            {
                if(message.Headers.Subject.Contains("Price Update") && message.MessagePart.IsText )
                {
                    string content = message.MessagePart.GetBodyAsText();

                }
            }
        }

        public static List<Message> FetchAllMessages(string hostname, int port, bool useSsl, string username, string password)
        {
            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                {
                    allMessages.Add(client.GetMessage(i));
                }

                //client.DeleteAllMessages();
                // Now return the fetched messages
                return allMessages;
            }
        }



        private void LogInfo(string message)
        {
            log.Info(message);
        }

        private void LogError(string message, Exception ex)
        {
            log.Error(message, ex);
        }
    }
}
